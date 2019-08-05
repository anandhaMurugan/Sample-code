using System;
using CoreAnimation;
using CoreGraphics;
using Helseboka.iOS.Constants;
using UIKit;

namespace Helseboka.iOS.Components.Views
{
    public class GradientLayer : CALayer
    {
        public GradientLayer() : base()
        {
            this.SetNeedsDisplay();
        }

        public GradientLayer(IntPtr handle) : base(handle)
        {
            this.SetNeedsDisplay();
        }

        public override void DrawInContext(CGContext ctx)
        {
            base.DrawInContext(ctx);

            var frame = Frame;
            var gradient = new CGGradient(CGColorSpace.CreateDeviceRGB(), new CGColor[] { Colors.Turquoise.CGColor, Colors.Purple.CGColor });
            var startPoint = new CGPoint(frame.Left + 0.25 * frame.Width, frame.Top + 0.2 * frame.Height);
            var endPoint = new CGPoint(frame.Right - 0.25 * frame.Width, frame.Bottom + 0.5 * frame.Height);
            var options = CGGradientDrawingOptions.DrawsBeforeStartLocation | CGGradientDrawingOptions.DrawsAfterEndLocation;
            ctx.DrawLinearGradient(gradient, startPoint, endPoint, options);
        }
    }
}
