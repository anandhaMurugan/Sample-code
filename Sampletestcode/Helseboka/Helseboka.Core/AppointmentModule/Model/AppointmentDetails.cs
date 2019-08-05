using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Helseboka.Core.AppointmentModule.Interface;
using Helseboka.Core.Common.CommonImpl;
using Helseboka.Core.Common.Constant;
using Helseboka.Core.Common.EnumDefinitions;
using Helseboka.Core.Common.Interface;
using Helseboka.Core.Common.Model;

namespace Helseboka.Core.AppointmentModule.Model
{
    public class AppointmentDetails : AppointmentInfo
    {
        public int Id { get; set; }
        public DateTime? AppointmentTime { get; set; }
        public String DoctorReplyText { get; set; }
        public AppointmentStatus Status { get; set; }
        public DateTime? RequestedTime { get; set; }
        public String DoctorFocusedReply { get; set; }
        public bool IsVideoConsultationConfirmed { get => CheckVideoUrlAvailability(); }

        public bool IsCancellationPossible
        {
            get => Status != AppointmentStatus.Cancelled && ((AppointmentTime.HasValue && AppointmentTime > DateTime.Now) || !AppointmentTime.HasValue);
        }

        public async Task<Response<Doctor>> GetDoctor()
        {
            if (Doctor == null)
            {
                var dataHandler = ApplicationCore.Container.Resolve<IARService>();
                var response = await dataHandler.GetDoctorDetails(DoctorId.ToString());
                if (response.IsSuccess)
                {
                    Doctor = response.Result;
                    return response;
                }
                else
                {
                    return response;
                }
            }
            else
            {
                return Response<Doctor>.GetSuccessResponse(Doctor);
            }
        }

        public async Task<Response> Cancel()
        {
            var dataHandler = ApplicationCore.Container.Resolve<IAppointmentAPI>();
            return await dataHandler.CancelAppointment(Id.ToString());
        }

        private bool CheckVideoUrlAvailability()
        {
            if (!string.IsNullOrEmpty(DoctorFocusedReply) || !string.IsNullOrEmpty(DoctorReplyText))
            {
                Regex regex = new Regex(AppConstant.RegexUrlMatchPattern);
                if (AppConstant.RegexIsMatch(DoctorFocusedReply, DoctorReplyText, regex))
                {
                    return true;
                }

                foreach (string info in AppConstant.VideoConfirmationInfos)
                {               
                    if(AppConstant.Contains(DoctorFocusedReply, DoctorReplyText, info, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
