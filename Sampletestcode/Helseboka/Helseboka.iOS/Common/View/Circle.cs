using System;
using System.ComponentModel;
using CoreGraphics;
using Foundation;
using Helseboka.iOS.Common.Constant;
using UIKit;

namespace Helseboka.iOS.Common.View
{
	[Register("Circle"), DesignTimeVisible(true)]
	public class Circle : UIView
	{
		public UIView InnerCircle { get; set; }

		[Export("InnerCircleRatio"), Browsable(true)]
		public float InnerCircleRatio { get; set; } = 0.6f;

		public Circle(IntPtr handle) : base(handle) { }

		public Circle()
		{
			Initialize();
		}

		public override void AwakeFromNib()
		{
			Initialize();
		}

		public void Initialize()
		{
			if (InnerCircle == null)
			{
				InnerCircle = new UIView();
				AddSubview(InnerCircle);
				HideInnerCircle();
			}

			this.Frame = new CGRect(Frame.Location, new CGSize(Frame.Size.Height, Frame.Size.Width));
            Layer.CornerRadius = Frame.Size.Height / 2;

			var inCircleHeight = Frame.Size.Height * InnerCircleRatio;
			var inCirclePadding = (Frame.Size.Height - inCircleHeight) / 2;

			InnerCircle.Frame = new CGRect(inCirclePadding, inCirclePadding, inCircleHeight, inCircleHeight);
			InnerCircle.Layer.CornerRadius = inCircleHeight / 2;

			BackgroundColor = Colors.FillColor;

			Layer.BorderWidth = 1;
			Layer.BorderColor = Colors.BorderColor.CGColor;

			InnerCircle.BackgroundColor = Colors.CircularFillColor;
		}

		public void ShowInnerCircle()
		{
			InnerCircle.Hidden = false;
		}

		public void HideInnerCircle()
        {
			InnerCircle.Hidden = true;
        }

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			Initialize();
		}
	}
}
