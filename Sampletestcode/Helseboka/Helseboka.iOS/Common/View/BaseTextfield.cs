using System;
using System.ComponentModel;
using CoreGraphics;
using Foundation;
using Helseboka.iOS.Common.Constant;
using UIKit;

namespace Helseboka.iOS.Common.View
{
    [Register("BaseTextfield"), DesignTimeVisible(true)]
    public class BaseTextfield : UITextField
    {
        private UIEdgeInsets padding = new UIEdgeInsets(0, 0, 0, 0);
        public UIEdgeInsets Padding
        {
            get => padding;
            set
            {
                padding = value;
                LayoutSubviews();
            }
        }

        public BaseTextfield()
        {
            CommontInitialization();
        }

        public BaseTextfield(IntPtr handler) : base(handler) { }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            CommontInitialization();
        }

        protected void CommontInitialization()
        {
            this.Font = Fonts.GetBoldFont(16);
            SetInactiveBackground();

			this.EditingDidBegin += Handle_EditingDidBegin;
			this.EditingDidEnd += Handle_EditingDidEnd;
        }

		void Handle_EditingDidBegin(object sender, EventArgs e)
		{
            SetActiveBackground();
		}

		void Handle_EditingDidEnd(object sender, EventArgs e)
		{
            SetInactiveBackground();
		}


        public override CGRect TextRect(CGRect forBounds)
        {
            return base.TextRect(padding.InsetRect(forBounds));
        }

        //public override CGRect PlaceholderRect(CGRect forBounds)
        //{
        //    return base.PlaceholderRect(padding.InsetRect(forBounds));
        //}

        public override CGRect EditingRect(CGRect forBounds)
        {
            return base.EditingRect(padding.InsetRect(forBounds));
        }

        private void SetInactiveBackground()
        {
            //Background = UIImage.FromBundle("Textbox-inactive-background");

            this.Layer.BorderColor = Colors.UnselectedLabelBorderColor.CGColor;
            this.Layer.BorderWidth = 1;
            this.BackgroundColor = Colors.SearchResultBackground;
        }

        private void SetActiveBackground()
        {
            //Background = UIImage.FromBundle("Textbox-active-background");

            this.Layer.BorderColor = Colors.SelectedLabelBorderColor.CGColor;
            this.Layer.BorderWidth = 2;
            this.BackgroundColor = Colors.SearchResultBackground;
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            this.Layer.CornerRadius = this.Frame.Height / 2;
        }


    }
}
