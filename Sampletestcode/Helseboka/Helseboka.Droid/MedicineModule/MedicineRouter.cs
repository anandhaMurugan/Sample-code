using System;
using Android.Support.V4.App;
using Helseboka.Core.MedicineModule.Interface;
using Helseboka.Core.MedicineModule.Model;
using Helseboka.Core.MedicineModule.Presenter;
using Helseboka.Droid.Common.EnumDefinitions;
using Helseboka.Droid.Common.Interfaces;
using Helseboka.Droid.Common.Router;
using Helseboka.Droid.Common.Views;
using Helseboka.Droid.MedicineModule.Views;

namespace Helseboka.Droid.MedicineModule
{
    public class MedicineRouter : BaseRouter, IMedicineRouter
    {
        private MedicineOverviewFragment overviewFragment;

        public MedicineRouter(IActivity activity) : base(activity)
        {
        }

        public void NavigateToSetMedicineAlarm(MedicineReminder medicine)
        {
            var fragment = new MedicineAlarmFragment(new MedicineHomePresenter(this), medicine);
            Activity.NavigateTo(fragment, TransitionEffect.Push);
        }

        public void NavigateToMedicineOverview(MedicineReminder medicine, bool isFromSearch)
        {
            overviewFragment = new MedicineOverviewFragment(new MedicineHomePresenter(this), isFromSearch, medicine);
            Activity.NavigateTo(overviewFragment, TransitionEffect.Push);
        }

        public void GoBackToHome()
        {
            Activity.NavigateTo(new MedicineHomeFragment(new MedicineHomePresenter(this)), TransitionEffect.Pop);
        }

        public void NavigateBackToOverviewFromAlarm()
        {
            if (overviewFragment != null)
            {
                Activity.NavigateTo(overviewFragment, TransitionEffect.Pop);
            }
        }

        public BaseFragment GetCurrentFragment()
        {
            return new MedicineHomeFragment(new MedicineHomePresenter(this));
        }
    }
}
