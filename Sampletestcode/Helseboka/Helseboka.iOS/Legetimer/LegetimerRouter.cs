using System;
using Helseboka.Core.AppointmentModule.Interface;
using Helseboka.Core.AppointmentModule.Model;
using Helseboka.Core.AppointmentModule.Presenter;
using Helseboka.iOS.Legetimer.View;
using Helseboka.iOS.Common.Extension;
using UIKit;

namespace Helseboka.iOS.Legetimer
{
    public class LegetimerRouter : UINavigationController, IAppointmentRouter
    {
        private UIStoryboard legetimerStoryboard;
        private IAppointmentPresenter presenter;
		public LegetimerRouter() 
		{
            legetimerStoryboard = UIStoryboard.FromName("Legetimer", null);
            presenter = new AppointmentPresenter(this);
            var legetimerListView = legetimerStoryboard.InstantiateViewController(LegetimerListView.Identifier) as LegetimerListView;
            legetimerListView.Presenter = presenter;
            legetimerListView.TabBarItem = new UITabBarItem("Home.TabBar.Appointment.Title".Translate(), UIImage.FromBundle("Legetimer"), UIImage.FromBundle("Legetimer-active"));

            this.ViewControllers = new UIViewController[] { legetimerListView };

			NavigationBar.Hidden = true;
		}

        public void Finish()
        {
            
        }

        public void GoBackToHome()
        {
            PopToRootViewController(true);
        }

        public void GoBackToDateSelection()
        {
            PopViewController(true);
        }

        public void ShowAppointmentDateSelectionView()
        {
            var dateSelectionView = legetimerStoryboard.InstantiateViewController(AppointmentDateSelectionView.Identifier) as AppointmentDateSelectionView;
            dateSelectionView.Presenter = presenter;
            PushViewController(dateSelectionView, true);
        }

        public void ShowAddSymptomView()
        {
            var addSymptomView = legetimerStoryboard.InstantiateViewController(AddSymptomsView.Identifier) as AddSymptomsView;
            addSymptomView.Presenter = presenter;
            PushViewController(addSymptomView, true);
        }

        public void ShowConfirmationView()
        {
            var confirmationView = legetimerStoryboard.InstantiateViewController(AppointmentConfirmationView.Identifier) as AppointmentConfirmationView;
            confirmationView.Presenter = presenter;
            PushViewController(confirmationView, true);
        }

        public void ShowAppointmentDetailsView(AppointmentDetails appointment)
        {
            var detailsView = legetimerStoryboard.InstantiateViewController(AppointmentDetailsView.Identifier) as AppointmentDetailsView;
            detailsView.Presenter = presenter;
            detailsView.Appointment = appointment;
            PushViewController(detailsView, true);
        }

        public void Start()
		{
			
		}
    }
}
