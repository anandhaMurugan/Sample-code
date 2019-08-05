using System;
using System.Threading.Tasks;
using Helseboka.Core.Auth.Model;
using Helseboka.Core.Common.EnumDefinitions;
using Helseboka.Core.Common.Model;
using Helseboka.Core.Common.Presenter;
using Helseboka.Core.Profile.Interface;
using Helseboka.Core.Profile.Model;

namespace Helseboka.Core.Profile.Presenter
{
    public class ProfilePresenter : BasePresenter, IProfilePresenter
    {
        private IProfileRouter Router { get => router as IProfileRouter; }
        public ProfilePresenter(IProfileRouter profileRouter)
        {
            router = profileRouter;
        }

        public async Task<User> GetCurrentUserProfile()
        {
            Loading();
            var response = await ApplicationCore.Instance.CurrentUser.GetDoctor();
            HideLoading();
            if (response.IsSuccess)
            {
                return ApplicationCore.Instance.CurrentUser;
            }
            else
            {
                RaiseError(response.ResponseInfo);
                return null;
            }
        }

        public void ShowDoctorSelectionView()
        {
            Router.ShowDoctorSelectionView();
        }

        public void ShowDoctorAndOfficeDetailsView()
        {
            Router.ShowDoctorAndOfficeDetailsView();
        }

        public void ShowUserInfoView()
        {
            Router.ShowUserInfoView();
        }

        public void ShowPINConfirmation()
        {
            Router.ShowPINConfirmation();
        }

        public void ShowTermsPage()
        {
            Router.NavigateToTerms();
        }

        public void Logout()
        {
            AuthService.Instance.Logout();
            Router.GoBackToLogin();
        }

        public void GoBackToHome()
        {
            Router.GoBackToHome();
        }

        public async Task DeleteProfile()
        {
            Loading();
            var response = await ApplicationCore.Instance.CurrentUser.DeleteProfile();
            HideLoading();
            if (response.IsSuccess)
            {
                Router.GoBackToLogin();
            }
            else
            {
                RaiseError(response.ResponseInfo);
            }

        }

        public async Task<Response> UpdateMobile(String mobile, String address)
        {
            var request = ApplicationCore.Instance.CurrentUser.Clone();
            request.Phone = mobile;
            request.Address = address;
            Loading();
            var response = await ApplicationCore.Instance.CurrentUser.UpdateUserInfo(request);
            if (response.IsSuccess)
            {
                var userResponse = await ApplicationCore.Instance.GetUserDetails();
                HideLoading();
                if (!response.IsSuccess)
                {
                    RaiseError(userResponse.ResponseInfo);
                }
                return userResponse;
            }
            else
            {
                HideLoading();
                RaiseError(response.ResponseInfo);
                return response;
            }
        }

        public LoginMode? GetLoginMode()
        {
            return AuthService.Instance.ModeOfLogin;
        }
    }
}
