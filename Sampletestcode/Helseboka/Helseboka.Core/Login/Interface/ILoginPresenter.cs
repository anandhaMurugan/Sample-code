using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Helseboka.Core.Common.Interfaces;
using Helseboka.Core.Common.Model;

namespace Helseboka.Core.Login.Interface
{
	public interface ILoginPresenter : IBasePresenter
    {
        bool CanCancelBankID { get; }

        Task<Response> LoginWithPIN(String pin);

        Task<Response> LoginWithTouchId();

		void StartBankId();

		void StartBankIdMobile();

		String GetAuthURL();

		Dictionary<String, String> GetSecurityHeaders();

		bool CheckAuthResponse(String redirectionUrl);

        void DidTapForgotPIN();

        void GoBackToBankIdOption();

        bool IsBankIDRedirect(String redirectionUrl);

        void CancelBankID();
    }
}
