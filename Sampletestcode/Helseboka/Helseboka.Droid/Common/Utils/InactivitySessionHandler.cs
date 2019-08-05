using System;
using System.Timers;
using Helseboka.Core.Auth.Model;
using Helseboka.Core.Common.Constant;
using Helseboka.Core.Common.Interfaces;
using Helseboka.Core.Common.Model;

namespace Helseboka.Droid.Common.Utils
{
    public class InactivitySessionHandler
    {
        private Timer inactivityTimer;
        private long lastinteractionTime;
        private IAnalytics EventTracker => ApplicationCore.Container.Resolve<IAnalytics>();

        public event EventHandler InactivityLogout;

        public InactivitySessionHandler()
        {
            inactivityTimer = new Timer(AppConstant.InactivityTimeOut * 1000);
            inactivityTimer.Elapsed += InactivityTimer_Elapsed;
        }


        public void ExtendSession()
        {
            inactivityTimer.Stop();
            inactivityTimer.Start();
            lastinteractionTime = Android.OS.SystemClock.ElapsedRealtime();
        }

        public void ReportApplicationPause()
        {
            inactivityTimer.Stop();
        }

        public void ReportApplicationResume()
        {
            var currentTime = Android.OS.SystemClock.ElapsedRealtime();
            if(currentTime - lastinteractionTime >= AppConstant.InactivityTimeOut * 1000)
            {
                EventTracker.TrackEvent(Core.Common.EnumDefinitions.HelsebokaEvent.InactivityLogoutWhileAppInBacground);
                InActivityLogout();
            }
        }

        void InactivityTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            EventTracker.TrackEvent(Core.Common.EnumDefinitions.HelsebokaEvent.InactivityLogoutWhileAppInForeground);
            inactivityTimer.Stop();
            InActivityLogout();
        }

        private void InActivityLogout()
        {
            AuthService.Instance.Logout();
            InactivityLogout?.Invoke(this, EventArgs.Empty);
        }
    }
}
