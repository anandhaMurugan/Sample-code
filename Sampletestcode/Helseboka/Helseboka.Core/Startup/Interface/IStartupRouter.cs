using System;
using System.Threading.Tasks;
using Helseboka.Core.Common.Interfaces;

namespace Helseboka.Core.Startup.Interface
{
	public interface IStartupRouter : IBaseRouter
    {
        void Start();

        void StartAfterDevSettings();

        void StartRegistrationFlow();

		void StartBankIdLogin();

		void StartPINLogin();

		void StartBioLogin();

		void NavigateToHome();

        void NavigateToPINRegistration(bool isFromMyProfile);

        void NavigateAfterPINSelection(bool isFromMyProfile);

        void StartLoginAgainAfterLogout();

        void NavigateAfterDeleteProfile();

        void ShowOnboardingView();

        void DoctorSelectionCompleted();

        void InactivityLogout();

        void NavigateToTerms();

        void NavigateToMobilePhoneNumber();

        string GetAppVersion();

        void StartAfterUpdateSettings(bool shouldUpdate, bool canUpdate, string descriptionText = null);

        void NavigateAfterTermsAndMobileCheck(bool isDoctorPresent);

    }
}
