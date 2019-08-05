using System;
using Helseboka.Core.Common.EnumDefinitions;
using Helseboka.Core.Common.Interfaces;

namespace Helseboka.Core.Login.Interface
{
    public class LoginEventArgs : EventArgs
    {
        public LoginMode Mode { get; set; }
        public bool IsNewPINSet { get; set; }
    }

	public interface ILoginRouter : IBaseRouter
    {
        event EventHandler<LoginEventArgs> LoginCompleted;
        
		void NavigateToBankIdWebView();

		void GoBackToBankIdOption();

        void ShowPinChange();

        void NavigateToBankID();

        void NavigateAfterAuthenticationCompleted(Boolean isPINSet);

        void GoBackToPINFromForgotPINView();
    }
}
