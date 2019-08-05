using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Helseboka.Core.AppointmentModule.Model;
using Helseboka.Core.Common.CommonImpl;
using Helseboka.Core.Common.EnumDefinitions;
using Helseboka.Core.Common.Model;
using Helseboka.Core.Common.Extension;
using Helseboka.Core.MedicineModule.Model;
using Helseboka.Core.Auth.Model;
using Helseboka.Core.Common.Interface;
using Helseboka.Core.Profile.Interface;
using Helseboka.Core.MedicineModule.Interface;
using Helseboka.Core.Common.Interfaces;
using Helseboka.Core.AppointmentModule.Interface;

namespace Helseboka.Core.Profile.Model
{
    public class User
    {
        public Doctor AssignedDoctor { get; protected set; }

        public String ID { get; set; }

        public String FirstName { get; set; }

        public String LastName { get; set; }

        public String Phone { get; set; }

        public String SSN { get; set; }

        public String Gender { get; set; }

        public String Address { get; set; }

        public String DateOfBirth { get; set; }

        public String DoctorId { get; set; }

        public bool TermsValid { get; set; }

        public List<MedicineReminder> MedicineReminders { get; private set; }

        public bool HasDoctor()
        {
            return !String.IsNullOrEmpty(DoctorId);
        }

        public async Task<Response<Doctor>> GetDoctor()
        {
            if (HasDoctor())
            {
                if (AssignedDoctor == null)
                {
                    var dataHandler = ApplicationCore.Container.Resolve<IARService>();
                    var response = await dataHandler.GetDoctorDetails(DoctorId);
                    if (response.IsSuccess)
                    {
                        AssignedDoctor = response.Result;
                        return response;
                    }
                    else
                    {
                        return response;
                    }
                }
                else
                {
                    return Response<Doctor>.GetSuccessResponse(AssignedDoctor);
                }
            }
            else
            {
                return Response<Doctor>.GetGenericClientErrorResponse();
            }
        }

        public async Task<Response> UpdateDoctor(Doctor doctor)
        {
            var request = GetUpdateDoctorRequest(doctor);
            var dataHandler = ApplicationCore.Container.Resolve<IUserAPI>();
            var response = await dataHandler.UpdateUserInfo(request);
            if (response.IsSuccess)
            {
                this.DoctorId = doctor.Id;
                AssignedDoctor = null;
                return await GetDoctor();
            }
            else
            {
                return response;
            }
        }

        public async Task<Response> UpdateUserInfo(User request)
        {
            var dataHandler = ApplicationCore.Container.Resolve<IUserAPI>();
            return await dataHandler.UpdateUserInfo(request);
        }

        public async Task<Response<List<MedicineReminder>>> GetAllMedicineAndReminders()
        {
            var dataHandler = ApplicationCore.Container.Resolve<IMedicineAPI>();
            var response = await dataHandler.GetAllMedicineAndReminders();

            if (response.IsSuccess)
            {
                MedicineReminders = response.Result;
                if(MedicineReminders != null && MedicineReminders.Count > 0)
                {
                    var reminderListClone = MedicineReminders.Select(x => x.Clone()).ToList();
                    ApplicationCore.Container.Resolve<INotificationService>().ScheduleNotification(reminderListClone).Forget();
                }
            }

            return response;
        }

        public async Task<Response> AddAppointment(List<DayOfWeek> days, List<TimeOfDay> times, List<String> symptoms, String notes)
        {
            var dataHandler = ApplicationCore.Container.Resolve<IAppointmentAPI>();
            var request = new AppointmentInfo();
            request.DoctorId = Convert.ToInt32(DoctorId);
            request.RequestedDays = days;
            request.RequestedTimes = times;
            request.Topics = symptoms;
            request.Notes = notes;

            return await dataHandler.AddAppointment(request);
        }

