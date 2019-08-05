using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Helseboka.Core.Auth.Model;
using Helseboka.Core.Common.Constant;
using Helseboka.Core.Common.Extension;
using Helseboka.Core.Common.Interfaces;
using Helseboka.Core.Common.Model;
using Helseboka.Core.Common.Presenter;
using Helseboka.Core.Login.Interface;
using Helseboka.Core.Startup.Interface;

namespace Helseboka.Core.Login.Presenter
{
    public class LoginPresenter : BasePresenter, ILoginPresenter, IPINConfirmation
    {
        private bool isForgotPINFlow = false;
        private IAnalytics EventTracker => ApplicationCore.Container.Resolve<IAnalytics>();

        private BankIDType bankIDType;
        public ILoginRouter Router 
		{
			get => router as ILoginRouter;
		}

        public bool CanCancelBankID 
        {
            get => isForgotPINFlow;
        }

		public LoginPresenter(ILoginRouter loginRouter)
        {
			router = loginRouter;
        }

        public async Task<Response> LoginWithPIN(string pin)
		{
            var response = await AuthService.Instance.LoginWithPIN(pin);
            if (response.IsSuccess)
            {
                Router.NavigateAfterAuthenticationCompleted(false);
            }

            return response;
		}

        public void GoBackToBankIdOption()
        {
            Router.GoBackToBankIdOption();
        }

        public async Task<Response> LoginWithTouchId()
		{
            var response = await AuthService.Instance.LoginWithBIO();
            if (response.IsSuccess)
            {
                Router.NavigateAfterAuthenticationCompleted(false);
            }

            return response;
		}

		public void StartBankId()
		{
            bankIDType = BankIDType.BID;
            StartBankIdAuthentication();
		}

		public void StartBankIdMobile()
		{
			bankIDType = BankIDType.BIM;
			StartBankIdAuthentication();
		}

		private async void StartBankIdAuthentication()
		{
			Loading();
			var response = await AuthService.Instance.StartAuth(bankIDType);
			HideLoading();
			if (response.IsSuccess)
			{
				Router.NavigateToBankIdWebView();
			}
            else
            {
                RaiseError(response.ResponseInfo);
            }
		}

		public String GetAuthURL()
		{
			return AuthService.Instance.AuthUrl;
		}

		public Dictionary<String, String> GetSecurityHeaders()
		{
			return AuthService.Instance.GetSecurityHeaders();
		}

		public bool CheckAuthResponse(String redirectionUrl)
		{
			if (redirectionUrl.StartsWith(AppConstant.AppScheme, StringComparison.OrdinalIgnoreCase))
            {
				if (redirectionUrl.StartsWith(AppConstant.OkUrl, StringComparison.OrdinalIgnoreCase))
				{
					CompleteAuthentication().Forget();
				}
				else
				{
                    EventTracker.TrackEvent("Error in bankID", new Dictionary<string, string> { { "url",redirectionUrl } });
					Router.GoBackToBankIdOption();
				}
                return false;
            }
            else
            {
                return true;
            }
		}

        public bool IsBankIDRedirect(String redirectionUrl)
        {
            if (!String.IsNullOrEmpty(redirectionUrl))
            {
                return redirectionUrl.StartsWith(AppConstant.BankIDRedirectionUrl, StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }

        public void PINSelectionCompleted(string pin)
        {
            AuthService.Instance.RegisterPIN(pin);
            Router.NavigateAfterAuthenticationCompleted(true);
            isForgotPINFlow = false;
        }

        public void DidTapForgotPIN()
        {
            isForgotPINFlow = true;
            Router.NavigateToBankID();
        }

        public void CancelBankID()
        {
            isForgotPINFlow = false;
            Router.GoBackToPINFromForgotPINView();
        }

		private async Task CompleteAuthentication()
		{
			Loading();
			var response = await AuthService.Instance.CompleteAuth();
			HideLoading();

            if (response.IsSuccess)
            {
                if (isForgotPINFlow)
                {
                    Router.ShowPinChange();
                }
                else
                {
                    Router.NavigateAfterAuthenticationCompleted(false);
                }
            }
            else
            {
                RaiseError(response.ResponseInfo);
                Router.GoBackToBankIdOption();
            }
		}


	}
}
