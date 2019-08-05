using System;
using Helseboka.Core.AppointmentModule.Model;
using Helseboka.Core.Common.Interfaces;

namespace Helseboka.Core.AppointmentModule.Interface
{
    public interface IAppointmentRouter : IBaseRouter
    {
        void ShowAppointmentDetailsView(AppointmentDetails appointment);

        void ShowAppointmentDateSelectionView();

        void GoBackToHome();

        void GoBackToDateSelection();

        void ShowAddSymptomView();

        void ShowConfirmationView();
    }
}
