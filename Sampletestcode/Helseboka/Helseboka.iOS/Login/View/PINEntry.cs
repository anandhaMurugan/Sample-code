using System;
using System.Threading.Tasks;
using Helseboka.Core.Login.Interface;
using Helseboka.iOS.Common.View;
using UIKit;
using Helseboka.Core.Common.Extension;
using Helseboka.Core.Common.Model;
using Helseboka.iOS.Common.Utilities;
using Helseboka.iOS.Common.PlatformEnums;
using Helseboka.iOS.Common.Extension;
using Foundation;

namespace Helseboka.iOS.Login.View
{
    public partial class PINEntry : BaseView
    {
		public ILoginPresenter Presenter
        {
			get => presenter as ILoginPresenter;
            set => presenter = value;
        }

        public PINEntry() { }

		public PINEntry(IntPtr ptr) : base(ptr) { }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            ActivityIndicator.Hidden = true;
            ErrorLabel.Hidden = true;

            if (Device.DeviceType == DeviceType.iPhones_5_5s_5c_SE)
            {
                LoginContainerTopConstraint.Constant = 20;
                EnterPINLabelTopConstraint.Constant = 20;
                ErrorLabelTopConstraint.Constant = 10;
                PINViewTopConstraint.Constant = 20;
                ForgotPINTopConstraint.Constant = 20;
                LoginContainerLeadingConstraint.Constant = 20;
                LoginContainerTrailingConstraint.Constant = -20;
            }
        }

        public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			PINView.Completed += PINView_Completed;
            PINView.EditingChanged += PINView_EditingChanged;
			PINView.Focus();
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
        }

		public override void ViewDidDisappear(bool animated)
		{
			base.ViewDidDisappear(animated);

			PINView.Completed -= PINView_Completed;
            PINView.EditingChanged -= PINView_EditingChanged;
		}

		protected override void Presenter_ErrorOccured(object sender, Core.Common.Model.BaseErrorResponseInfo e)
        {
			base.Presenter_ErrorOccured(sender, e);
        }

		partial void ForgotPIN_Tapped(UIButton sender)
		{
            Presenter.DidTapForgotPIN();
		}

        private void PINView_EditingChanged(object sender, string e)
        {
            ErrorLabel.Hidden = true;
        }

		private void PINView_Completed(object sender, string e)
		{
            PINView.UnFocus();
            Login(e).Forget();
		}

        private async Task Login(String password)
        {
            PINView.UserInteractionEnabled = false;
            ActivityIndicator.Hidden = false;
            ActivityIndicator.StartAnimating();

            var response = await Presenter.LoginWithPIN(password);

            ActivityIndicator.StopAnimating();
            ActivityIndicator.Hidden = true;

            if (!response.IsSuccess)
            {
                if(response.ResponseInfo is BaseAPIErrorResponseInfo baseAPIError && baseAPIError.Error == Core.Common.EnumDefinitions.APIError.WrongPIN)
                {
                    ErrorLabel.Hidden = false;
                    ErrorLabel.Text = "Login.Error.WrongPIN".Translate();
                }
                else
                {
                    var result = await ProcessAPIError(response);
                    if(result)
                    {
                        return;
                    }
                }
                PINView.Clear();
                PINView.Focus();
                PINView.UserInteractionEnabled = true;
            }
        }

    }
}

