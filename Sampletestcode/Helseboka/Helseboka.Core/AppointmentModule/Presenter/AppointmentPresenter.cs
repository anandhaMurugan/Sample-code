using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Helseboka.Core.AppointmentModule.Interface;
using Helseboka.Core.AppointmentModule.Model;
using Helseboka.Core.Common.EnumDefinitions;
using Helseboka.Core.Common.Model;
using Helseboka.Core.Common.Presenter;
using Helseboka.Core.Common.Extension;

namespace Helseboka.Core.AppointmentModule.Presenter
{
    public class AppointmentPresenter : BasePresenter, IAppointmentPresenter
    {
        private IAppointmentRouter Router { get => router as IAppointmentRouter; }
        private const int pageSize = 15;
        private int nextPageToLoad = 0;
        private List<DayOfWeek> selectedDays;
        private List<TimeOfDay> availableTimes;

        public bool HasMoreData { get; private set; }


        public AppointmentPresenter(IAppointmentRouter appointmentRouter)
        {
            router = appointmentRouter;
        }

        public void AddAppointment(List<String> symptoms)
        {
            AddAppointmentInternal(symptoms).Forget();
        }

        private async Task AddAppointmentInternal(List<String> symptoms)
        {
            Loading();
            var response = await ApplicationCore.Instance.CurrentUser.AddAppointment(selectedDays, availableTimes, symptoms, String.Empty);
            HideLoading();
            if (response.IsSuccess)
            {
                Router.ShowConfirmationView();
            }
            else
            {
                RaiseError(response.ResponseInfo);
            }
        }

        public async Task<AppointmentCollection> GetAllAppointments()
        {
            nextPageToLoad = 0;
            return new AppointmentCollection(await LoadMore());
        }

        public async Task<bool> LoadMore(AppointmentCollection appointments)
        {
            if (appointments != null)
            {
                var response = await LoadMore();
                if (response != null && response.Count > 0)
                {
                    appointments.Update(response);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private async Task<List<AppointmentDetails>> LoadMore()
        {
            var response = await ApplicationCore.Instance.CurrentUser.GetAllAppointments(nextPageToLoad++, pageSize);
            if (response.IsSuccess && response.Result.Content != null)
            {
                HasMoreData = nextPageToLoad < response.Result.TotalPages;
                var dataList = response.Result.Content;
                foreach(var item in dataList)
                {
                    await GetDoctorDetails(item);
                }              
                return dataList;
            }
            else
            {
                RaiseError(response.ResponseInfo);
                return new List<AppointmentDetails>();
            }
        }

        public async Task<Response> CancelAppointment(AppointmentDetails appointment)
        {
            return await appointment.Cancel();
        }

        public void DidSelectAppointmentDateTime(List<DayOfWeek> days, List<TimeOfDay> times)
        {
            selectedDays = days;
            availableTimes = times;
            Router.ShowAddSymptomView();
        }

        public async Task<Doctor> GetDoctorDetails(AppointmentDetails appointment)
        {
            Loading();
            var response = await appointment.GetDoctor();
            HideLoading();
            if (response.IsSuccess)
            {
                return response.Result;
            }
            else
            {
                RaiseError(response.ResponseInfo);
                return null;
            }
        }

        public async Task<Response<Doctor>> GetDoctor()
        {
            Loading();
            var response = await ApplicationCore.Instance.CurrentUser.GetDoctor();
            HideLoading();
            if (!response.IsSuccess)
            {
                RaiseError(response.ResponseInfo);
            }

            return response;
        }

        public void ShowAppointmentDetails(AppointmentDetails appointment)
        {
            Router.ShowAppointmentDetailsView(appointment);
        }

        public void GoBackToHome()
        {
            Router.GoBackToHome();
        }

        public void ShowAppointmentDateSelectionView()
        {
            Router.ShowAppointmentDateSelectionView();
        }

        public void GoBackToDateSelection()
        {
            Router.GoBackToDateSelection();
        }
    }
}
