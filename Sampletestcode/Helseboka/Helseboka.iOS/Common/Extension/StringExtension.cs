using System;
using Foundation;

namespace Helseboka.iOS.Common.Extension
{
    public static class StringExtension
    {
        public static String Translate(this String message)
        {
            return NSBundle.MainBundle.GetLocalizedString(message);
        }
    }
}
