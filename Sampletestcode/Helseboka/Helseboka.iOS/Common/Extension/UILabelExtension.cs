using System;
using UIKit;

namespace Helseboka.iOS.Common.Extension
{
    public static class UILabelExtension
    {
        public static void SetBackgroundImage(this UILabel label, UIImage image)
        {
            label.BackgroundColor = UIColor.FromPatternImage(image);
        }
    }
}
