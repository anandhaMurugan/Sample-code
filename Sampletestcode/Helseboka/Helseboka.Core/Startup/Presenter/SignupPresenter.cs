using System;
using Helseboka.Core.Auth.Model;
using Helseboka.Core.Common.Presenter;
using Helseboka.Core.Startup.Interface;

namespace Helseboka.Core.Startup.Presenter
{
    public class SignupPresenter : BasePresenter, ISignupPresenter, IPINConfirmation
    {
		private IStartupRouter Router { get => router as IStartupRouter; }

		public SignupPresenter(IStartupRouter startupRouter)
        {
			router = startupRouter;
        }

		public void RegisterBiometric()
		{
			AuthService.Instance.RegisterBio();
            Router.NavigateAfterPINSelection(false);
		}

		public void ShowPINRegistration()
		{
            Router.NavigateToPINRegistration(false);
		}

        public void PINSelectionCompleted(string pin)
        {
            AuthService.Instance.RegisterPIN(pin);
            Router.NavigateAfterPINSelection(false);
        }

        public void CancelPINBioRegister()
        {
            Router.NavigateAfterPINSelection(false);
        }
    }
}
