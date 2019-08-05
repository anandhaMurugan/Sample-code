using Helseboka.Core.Common.Model;
using Helseboka.Core.Common.Presenter;
using Helseboka.Core.Terms.Interface;
using Helseboka.Core.Terms.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helseboka.Core.Common.Extension;
using Helseboka.Core.Startup.Interface;
using Xamarin.Forms;
using Helseboka.Core.Profile.Model;
using Helseboka.Core.Profile.Interface;

namespace Helseboka.Core.Terms.Presenter
{
    public class TermsPresenter : BasePresenter, ITermsPresenter
    {
        private TermsListModel TermsDatas = new TermsListModel();
        private IStartupRouter Router { get => router as IStartupRouter; }
        private IProfileRouter ProfileRouter { get => router as IProfileRouter; }
        private User userDetails;
        public bool IsFromProfile { get; set; }

        public TermsPresenter(IStartupRouter router)
        {
            this.router = router;
        }

        public TermsPresenter(IProfileRouter router)
        {
            this.router = router;
        }

        public async Task<TermsListModel> RefreshTermsDatas()
        {
            var response = await TermsDatas.GetAllTerms();
            if (response.IsSuccess)
            {
                return response.Result;
            }
            return null;
        }

        public async Task<Response> UpdateTerms(List<int> acceptedIds)
        {
            var response = await TermsDatas.UpdateAllTerms(acceptedIds);
            if (response.IsSuccess)
            {
                HideLoading();
                if (IsFromProfile)
                {
                    ProfileRouter.GoBackToHome();
                }
                else
                {
                    userDetails = ApplicationCore.Instance.CurrentUser;
                    if (string.IsNullOrEmpty(userDetails.Phone))
                    {
                        Router.NavigateToMobilePhoneNumber();
                    }
                    else
                    {
                        CheckForDoctorAndProceed();
                    }
                }
                return response;
            }
            else
            {
                HideLoading();
                RaiseError(response.ResponseInfo);
                return null;
            }
        }

        public void CheckUserTermsDetails()
        {
            userDetails = ApplicationCore.Instance.CurrentUser;

            if (!userDetails.TermsValid)
            {
                Router.NavigateToTerms();
            }
            else if (string.IsNullOrEmpty(userDetails.Phone))
            {
                Router.NavigateToMobilePhoneNumber();
            }
            else
            {
                CheckForDoctorAndProceed();
            }
        }

        private void CheckForDoctorAndProceed()
        {
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