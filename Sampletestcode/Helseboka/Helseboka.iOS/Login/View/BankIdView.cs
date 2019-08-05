using System;
using System.Threading.Tasks;
using Foundation;
using Helseboka.Core.Common.EnumDefinitions;
using Helseboka.Core.Login.Interface;
using Helseboka.iOS.Common.View;
using LocalAuthentication;
using UIKit;
using Helseboka.Core.Common.Extension;
using Helseboka.iOS.Common.Extension;
using Helseboka.iOS.Common.Utilities;
using Helseboka.iOS.Common.PlatformEnums;

namespace Helseboka.iOS.Login.View
{
    public partial class BankIdView : BaseView
    {
        public LoginMode LoginMode { get; set; }

		public ILoginPresenter Presenter
        {
			get => presenter as ILoginPresenter;
            set => presenter = value;
        }

        public BankIdView() { }

		public BankIdView (IntPtr handler) : base(handler) { }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            if (Device.DeviceType == DeviceType.iPhones_5_5s_5c_SE)
            {
                ContainerBottomConstraint.Constant = -30;
            }

            CancelButton.Hidden = !Presenter.CanCancelBankID;
        }

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			NavigationController.NavigationBar.Hidden = true;
		}

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            if (LoginMode == LoginMode.Biometric)
            {
                var context = new LAContext();
                var myReason = new NSString("Login.Biometric.HelpText".Translate());

                if (context.CanEvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, out var AuthError))
                {
                    var replyHandler = new LAContextReplyHandler((success, error) => {
                        this.InvokeOnMainThread(() => {
                            if (success)
                            {
                                LoginWithBio().Forget();
                            }
                        });
                    });
                    context.EvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, myReason, replyHandler);
                };
            }
        }

        partial void Cancel_Tapped(UIButton sender)
        {
            Presenter.CancelBankID();
        }

        private async Task LoginWithBio()
        {
            ShowLoader();
            var response = await Presenter.LoginWithTouchId();
            HideLoader();

            if (!response.IsSuccess)
            {
                await ProcessAPIError(response);
            }
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

		partial void BankID_Tapped(PrimaryActionButton sender)
		{
            LoginMode = LoginMode.BankID;
			Presenter.StartBankId();
		}

		partial void BankIDMobile_Tapped(PrimaryActionButton sender)
		{
            LoginMode = LoginMode.BankID;
            Presenter.StartBankIdMobile();
		}
    }
}

