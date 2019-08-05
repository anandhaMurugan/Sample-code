using System;
using System.Collections.Generic;
using Helseboka.Core.Common.EnumDefinitions;
using Helseboka.Core.Common.Interfaces;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace Helseboka.Droid.Common.CommonImpl
{
    public class AnalyticsHandler : IAnalytics
    {
        public void Initialize()
        {
#if !DEBUG
            AppCenter.Start("137720bc-51a5-4f0c-be1a-216bf2a346b1", typeof(Analytics), typeof(Crashes));
#endif
        }

        public void TrackError(Exception exception, Dictionary<string, string> properties = null)
        {
#if !DEBUG
            Crashes.TrackError(exception, properties);
#endif
        }

        public void TrackEvent(HelsebokaEvent eventDetails, Dictionary<string, string> properties = null)
        {
            TrackEvent(eventDetails.ToString(), properties);
        }

        public void TrackEvent(string eventName, Dictionary<string, string> properties = null)
        {
#if !DEBUG
            Analytics.TrackEvent(eventName, properties);
#endif
        }
    }
}
