using System;
using System.Collections.Generic;
using Helseboka.Core.Common.EnumDefinitions;
using Helseboka.Core.Common.Interfaces;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace Helseboka.iOS.Common.CommonImpl
{
    public class AnalyticsHandler : IAnalytics
    {
        public void Initialize()
        {
#if !DEBUG
            AppCenter.Start("49f92bf2-8e23-4bd8-8c98-ccc1df96bc69", typeof(Analytics), typeof(Crashes));
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
