using System;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Helseboka.iOS.Components.Views
{
    [Register("GradientView")]
    public class GradientView : UIView
    {
        private GradientLayer _gradientLayer;

        public GradientView(IntPtr handle) : base(handle)
        {
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            _gradientLayer = new GradientLayer
            {
                Frame = Bounds
            };

            Layer.AddSublayer(_gradientLayer);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            _gradientLayer.Frame = Bounds;
        }
    }
}
