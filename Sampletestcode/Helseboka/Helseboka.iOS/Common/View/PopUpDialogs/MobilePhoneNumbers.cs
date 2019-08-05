using Helseboka.Core.MobilephoneNumber.Interface;
using Helseboka.Core.Profile.Model;
using Helseboka.Core.Resources.StringResources;
using Helseboka.iOS.Common.Constant;
using Helseboka.iOS.Common.View;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UIKit;
using Helseboka.Core.Common.Extension;
using Helseboka.iOS.Common.Extension;

namespace Helseboka.iOS
{
    public partial class MobilePhoneNumbers : BaseModalViewController
    {
        private User currentUser;

        public IMobilePhoneNumberPresenter Presenter
        {
            get => presenter as IMobilePhoneNumberPresenter;
            set => presenter = value;
        }

        public MobilePhoneNumbers() : base("MobilePhoneNumbers"){ }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            DescriptionText.Text = AppResources.MobilePhoneNumberPageText;
            ErrorText.Text = AppResources.MobilePhoneNumberPageErrorMessage;
            ErrorMessageText.Text = AppResources.MobilePhoneNumberPageErrorMessageText;
            EditTextBox.Placeholder = AppResources.MobilePhoneNumberPageErrorMessage;
            PopupTitle.Text = AppResources.PhoneNumberPopupTitle;
            OkButton.SetTitle(AppResources.OkButtonText, UIControlState.Normal);

            ErrorText.Hidden = true;
            ErrorMessageText.Hidden = true;
            EditTextBox.KeyboardType = UIKeyboardType.PhonePad;
            EditTextBox.BecomeFirstResponder();

            OkButton.TouchUpInside += OkButton_TouchUpInside;
            EditTextBox.EditingChanged += EditText_EditingChanged;

            this.View.BackgroundColor = UIColor.Clear;
            this.View.BackgroundColor = UIColor.FromWhiteAlpha(1.0f, 0.5f);
            BoxView.Layer.BorderColor = Colors.UnselectedLabelBorderColor.CGColor;
            BoxView.Layer.BackgroundColor = Colors.SearchResultBackground.CGColor;
            BoxView.Layer.BorderWidth = 1;
            BoxView.Layer.CornerRadius = BoxView.Frame.Height / 2;

            DialogView.AddBorder(Colors.PopUpShadowColor, 13);
            DialogView.AddShadow(Colors.PopUpShadowColor, 13, 0, 0);
        }

        private void EditText_EditingChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(EditTextBox.Text))
            {
                ErrorText.Hidden = true;
            }
            else
            {
                Regex regex = new Regex(@"^[0-9+ ]+$");
                if (!regex.IsMatch(EditTextBox.Text))
                {
                    ErrorMessageText.Hidden = false;
                }
                else
                {
                    ErrorMessageText.Hidden = true;
                    ErrorText.Hidden = true;
                }
            }
        }

        private void OkButton_TouchUpInside(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(EditTextBox.Text))
            {
                ErrorText.Hidden = false;
            }
            else
            {
                if (ErrorMessageText.Hidden && ErrorText.Hidden)
                {
                    Save().Forget();
                }
            }
        }

        private async Task Save()
        {
            var response = await Presenter.UpdateMobilePhoneNumber(EditTextBox.Text);
            if (response.IsSuccess)
            {
                await LoadData();
                Close();
                Presenter.UpdateSuccess();
            }
        }

        private async Task LoadData()
        {
            currentUser = await Presenter.GetCurrentUserProfile();
            if (currentUser != null)
            {
                EditTextBox.Text = currentUser.Phone;
            }
        }
    }
}