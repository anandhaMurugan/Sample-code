using System;
using CoreGraphics;
using Foundation;
using Helseboka.iOS.Common.Constant;
using Helseboka.iOS.Common.View;
using UIKit;

namespace Helseboka.iOS.Common.TableViewCell
{
	public partial class HelpTableViewCell : BaseTableViewCell
    {
        public static readonly NSString Key = new NSString("HelpTableViewCell");
        public static readonly UINib Nib;

        static HelpTableViewCell()
        {
            Nib = UINib.FromName("HelpTableViewCell", NSBundle.MainBundle);
        }

		private String DescriptionText { get; set; }

        public HelpTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

		public static HelpTableViewCell Create()
        {
			return (HelpTableViewCell)Nib.Instantiate(null, null)[0];
        }

        public void Configure(String title, String description, Action<NSUrl> onLinkTap)
        {
			HelpTitle.Text = title;
            DescriptionText = description;
            HelpDescription.Delegate = new LinkDelegate(onLinkTap);
            TextContainerView.Layer.BorderColor = Colors.ExpanderBorderColor.CGColor;
            TextContainerView.Layer.BorderWidth = 1;
            TextContainerView.Layer.CornerRadius = 15;
            SelectionStyle = UITableViewCellSelectionStyle.None;
        }

        public void Expand()
        {
			HelpDescription.Text = DescriptionText;
            TextContainerView.BackgroundColor = Colors.ExpanderFillColor;
            TitleBottomConstraint.Constant = 15;
            DescriptionBottomConstraint.Constant = 15;
			RotateImage(true);
        }

        public void Collapse()
        {
			HelpDescription.Text = String.Empty;
            TextContainerView.BackgroundColor = UIColor.White;
            TitleBottomConstraint.Constant = 10;
            DescriptionBottomConstraint.Constant = 0;
			RotateImage(false);
        }

		private void RotateImage(bool isUp)
		{
			UIView.Animate(0.3, 0, UIViewAnimationOptions.CurveLinear, () =>
			{
				float rotation;
				var current = Math.Atan2(ArrowImage.Transform.yx, ArrowImage.Transform.xx);
                // Initial state. Image is >
				if (current <= 0.000001)
				{
					rotation = isUp ? (((float)Math.PI / 2) + ((float)Math.PI)) : ((float)Math.PI / 2);
				}
				// cell is collapsed. Image is ⋁
				else if (Math.Abs(((float)Math.PI / 2) - current) < 0.0001)
				{
					rotation = isUp ? (float)Math.PI : 0;
				}
				// cell is open. Image is ⋀
				else
				{
					rotation = isUp ? 0 : -(float)Math.PI;
				}
				ArrowImage.Transform = CGAffineTransform.MakeRotation(rotation);
			}, null);
		}
    }
}
