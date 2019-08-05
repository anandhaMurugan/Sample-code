using System;
using System.Globalization;
using Foundation;
using Helseboka.Core.Common.Interfaces;
using LocalAuthentication;
using UIKit;

namespace Helseboka.iOS.Common.CommonImpl
{
	public class DeviceHandler : IDeviceHandler
    {
        public DeviceHandler()
        {
        }

        public string GetLanguageCode()
		{
            return NSLocale.CurrentLocale.LanguageCode == "en" ? "en" : "nb";
		}

		public bool IsFaceIDSupported()
		{
			var context = new LAContext();
			if (context.CanEvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, out var authError))
			{
				if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
				{
					return context.BiometryType == LABiometryType.FaceId;
				}
				else
				{
					return false;
				}
			}
			else
			{
				return false;
			}
		}

		public bool IsTouchIDSupported()
		{
			var context = new LAContext();
			if (context.CanEvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, out var authError))
            {
				return context.BiometryType == LABiometryType.TouchId;
            }
            else
            {
                return false;
            }
		}
	}
}
