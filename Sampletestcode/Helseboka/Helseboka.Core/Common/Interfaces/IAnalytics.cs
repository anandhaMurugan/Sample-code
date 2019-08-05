using System;
using System.Collections.Generic;
using Helseboka.Core.Common.EnumDefinitions;

namespace Helseboka.Core.Common.Interfaces
{
    public interface IAnalytics
    {
        void Initialize();

        void TrackEvent(HelsebokaEvent eventDetails, Dictionary<String, String> properties = null);

        void TrackEvent(String eventName, Dictionary<String, String> properties = null);

        void TrackError(Exception exception, Dictionary<String, String> properties = null);
    }
}
