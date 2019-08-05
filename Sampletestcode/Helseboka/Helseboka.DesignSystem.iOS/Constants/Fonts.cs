using System;
using UIKit;

namespace Helseboka.DesignSystem.iOS.Constants
{
    public static class Fonts
    {
        public static UIFont Bold(nfloat size)
        {
            return UIFont.FromName("AvenirNext-Bold", size);
        }

        public static UIFont Medium(nfloat size)
        {
            return UIFont.FromName("AvenirNext-Medium", size);
        }

        public static UIFont Regular(nfloat size)
        {
            return UIFont.FromName("AvenirNext-Regular", size);
        }
    }
}
