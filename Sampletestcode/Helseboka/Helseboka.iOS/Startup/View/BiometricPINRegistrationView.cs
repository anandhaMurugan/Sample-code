using System;
using Foundation;
using Helseboka.Core.Common.CommonImpl;
using Helseboka.Core.Common.Interfaces;
using Helseboka.Core.Startup.Interface;
using Helseboka.iOS.Common.View;
using Helseboka.iOS.Common.Extension;
using LocalAuthentication;
using UIKit;
using Helseboka.iOS.Common.Utilities;
using Helseboka.iOS.Common.PlatformEnums;
using Helseboka.Core.Common.Model;

namespace Helseboka.iOS.Startup.View
{
    public partial class BiometricPINRegistrationView : BaseView
    {
		public ISignupPresenter Presenter
        {
            get => presenter as ISignupPresenter;
            set => presenter = value;
        }

		private IDeviceHandler DeviceHandler
		{
            get => ApplicationCore.Container.Resolve<IDeviceHandler>();
		}

        public BiometricPINRegistrationView() : base() { }

        public BiometricPINRegistrationView(IntPtr handler) : base(handler) { }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            if (Device.DeviceType == DeviceType.iPhones_5_5s_5c_SE)
            {
                ContainerBottomConstraint.Constant = -30;

                TouchIDButtonHeight.Constant = 50;
                PINHeightConstraint.Constant = 50;
                TouchIdButtonWidthConstraint.Constant = 200;
                PINWidthConstraint.Constant = 200;
            }
            CancelButton.Hidden = true;
        }

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			DesignScreen();
		}

		private void DesignScreen()
		{
			if (DeviceHandler.IsFaceIDSupported())
            {
				TouchID.Hidden = false;
                TouchID.SetTitle("Login.FaceID.Button".Translate(), UIControlState.Normal);
				TouchID.SetBackgroundImage(UIImage.FromBundle("FaceID-button-backgrround"), UIControlState.Normal);
				TouchIDButtonHeight.Constant = 70;
                TouchIDBottomConstraint.Constant = 19;
            }
			else if (DeviceHandler.IsTouchIDSupported())
            {
				TouchID.Hidden = false;
                TouchID.SetTitle("Login.TouchId.Button".Translate(), UIControlState.Normal);
				TouchID.SetBackgroundImage(UIImage.FromBundle("TouchID-button-background"), UIControlState.Normal);
				TouchIDButtonHeight.Constant = 70;
                TouchIDBottomConstraint.Constant = 19;
            }
			else
			{
				TouchID.Hidden = true;
				TouchIDButtonHeight.Constant = 0;
				TouchIDBottomConstraint.Constant = 0;
			}
		}

		partial void TouchID_Tapped(BasePrimaryActionButton sender)
		{
			var context = new LAContext();
            NSError AuthError;
            var myReason = new NSString("Login.Biometric.Registration.HelpText".Translate());

			if (context.CanEvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, out AuthError))
            {
                var replyHandler = new LAContextReplyHandler((success, error) => {
                    this.InvokeOnMainThread(() => {
                        if (success)
                        {
							Presenter.RegisterBiometric();
                        }
                    });
                });
                context.EvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, myReason, replyHandler);
            };
		}

		partial void PIN_tapped(BasePrimaryActionButton sender)
		{
			Presenter.ShowPINRegistration();
		}

        partial void Cancel_Tapped(SignUpSecondaryActionButton sender)
        {
            //Presenter.CancelPINBioRegister();
        }
    }
}

