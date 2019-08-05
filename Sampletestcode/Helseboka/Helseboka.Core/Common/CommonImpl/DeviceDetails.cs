using Helseboka.Core.Common.EnumDefinitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Helseboka.Core.Common.CommonImpl
{
    public class DeviceDetails
    {
        public static DeviceDetails Instance { get { return Nested.instance; } }

        public String AppVersion { get; set; }

        private class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }

            internal static readonly DeviceDetails instance = new DeviceDetails();
        }

        public PlatformType GetDevicePlatform()
        {
            if(Device.RuntimePlatform == Device.iOS)
            {
                return PlatformType.IOS;
            }
            else if(Device.RuntimePlatform == Device.Android)
            {
                return PlatformType.ANDROID;
            }
            return PlatformType.UNKNOWN;
        }
    }
}
