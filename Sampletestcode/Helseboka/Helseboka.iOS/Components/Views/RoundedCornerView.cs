using System;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Helseboka.iOS.Components.Views
{
    [Register("RoundedCornerView")]
    public class RoundedCornerView : UIView
    {
        public CACornerMask CornerMask = CACornerMask.MinXMinYCorner | CACornerMask.MaxXMinYCorner | CACornerMask.MinXMaxYCorner | CACornerMask.MaxXMaxYCorner;

        public RoundedCornerView(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override UIColor BackgroundColor
        {
            get { return UIColor.FromCGColor(Layer.BackgroundColor); }
            set { Layer.BackgroundColor = value.CGColor; }
        }

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);
            Layer.CornerRadius = 25;
            if (Layer.MaskedCorners != CornerMask)
            {
                Layer.MaskedCorners = CornerMask;
            }
        }

    }
}
