using Helseboka.Core.Common.Model;
using Helseboka.Core.Common.Presenter;
using Helseboka.Core.MobilephoneNumber.Interface;
using Helseboka.Core.Profile.Model;
using Helseboka.Core.Startup.Interface;
using System.Threading.Tasks;

namespace Helseboka.Core.MobilephoneNumber.Presenter
{
    public class MobilePhoneNumberPresenter : BasePresenter, IMobilePhoneNumberPresenter
    {
        private IStartupRouter Router { get => router as IStartupRouter; }

        public MobilePhoneNumberPresenter(IStartupRouter router)
        {
            this.router = router;
        }

        public async Task<User> GetCurrentUserProfile()
        {
            return ApplicationCore.Instance.CurrentUser;
        }

        public async Task<Response> UpdateMobilePhoneNumber(string mobile)
        {
            var request = ApplicationCore.Instance.CurrentUser.Clone();
            request.Phone = mobile;
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

        public void UpdateSuccess()
        {
            var userDetails = ApplicationCore.Instance.CurrentUser;
            if (!userDetails.HasDoctor())
            {
                Router.NavigateAfterTermsAndMobileCheck(false);
            }
            else
            {
                Router.NavigateAfterTermsAndMobileCheck(true);
            }
        }
    }
}