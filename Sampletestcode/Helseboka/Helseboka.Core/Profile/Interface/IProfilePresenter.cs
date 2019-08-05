using System;
using System.Threading.Tasks;
using Helseboka.Core.Common.EnumDefinitions;
using Helseboka.Core.Common.Interfaces;
using Helseboka.Core.Common.Model;
using Helseboka.Core.Profile.Model;

namespace Helseboka.Core.Profile.Interface
{
    public interface IProfilePresenter : IBasePresenter
    {
        Task<User> GetCurrentUserProfile();

        void Logout();

        void ShowUserInfoView();

        void ShowDoctorAndOfficeDetailsView();

        void ShowDoctorSelectionView();

        void ShowPINConfirmation();

        void ShowTermsPage();

        void GoBackToHome();

        Task<Response> UpdateMobile(String mobile, String address);

        LoginMode? GetLoginMode();

        Task DeleteProfile();
    }
}
