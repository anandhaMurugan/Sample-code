using System;
using UIKit;

namespace Helseboka.iOS.Common.Constant
{
    public static class Fonts
    {
        public static UIFont GetNormalFont(int size)
        {
            return UIFont.FromName("AvenirNext-Regular", size);
        }

        public static UIFont GetMediumFont(int size)
        {
            return UIFont.FromName("AvenirNext-Medium", size);
        }

        public static UIFont GetBoldFont(int size)
        {
            return UIFont.FromName("AvenirNext-Bold", size);
        }
		public static UIFont GetHeavyFont(int size)
        {
            return UIFont.FromName("AvenirNext-Heavy", size);
        }
    }
}
