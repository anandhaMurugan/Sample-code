using System;
using Foundation;
using Helseboka.Core.Auth.Model;
using Helseboka.Core.Common.Constant;
using Helseboka.Core.Common.Interfaces;
using Helseboka.Core.Common.Model;
using UIKit;

namespace Helseboka.iOS.Common.Utilities
{
    public class InActivityHandler
    {
        #region Singleton Implementation
        //Lazy singleton initialization
        // http://csharpindepth.com/Articles/General/Singleton.aspx
        // Double-check-Locking will not be required - https://msdn.microsoft.com/en-us/library/ff650316.aspx
        private InActivityHandler()
        {
            inactivityTimer = NSTimer.CreateScheduledTimer(TimeSpan.FromSeconds(AppConstant.InactivityTimeOut), InActivityTimer_Elapsed);
        }

        public static InActivityHandler Instance { get { return Nested.instance; } }

        private class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }

            internal static readonly InActivityHandler instance = new InActivityHandler();
        }
        #endregion

        private NSTimer inactivityTimer;
        private double lastActivityElapsedTime;
        private IAnalytics EventTracker => ApplicationCore.Container.Resolve<IAnalytics>();

        void InActivityTimer_Elapsed(NSTimer obj)
        {
            EventTracker.TrackEvent(Core.Common.EnumDefinitions.HelsebokaEvent.InactivityLogoutWhileAppInForeground);
            InactivityLogout();
        }

        private void InactivityLogout()
        {
            if (UIApplication.SharedApplication.Delegate is AppDelegate appDelegate)
            {
                AuthService.Instance.Logout();
                appDelegate.InitialRouter.InactivityLogout();
            }
        }

        public void Reset()
        {
            inactivityTimer.Invalidate();
            inactivityTimer.Dispose();
            inactivityTimer = NSTimer.CreateScheduledTimer(TimeSpan.FromSeconds(AppConstant.InactivityTimeOut), InActivityTimer_Elapsed);
            lastActivityElapsedTime = NSProcessInfo.ProcessInfo.SystemUptime;
        }

        public void Stop()
        {
            inactivityTimer.Invalidate();
        }

        public void ApplicationResumes()
        {
            var currentTime = NSProcessInfo.ProcessInfo.SystemUptime;
            if(currentTime - lastActivityElapsedTime >= AppConstant.InactivityTimeOut)
            {
                EventTracker.TrackEvent(Core.Common.EnumDefinitions.HelsebokaEvent.InactivityLogoutWhileAppInBacground);
                InactivityLogout();
            }
        }

    }
}
