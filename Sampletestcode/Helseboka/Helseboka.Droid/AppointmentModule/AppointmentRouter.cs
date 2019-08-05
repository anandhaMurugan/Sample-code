using System;
using Helseboka.Core.AppointmentModule.Interface;
using Helseboka.Core.AppointmentModule.Model;
using Helseboka.Core.AppointmentModule.Presenter;
using Helseboka.Droid.AppointmentModule.Views;
using Helseboka.Droid.Common.EnumDefinitions;
using Helseboka.Droid.Common.Interfaces;
using Helseboka.Droid.Common.Router;
using Helseboka.Droid.Common.Views;

namespace Helseboka.Droid.AppointmentModule
{
    public class AppointmentRouter : BaseRouter, IAppointmentRouter
    {
        private AppointmentPresenter presenter;
        private AppointmentDateSelectionFragment appointmentDateSelectionFragment;

        public AppointmentRouter(IActivity activity) : base(activity)
        {
            presenter = new AppointmentPresenter(this);
        }

        public void GoBackToDateSelection()
        {
            if(appointmentDateSelectionFragment == null)
            {
                appointmentDateSelectionFragment = new AppointmentDateSelectionFragment(presenter);
            }

            Activity.NavigateTo(appointmentDateSelectionFragment, TransitionEffect.Pop);
        }

        public void GoBackToHome()
        {
            var fragment = new AppointmentHomeFragment(presenter);
            Activity.NavigateTo(fragment, TransitionEffect.Pop);
            appointmentDateSelectionFragment = null;
        }

        public void ShowAddSymptomView()
        {
            var fragment = new AppointmentSymptomSelectionFragment(presenter);
            Activity.NavigateTo(fragment, TransitionEffect.Push);
        }

        public void ShowAppointmentDateSelectionView()
        {
            appointmentDateSelectionFragment = new AppointmentDateSelectionFragment(presenter);
            Activity.NavigateTo(appointmentDateSelectionFragment, TransitionEffect.Push);
        }

        public void ShowAppointmentDetailsView(AppointmentDetails appointment)
        {
            var fragment = new AppointmentOverviewFragment(presenter, appointment);
            Activity.NavigateTo(fragment, TransitionEffect.Push);
        }

        public void ShowConfirmationView()
        {
            appointmentDateSelectionFragment = null;
            var fragment = new AppointmentConfirmationFragment(presenter);
            Activity.NavigateTo(fragment, TransitionEffect.Push);
        }

        public BaseFragment GetCurrentFragment()
        {
            return new AppointmentHomeFragment(presenter);
        }
    }
}
