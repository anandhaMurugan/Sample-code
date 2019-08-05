using System;
using Helseboka.Core.Common.EnumDefinitions;
using Helseboka.Core.Dashboard.Presenter;
using Helseboka.Core.Login.Interface;
using Helseboka.Core.Startup.Interface;
using Helseboka.Core.Startup.Presenter;
using Helseboka.iOS.Dashboard.View;
using Helseboka.iOS.Home.View;
using Helseboka.iOS.Legedialog;
using Helseboka.iOS.Common.Extension;
using Helseboka.iOS.Legetimer;
using Helseboka.iOS.Login;
using Helseboka.iOS.Medisiner;
using Helseboka.iOS.Profile;
using Helseboka.iOS.Startup.View;
using UIKit;
using Helseboka.Core.Terms.Presenter;
using Helseboka.Core.Common.Extension;
using Helseboka.iOS.Common.View.PopUpDialogs;
using Helseboka.Core.Startup.UpdateVersion.Presenter;
using Foundation;
using Helseboka.Core.MobilephoneNumber.Presenter;
using Helseboka.Core.Resources.StringResources;
using Helseboka.Core.Common.CommonImpl;

namespace Helseboka.iOS.Startup
{
    public class StartupRouter : UINavigationController, IStartupRouter
    {
		private UIWindow rootWindow;
		private UIStoryboard startupStoryboard;
        private UIStoryboard TermsStory;
        private bool isFromMyProfile = false;
        private bool isUserMovedForwardAfterLogin = false;
        private bool isRegistrationFlow = false;
		public StartupRouter(UIWindow window)
		{
			rootWindow = window;
		}



        public void Start()
        {
            DeviceDetails.Instance.AppVersion = GetAppVersion();
#if DEBUG
            startupStoryboard = UIStoryboard.FromName("Startup", null);
            if (rootWindow != null && startupStoryboard != null)
            {
                var urlconfig = startupStoryboard.InstantiateViewController("UrlConfigController") as UrlConfigController;
                if (urlconfig != null)
                {

                    urlconfig.Presenter = new UrlPresenter(this);
                    this.NavigationBar.Hidden = true;
                    this.ViewControllers = new UIViewController[] { urlconfig };
                    rootWindow.RootViewController = this;
                    rootWindow.MakeKeyAndVisible();
                }
            }
#else
            StartAfterDevSettings();
#endif
        }

        public void DoctorSelectionCompleted()
        {
            if (isFromMyProfile)
            {
                isFromMyProfile = false;
                DismissViewController(true, null);
            }
            else
            {
                NavigateToHome();
            }
        }

        void LoginRouter_LoginCompleted(object sender, LoginEventArgs e)
		{
            isUserMovedForwardAfterLogin = true;
            if (e.Mode == LoginMode.PIN || e.Mode == LoginMode.Biometric)
			{
                //NavigateToHome();
                isRegistrationFlow = false;
                CheckTermsAndMobile();
            }
			else
			{
				var bioView = startupStoryboard.InstantiateViewController("BiometricPINRegistrationView") as BiometricPINRegistrationView;
				if (bioView != null)
                {
					bioView.Presenter = new SignupPresenter(this);
					PushViewController(bioView, true);
					DismissModalViewController(true);
                }
			}
		}

        public void ShowOnboardingView()
        {
            var onboardingView = startupStoryboard.InstantiateViewController(OnboardingView.Identifier) as OnboardingView;
            if (onboardingView != null)
            {
                onboardingView.Presenter = new SplashPresenter(this);
                PresentViewController(onboardingView, true, null);
            }
        }

		public void NavigateToHome()
		{
            isRegistrationFlow = false;
            isUserMovedForwardAfterLogin = true;
            var home = new HomeTabView();
            home.ViewControllers = GetHomeViewControllers();
            home.SelectedIndex = 0;
            PushViewController(home, true);
            DismissModalViewController(true); 
        }

        public void NavigateToTerms()
        {
            TermsStory = UIStoryboard.FromName("Terms", null);
            if (TermsStory.InstantiateViewController("TermsViewController") is TermsViewController termsViewController)
            {
                var presenter = new TermsPresenter(this);
                presenter.IsFromProfile = false;
                termsViewController.Presenter = presenter;
                PushViewController(termsViewController, true);
                DismissModalViewController(true);
            }
        }

        public void NavigateToMobilePhoneNumber()
        {
            var dialog = new MobilePhoneNumbers();
            dialog.Presenter = new MobilePhoneNumberPresenter(this);
            dialog.Show();
        }

		public void StartBankIdLogin()
		{
            DismissViewController(false, null);
			var loginRouter = new LoginRouter(LoginMode.BankID);
			loginRouter.LoginCompleted += LoginRouter_LoginCompleted;
			PresentViewController(loginRouter, true, null);
		}

		public void StartPINLogin()
		{
			var loginRouter = new LoginRouter(LoginMode.PIN);
            loginRouter.LoginCompleted += LoginRouter_LoginCompleted;
            PresentViewController(loginRouter, true, null);
		}

		public void StartBioLogin()
		{
			var loginRouter = new LoginRouter(LoginMode.Biometric);
            loginRouter.LoginCompleted += LoginRouter_LoginCompleted;
            PresentViewController(loginRouter, true, null);
		}

        public void NavigateToPINRegistration(bool isFromMyProfile)
		{
			var confirmPINView = startupStoryboard.InstantiateViewController("PINConfirmation") as PINConfirmation;
            if (confirmPINView != null)
            {
                confirmPINView.Presenter = new SignupPresenter(this);
                if (isFromMyProfile)
                {
                    this.isFromMyProfile = isFromMyProfile;
                    PresentViewController(confirmPINView, false, null);
                }
                else
                {
                    PushViewController(confirmPINView, true);
                }
            }
		}

