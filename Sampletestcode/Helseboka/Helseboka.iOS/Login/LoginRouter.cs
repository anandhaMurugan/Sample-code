using System;
using Foundation;
using Helseboka.Core.Common.EnumDefinitions;
using Helseboka.Core.Login.Interface;
using Helseboka.Core.Login.Presenter;
using Helseboka.iOS.Login.View;
using Helseboka.iOS.Startup.View;
using SafariServices;
using UIKit;

namespace Helseboka.iOS.Login
{	    
	public class LoginRouter : UINavigationController, ILoginRouter
    {
		private LoginMode loginMode;
		private UIStoryboard loginStoryboard;

        // One presenter object for all view
        private LoginPresenter presenter;

        public event EventHandler<LoginEventArgs> LoginCompleted;

		public LoginRouter(LoginMode loginMode)
        {
            presenter = new LoginPresenter(this);
            NavigationBar.Hidden = true;
			this.loginMode = loginMode;
			loginStoryboard = UIStoryboard.FromName("Login", null);
            if (loginMode == LoginMode.PIN)
			{
                SetupForPIN();
			}
            else
			{
                SetupForBankId();
			}
        }

        public void NavigateAfterAuthenticationCompleted(Boolean isPINSet)
		{
            LoginCompleted?.Invoke(this, new LoginEventArgs {Mode = loginMode, IsNewPINSet = isPINSet });
		}

        public void SetupForBankId()
		{
			if (loginStoryboard != null)
            {
				var bankId = loginStoryboard.InstantiateViewController("BankIdView") as BankIdView;
				if (bankId != null)
                {
                    bankId.Presenter = presenter;
                    bankId.LoginMode = loginMode;
					this.ViewControllers = new UIViewController[] { bankId };
                }
            }
		}

        public void NavigateToBankID()
        {
            if (loginStoryboard != null)
            {
                var bankId = loginStoryboard.InstantiateViewController("BankIdView") as BankIdView;
                if (bankId != null)
                {
                    bankId.Presenter = presenter;
                    bankId.LoginMode = loginMode;
                    PushViewController(bankId, true);
                }
            }
        }

        public void GoBackToPINFromForgotPINView()
        {
            PopViewController(true);
        }

        private void SetupForPIN()
		{
			if (loginStoryboard != null)
            {
                var pinEntry = loginStoryboard.InstantiateViewController("PINEntry") as PINEntry;
                if (pinEntry != null)
                {
                    pinEntry.Presenter = presenter;
					ViewControllers = new UIViewController[] { pinEntry };
                }
            }
		}

		public void NavigateToBankIdWebView()
		{
            //if (loginStoryboard != null)
            //        {
            //var webview = loginStoryboard.InstantiateViewController("BankIdWebView") as BankIdWebView;
            //if (webview != null)
            //           {
            //               webview.Presenter = presenter;
            //NavigationBar.Hidden = false;
            //PushViewController(webview, true);
            //    }
            //}

            (UIApplication.SharedApplication.Delegate as AppDelegate).LoginPresenter = presenter;

            var url = new NSUrl(presenter.GetAuthURL());
            var safari = new SFSafariViewController(url, true);
            PresentViewController(safari, true, null);
		}

        public void ShowPinChange()
        {               
            var startupStoryboard = UIStoryboard.FromName("Startup", null);
            if (startupStoryboard.InstantiateViewController("PINConfirmation") is PINConfirmation confirmPINView)
            {
                confirmPINView.Presenter = presenter;
                PushViewController(confirmPINView, false);
            }
            DismissViewController(true, null);
        }

		public void GoBackToBankIdOption()
		{
            DismissViewController(true, null);
			//PopToRootViewController(true);
		}
	}
}
