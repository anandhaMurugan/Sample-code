using System;
using Foundation;
using Helseboka.iOS.Common.Constant;
using Helseboka.iOS.Common.Extension;
using UIKit;

namespace Helseboka.iOS.Common.View.PopUpDialogs
{
    public partial class YesNoDialogView : BaseModalViewController
    {
        public event EventHandler LeftButtonTapped;
        public event EventHandler RightButtonTapped;
        private String titleText;
        private String messageText;
        private String leftButtonText;
        private String rightButtonText;

        public NSAttributedString AttributedMessage { get; set; }

        public YesNoDialogView(String title = "", String message = "", String leftButtonText = "", String rightButtonText = "") : base("YesNoDialogView")
        {
            titleText = title;
            messageText = message;
            this.leftButtonText = leftButtonText;
            this.rightButtonText = rightButtonText;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            DialogView.AddBorder(Colors.PopUpShadowColor, 13);
            DialogView.AddShadow(Colors.PopUpShadowColor, 13, 0, 0);

            if(!String.IsNullOrEmpty(titleText))
            {
                TitleLabel.Text = titleText;
            }

            if (!String.IsNullOrEmpty(messageText))
            {
                MessageLabel.Text = messageText;
            }

            if(AttributedMessage != null)
            {
                MessageLabel.AttributedText = AttributedMessage;
            }

            if(!String.IsNullOrEmpty(leftButtonText))
            {
                LeftButton.SetTitle(leftButtonText, UIControlState.Normal);
            }

            if (!String.IsNullOrEmpty(rightButtonText))
            {
                RightButton.SetTitle(rightButtonText, UIControlState.Normal);
            }
        }

        partial void Close_Tapped(UIButton sender)
        {
            Close();
        }

        partial void LeftButton_Tapped(UIButton sender)
        {
            LeftButtonTapped?.Invoke(this, EventArgs.Empty);
            LeftButton.BackgroundColor = Colors.PopUpSelectedButtonColor;
            Close();
        }

        partial void RightButton_Tapped(UIButton sender)
        {
            RightButton.BackgroundColor = Colors.PopUpSelectedButtonColor;
            RightButtonTapped?.Invoke(this, EventArgs.Empty);
            Close();
        }

    }
}

