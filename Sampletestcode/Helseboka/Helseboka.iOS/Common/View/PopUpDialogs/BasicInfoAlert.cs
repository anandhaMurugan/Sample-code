using System;
using Foundation;
using Helseboka.Core.Resources.StringResources;
using UIKit;

namespace Helseboka.iOS.Common.View.PopUpDialogs
{
    public partial class BasicInfoAlert : BaseModalViewController
    {
        private String title;
        private String message;
        private String buttonText;
        private Action okTapped;

        public NSAttributedString AttributedMessage { get; set; }

        public BasicInfoAlert(String message, String title = "", String buttonText = "", Action onOkTapped = null) : base("BasicInfoAlert")
        {
            this.message = message;
            this.okTapped = onOkTapped;

            if (String.IsNullOrEmpty(title))
            {
                this.title = AppResources.BasicInfoTitle;
            }

            if (!String.IsNullOrEmpty(buttonText))
            {
                this.buttonText = buttonText;
            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            MessageLabel.Text = message;

            if(AttributedMessage != null)
            {
                MessageLabel.AttributedText = AttributedMessage;
            }

            if (!String.IsNullOrEmpty(title))
            {
                TitleLabel.Text = title;
            }

            if (!String.IsNullOrEmpty(buttonText))
            {
                OkButtonLabel.SetTitle(buttonText, UIControlState.Normal);
            }

            var image = UIImage.FromBundle("Modal-background");
            ContentView.Layer.Contents = image.CGImage;
        }

        partial void Ok_Tapped(MediumActionButton sender)
        {
            Close();
            okTapped?.Invoke();
        }
    }
}