		private UIViewController[] GetHomeViewControllers()
        {
            var aktueltStoryboard = UIStoryboard.FromName("Dashboard", null);

            var aktueltHomeView = aktueltStoryboard.InstantiateViewController("DashboardHomeView") as DashboardHomeView;
            aktueltHomeView.Presenter = new DashboardPresenter();
            aktueltHomeView.TabBarItem = new UITabBarItem("Home.TabBar.Dashboard.Title".Translate(), UIImage.FromBundle("Dashboard"), UIImage.FromBundle("Dashboard-active"));

            UIViewController[] viewcontrollers = new UIViewController[] {
                aktueltHomeView,
                new LegetimerRouter(),
                new LegedialogRouter(),
                new MedicineRouter(),
                new ProfileRouter(this)
            };

            return viewcontrollers;
        }

        public void StartLoginAgainAfterLogout()
        {
            isUserMovedForwardAfterLogin = false;
            PopToRootViewController(true);
            var splashView = this.ViewControllers[0] as Splash;
            if (splashView != null)
            {
                splashView.Presenter.DecideAndNavigateAppFlow();
            }
        }

        public void InactivityLogout()
        {
            this.InvokeOnMainThread(() =>
            {
                if (isUserMovedForwardAfterLogin)
                {
                    DismissModalViewController(false);
                    DismissViewController(false, null);
                    PopToRootViewController(false);
                    var splashView = this.ViewControllers[0] as Splash;
                    if (splashView != null)
                    {
                        splashView.Presenter.DecideAndNavigateAppFlow();
                    }
                    isUserMovedForwardAfterLogin = false;
                }
            });
        }

        public void CheckTermsAndMobile()
        {
            var presenter = new TermsPresenter(this);
            presenter.CheckUserTermsDetails();
        }

        public void NavigateAfterTermsAndMobileCheck(bool isDoctorPresent)
        {
            if (!isRegistrationFlow && isDoctorPresent)
            {
                NavigateToHome();
            }
            else
            {
                NavigateToDoctorSelection(false);
            }
        }

        public void NavigateToDoctorSelection(bool isFromMyProfile)
        {
            if (startupStoryboard.InstantiateViewController("DoctorSelectionView") is DoctorSelectionView doctorSelectionView)
            {
                doctorSelectionView.Presenter = new DoctorSelectionPresenter(this);
                if (isFromMyProfile)
                {
                    this.isFromMyProfile = isFromMyProfile;
                    PresentViewController(doctorSelectionView, true, null);
                }
                else
                {
                    PushViewController(doctorSelectionView, true);
                }
                DismissModalViewController(true);
            }
        }

        public void NavigateAfterPINSelection(bool isFromMyProfile)
        {
            // Navigation is happening when user is changing PIN from MyProfile
            if (this.isFromMyProfile)
            {
                this.isFromMyProfile = false;
                DismissViewController(true, null);
                return;
            }

            if(isFromMyProfile)
            {
                NavigateToDoctorSelection(isFromMyProfile);
            }
            else
            {
                isRegistrationFlow = true;
                CheckTermsAndMobile();
            }
        }

        public void StartRegistrationFlow()
        {
            StartBankIdLogin();
        }

        public void NavigateAfterDeleteProfile()
        {
            this.InvokeOnMainThread(() =>
            {
                isUserMovedForwardAfterLogin = false;
                DismissModalViewController(false);
                DismissViewController(false, null);
                PopToRootViewController(true);
                var splashView = this.ViewControllers[0] as Splash;
                if (splashView != null)
                {
                    splashView.Presenter.DecideAndNavigateAppFlow();
                }
            });
        }

        public void StartAfterDevSettings()
        {
            var updatePresenter = new UpdatePresenter(this);
            updatePresenter.UpdateVersionCheckDetails().Forget();
        }

        public string GetAppVersion()
        {
            var VersionNumber = NSBundle.MainBundle.InfoDictionary.ValueForKey(new NSString("CFBundleShortVersionString")).ToString();
            return VersionNumber;
        }

        public void StartAfterUpdateSettings(bool shouldUpdate, bool canUpdate, string descriptionText)
        {
            if(shouldUpdate || canUpdate)
            {
                startupStoryboard = UIStoryboard.FromName("Startup", null);           
                var updatePage = startupStoryboard.InstantiateViewController("ForceUpdateViewController") as ForceUpdateViewController;
                if (updatePage != null)
                {
                    updatePage.Presenter = new UpdatePresenter(this);
                    updatePage.shouldUpdate = shouldUpdate;
                    updatePage.descriptionText = descriptionText;
                    this.ViewControllers = new UIViewController[] { updatePage };
                    this.NavigationBar.Hidden = true;
                    rootWindow.RootViewController = this;
                    rootWindow.MakeKeyAndVisible();
                }
            }
            else
            {
                startupStoryboard = UIStoryboard.FromName("Startup", null);
                if (rootWindow != null && startupStoryboard != null)
                {
                    var splash = startupStoryboard.InstantiateViewController("Splash") as Splash;
                    if (splash != null)
                    {
                        splash.Presenter = new SplashPresenter(this);
                        this.NavigationBar.Hidden = true;
                        this.ViewControllers = new UIViewController[] { splash };
                        rootWindow.RootViewController = this;
                        rootWindow.MakeKeyAndVisible();
                    }
                }
            }
        }
    }
}