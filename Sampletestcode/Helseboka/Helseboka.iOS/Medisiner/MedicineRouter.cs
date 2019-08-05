using System;
using Helseboka.Core.MedicineModule.Interface;
using Helseboka.Core.MedicineModule.Model;
using Helseboka.Core.MedicineModule.Presenter;
using Helseboka.iOS.Medisiner.View;
using Helseboka.iOS.Common.Extension;
using UIKit;

namespace Helseboka.iOS.Medisiner
{
    public class MedicineRouter : UINavigationController, IMedicineRouter
    {
        private UIStoryboard MedicineStoryboard { get; set; }
        private MedicineHomePresenter presenter;

        public MedicineRouter()
        {
            presenter = new MedicineHomePresenter(this);

            MedicineStoryboard = UIStoryboard.FromName("Medicine", null);
            var medisinListView = MedicineStoryboard.InstantiateViewController("MedisinListView") as MedisinListView;
            medisinListView.TabBarItem = new UITabBarItem("Home.TabBar.Medicine.Title".Translate(), UIImage.FromBundle("Medisiner"), UIImage.FromBundle("Medisiner-active"));
            medisinListView.Presenter = presenter;

            this.ViewControllers = new UIViewController[] { medisinListView };
            NavigationBar.Hidden = true;
        }

        public void NavigateToSetMedicineAlarm(MedicineReminder medicine)
        {
            if(MedicineStoryboard.InstantiateViewController("MedicineAlarmView") is MedicineAlarmView alarmView)
            {
                alarmView.Medicine = medicine;
                alarmView.Presenter = presenter;
                PushViewController(alarmView, true);
            }
        }

        public void NavigateToMedicineOverview(MedicineReminder medicine, Boolean isFromSearch)
        {
            if (MedicineStoryboard.InstantiateViewController("MedicineOverview") is MedicineOverview medicineOverview)
            {
                medicineOverview.MedicineDetails = medicine;
                medicineOverview.Presenter = presenter;
                medicineOverview.IsFromSearch = isFromSearch;
                PushViewController(medicineOverview, true);
            }
        }

        public void NavigateBackToOverviewFromAlarm()
        {
            PopViewController(true);
        }

        public void GoBackToHome()
        {
            PopToRootViewController(true);
        }

        public void Start()
        {
            
        }

        public void Finish()
        {
            
        }
    }
}
