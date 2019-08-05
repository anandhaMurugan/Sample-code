using System;
using System.Globalization;
using Android.Content;
using Helseboka.Core.Common.Interfaces;
using Java.Util;

namespace Helseboka.Droid.Common.CommonImpl
{
    public class DeviceHandler : IDeviceHandler
    {
        private Context context;
        public DeviceHandler(Context context)
        {
            this.context = context;
        }

        public string GetLanguageCode()
        {
            //var language = Locale.Default.Language;
            var language = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            return language == "en" ? "en" : "nb";
        }

        public bool IsFaceIDSupported()
        {
            return false;
        }

        public bool IsTouchIDSupported()
        {
            return false;
        }
    }
}
