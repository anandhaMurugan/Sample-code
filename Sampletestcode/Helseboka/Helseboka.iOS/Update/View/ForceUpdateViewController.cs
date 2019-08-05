using System;
using Foundation;
using Helseboka.Core.Common.Constant;
using Helseboka.Core.Startup.UpdateVersion.Interface;
using Helseboka.iOS.Constants;
using Helseboka.iOS;
using Helseboka.iOS.Common.Extension;
using Helseboka.iOS.Common.View;
using SafariServices;
using UIKit;

namespace Helseboka.iOS
{
    public partial class ForceUpdateViewController : BaseView
    {
        public IUpdatePresenter Presenter
        {
            get => presenter as IUpdatePresenter;
            set => presenter = value;
        }

        public ForceUpdateViewController(IntPtr handle) : base(handle) { }

        public bool shouldUpdate;
        public string descriptionText;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            HideLoader();
            UpdateButton.TouchUpInside += UpdateButton_TouchUpInside;
            TitleLabel.Font = Fonts.Bold(24);
            DescriptionLabel.Font = Fonts.Medium(16);
            DescriptionLabel.Text = descriptionText;
            UpdateButton.SetTitle("Update.UpdateButton.Text".Translate(), UIControlState.Normal);
            TitleLabel.Text = "Update.Title.Text".Translate();

            if (shouldUpdate)
            {
                CloseOrNotNowButton.Hidden = true;
            }
            else
            {
                CloseOrNotNowButton.SetTitle("Update.NotNowButton.Text".Translate(), UIControlState.Normal);
            }

            CloseOrNotNowButton.TouchUpInside += CloseOrNotNowButton_TouchUpInside;
        }

        private void CloseOrNotNowButton_TouchUpInside(object sender, EventArgs e)
        {
            if (!shouldUpdate)
            {
                Presenter.ProceedCloseOrNotNowClicked();
            }
        }

        private void UpdateButton_TouchUpInside(object sender, EventArgs e)
        {
            var url = new NSUrl(APIEndPoints.GetUpdateIosUrl);
            var safari = new SFSafariViewController(url, true);
            PresentViewController(safari, true, null);
        }

    }
}