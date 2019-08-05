using System;
using Helseboka.iOS.Common.PlatformEnums;
using UIKit;

namespace Helseboka.iOS.Common.Utilities
{
	public static class Device
    {
		public static DeviceType DeviceType
        {
			get
			{
				switch ((int)UIScreen.MainScreen.NativeBounds.Height)
                {
                    case 960: return DeviceType.iPhone4_4S;
                    case 1136: return DeviceType.iPhones_5_5s_5c_SE;
                    case 1334: return DeviceType.iPhones_6_6s_7_8;
                    case 1920:
                    case 2208: return DeviceType.iPhones_6Plus_6sPlus_7Plus_8Plus;
                    case 2436: return DeviceType.iPhoneX;
                    default: return DeviceType.Unknown;
                }
			}
        }
    }
}
