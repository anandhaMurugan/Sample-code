using System;
using Helseboka.Core.Common.EnumDefinitions;
using Helseboka.Core.Common.Interfaces;
using Helseboka.iOS.Common.Extension;
using Helseboka.Core.Profile.Interface;
using Helseboka.Core.Profile.Presenter;
using Helseboka.Core.Startup.Interface;
using Helseboka.iOS.Login;
using Helseboka.iOS.Profile.View;
using UIKit;
using Helseboka.Core.Login.Interface;
using Helseboka.Core.Terms.Presenter;

namespace Helseboka.iOS.Profile
{
    public class ProfileRouter : UINavigationController, IProfileRouter
    {
        private UIStoryboard profileStoryboard;
        private IProfilePresenter presenter;
        [Weak] private IStartupRouter presentingRouter;

        public ProfileRouter(IStartupRouter presentingRouter)
        {
            this.presentingRouter = presentingRouter;
            profileStoryboard = UIStoryboard.FromName("Profile", null);
            presenter = new ProfilePresenter(this);
            var myProfileView = profileStoryboard.InstantiateViewController(MyProfileView.Identifier) as MyProfileView;
            myProfileView.Presenter = presenter;

            myProfileView.TabBarItem = new UITabBarItem("Home.TabBar.Profile.Title".Translate(), UIImage.FromBundle("Profil"), UIImage.FromBundle("Profil-active"));

            this.ViewControllers = new UIViewController[] { myProfileView };

            NavigationBar.Hidden = true;
        }

        public void ShowUserInfoView()
        {
            var view = profileStoryboard.InstantiateViewController(UserInfoViewController.Identifier) as UserInfoViewController;
            view.Presenter = presenter;
            PushViewController(view, true);
        }

        public void ShowDoctorAndOfficeDetailsView()
        {
            var view = profileStoryboard.InstantiateViewController(LegeDetailsViewController.Identifier) as LegeDetailsViewController;
            view.Presenter = presenter;
            PushViewController(view, true);
        }

        public void ShowDoctorSelectionView()
        {
            if (presentingRouter != null)
            {
                presentingRouter.NavigateAfterPINSelection(true);
            }
        }
    
        public void NavigateToTerms()
        {
            UIStoryboard TermsStory = UIStoryboard.FromName("Terms", null);
            if (TermsStory.InstantiateViewController("TermsViewController") is TermsViewController termsViewController)
            {
                var presenter = new TermsPresenter(this);
                presenter.IsFromProfile = true;
                termsViewController.Presenter = presenter;
                TabBarController.TabBar.Hidden = true;
                PushViewController(termsViewController, true);
            }
        }

        public void GoBackToHome()
        {
            TabBarController.TabBar.Hidden = false;
            PopToRootViewController(true);
        }

        public void Finish()
        {
        }

        public void Start()
        {
        }

        public void GoBackToLogin()
        {
            PopToRootViewController(true);
            if(presentingRouter != null)
            {
                presentingRouter.StartLoginAgainAfterLogout();
            }
        }

        public void ShowPINConfirmation()
        {
            var loginRouter = new LoginRouter(LoginMode.PIN);
            loginRouter.LoginCompleted += LoginRouter_LoginCompleted;
            PresentViewController(loginRouter, true, null);
        }

        private void LoginRouter_LoginCompleted(object sender, LoginEventArgs e)
        {
            DismissViewController(false, () =>
            {
                if (presentingRouter != null && !e.IsNewPINSet)
                {
                    presentingRouter.NavigateToPINRegistration(true);
                }
            });
        }

        public void NavigateAfterDeleteProfile()
        {
            PopToRootViewController(true);
            if (presentingRouter != null)
            {
                presentingRouter.NavigateAfterDeleteProfile();
            }
        }
    }
}
