using System;

namespace Helseboka.Core.Profile.Model
{
    public class AddPushNotificationTokenRequest
    {
        public String DevicePlatform { get; set; }
        public String DeviceToken { get; set; }
    }
}
