using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Helseboka.Core.AppointmentModule.Model;
using Helseboka.Core.Common.EnumDefinitions;
using Helseboka.Core.Common.Interfaces;
using Helseboka.Core.Common.Model;

namespace Helseboka.Core.AppointmentModule.Interface
{
    public interface IAppointmentPresenter : IBasePresenter
    {
        bool HasMoreData
        {
            get;
        }

        void AddAppointment(List<String> symptoms);
        Task<AppointmentCollection> GetAllAppointments();
        Task<bool> LoadMore(AppointmentCollection appointments);
        Task<Doctor> GetDoctorDetails(AppointmentDetails appointment);
        Task<Response> CancelAppointment(AppointmentDetails appointment);
        void GoBackToHome();
        void GoBackToDateSelection();
        void ShowAppointmentDetails(AppointmentDetails appointment);
        void ShowAppointmentDateSelectionView();
        void DidSelectAppointmentDateTime(List<DayOfWeek> days, List<TimeOfDay> times);
        Task<Response<Doctor>> GetDoctor();
    }
}