using System;
using Helseboka.Core.Common.Interfaces;

namespace Helseboka.Core.Profile.Interface
{
    public interface IProfileRouter : IBaseRouter
    {
        void ShowUserInfoView();
        void ShowDoctorSelectionView();
        void ShowDoctorAndOfficeDetailsView();
        void ShowPINConfirmation();
        void GoBackToHome();
        void GoBackToLogin();
        void NavigateAfterDeleteProfile();
        void NavigateToTerms();
    }
}