        public async Task<Response<PaginationResponse<AppointmentDetails>>> GetAllAppointments(int pageNum, int pageSize)
        {
            var dataHandler = ApplicationCore.Container.Resolve<IAppointmentAPI>();
            return await dataHandler.GetAllAppointments(pageNum, pageSize);
        }

        private User GetUpdateDoctorRequest(Doctor doctor)
        {
            var user = this.Clone();

            user.DoctorId = doctor.Id;

            return user;
        }

        public async Task<Response> UpdateNotificationToken(String platform, String notificationToken)
        {
            var dataHandler = ApplicationCore.Container.Resolve<IUserAPI>();
            var request = new AddPushNotificationTokenRequest() { DevicePlatform = platform, DeviceToken = notificationToken };

            return await dataHandler.UpdateNotificationToken(request);
        }

        public async Task<Response<AppointmentDetails>> GetNextAppointment()
        {
            var dataHandler = ApplicationCore.Container.Resolve<IAppointmentAPI>();
            var response = await dataHandler.GetNextAppointment();
            if (response.IsSuccess && response.Result != null && response.Result.Id != 0)
            {
                var doctorResponse = await response.Result.GetDoctor();
                if (doctorResponse.IsSuccess)
                {
                    return response;
                }
                else
                {
                    return new Response<AppointmentDetails>(doctorResponse.ResponseInfo, null);
                }
            }
            else
            {
                // This endpoint returns 404 if there is no appointment scheduled
                if(response.ResponseInfo is BaseAPIErrorResponseInfo apierror && apierror.Error == APIError.NotFound)
                {
                    return Response<AppointmentDetails>.GetSuccessResponse(null);
                }

                return response;
            }
        }

        public async Task<Response<List<AlarmDetails>>> GetAlarms(DateTime date)
        {
            var dataHandler = ApplicationCore.Container.Resolve<IMedicineAPI>();
            var response = await dataHandler.GetAlarms(date.ToString("yyyy-MM-dd"));
            if (response.IsSuccess)
            {
                var result = new List<AlarmDetails>();

                AlarmDetails nextAlarm = null;
                long minDifference = long.MaxValue;

                if (response.Result != null)
                {
                    foreach (var medicineReminder in response.Result)
                    {
                        if (medicineReminder.Alarms != null)
                        {
                            foreach (var alarm in medicineReminder.Alarms)
                            {
                                var alarmDetails = new AlarmDetails(alarm, medicineReminder.Context);
                                result.Add(alarmDetails);

                                var timeDifference = alarmDetails.Time.Ticks - DateTime.Now.Ticks;
                                if (timeDifference < minDifference && timeDifference > 0)
                                {
                                    minDifference = timeDifference;
                                    nextAlarm = alarmDetails;
                                }
                            }
                        }
                    }

                    if (nextAlarm != null && date.Date == DateTime.Now.Date)
                    {
                        nextAlarm.IsNextAlarm = true;
                    }

                    result.Sort((x, y) => x.Time.CompareTo(y.Time));
                }

                return Response<List<AlarmDetails>>.GetSuccessResponse(result);
            }
            else
            {
                return new Response<List<AlarmDetails>>(response.ResponseInfo, null);
            }
        }

        public async Task<Response> DeleteProfile()
        {
            var dataHandler = ApplicationCore.Container.Resolve<IUserAPI>();
            var response = await dataHandler.DeleteProfile();
            if(response.IsSuccess)
            {
                AuthService.Instance.DeleteAllUserAuthData();
            }

            return response;
        }

        public User Clone()
        {
            var user = new User();
            user.Address = this.Address;
            user.DateOfBirth = this.DateOfBirth;
            user.DoctorId = this.DoctorId;
            user.FirstName = this.FirstName;
            user.Gender = this.Gender;
            user.ID = this.ID;
            user.LastName = this.LastName;
            user.Phone = this.Phone;
            user.SSN = this.SSN;

            return user;
        }
    }
}
