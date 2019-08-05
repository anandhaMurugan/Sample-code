using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.CustomTabs;
using Android.Support.Design.Widget;
using Android.Util;
using Android.Views;
using Helseboka.Core.Common.Constant;
using Helseboka.Core.Common.EnumDefinitions;
using Helseboka.Core.Common.Interfaces;
using Helseboka.Core.Common.Model;
using Helseboka.Core.Dashboard.Presenter;
using Helseboka.Core.Login.Interface;
using Helseboka.Core.Login.Presenter;
using Helseboka.Core.Profile.Interface;
using Helseboka.Core.Profile.Presenter;
using Helseboka.Core.Startup.Interface;
using Helseboka.Core.Startup.Presenter;
using Helseboka.Core.Terms.Model;
using Helseboka.Core.Terms.Presenter;
using Helseboka.Droid.AppointmentModule;
using Helseboka.Droid.Chat;
using Helseboka.Droid.Common.CommonImpl;
using Helseboka.Droid.Common.EnumDefinitions;
using Helseboka.Droid.Common.Interfaces;
using Helseboka.Droid.Common.Utils;
using Helseboka.Droid.Common.Views;
using Helseboka.Droid.Dashboard.Views;
using Helseboka.Droid.MedicineModule;
using Helseboka.Droid.MobilephoneNumber.Views;
using Helseboka.Droid.ProfileModule.Views;
using Helseboka.Droid.Startup.Views;
using Helseboka.Core.Common.Extension;
using Helseboka.Core.MobilephoneNumber.Interface;
using Helseboka.Core.MobilephoneNumber.Presenter;
using Helseboka.Core.Startup.UpdateVersion.Presenter;
using System.Threading.Tasks;
using Helseboka.Core.Common.CommonImpl;

namespace Helseboka.Droid.Startup
{
    [Activity(MainLauncher = true, WindowSoftInputMode = SoftInput.AdjustResize,LaunchMode = LaunchMode.SingleInstance, ScreenOrientation = ScreenOrientation.Portrait)]
    [IntentFilter(new[] { Intent.ActionView },
        Categories = new[] {
            Intent.ActionView,
            Intent.CategoryDefault,
            Intent.CategoryBrowsable
        },
        DataScheme = "helseboka")
    ]
    public class MainActivity : BaseActivity, IStartupRouter, ILoginRouter, IProfileRouter, IActivity, ViewTreeObserver.IOnGlobalLayoutListener
    {
        private IAnalytics EventTracker => ApplicationCore.Container.Resolve<IAnalytics>();

        private SplashPresenter splashPresenter;
        private UrlPresenter urlPresenter;
        private UpdatePresenter updatePresenter;
        private TermsPresenter termsPresenter;
        private MobilePhoneNumberPresenter mobilePhoneNumberPresenter;
        private LoginPresenter loginPresenter;
        private SignupPresenter signupPresenter;
        private ProfilePresenter profilePresenter;
        private BottomNavigationView bottomNavigation;
        private ChatRouter chatRouter;
        private MedicineRouter medicineRouter;
        private AppointmentRouter appointmentRouter;
        private bool keyboardListenersAttached = false;
        public ViewGroup rootLayout;
        private bool isNavigationFromMyProfile = false;
        private bool isRegistrationFlow = false;
        private DashboardFragment _dashboard;
        private InactivitySessionHandler inactivityHandler;
        private bool isUserMovedAfterLogin = false;
        private bool isFromSignUp = false;

        public override View RootView 
        {
            get
            {
                return rootLayout;
            }
        }

        public event EventHandler<LoginEventArgs> LoginCompleted;
        public event EventHandler KeyboardHide;
        public event EventHandler<int> KeyboardVisible;

        public MainActivity()
        {
            inactivityHandler = new InactivitySessionHandler();
            inactivityHandler.InactivityLogout += InactivityHandler_InactivityLogout;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Window.AddFlags(WindowManagerFlags.Secure);
            Xamarin.Forms.Forms.Init(this, savedInstanceState);
            var data = Intent.Data;
            if(data != null)
            {
                EventTracker.TrackEvent("BankID callback", new Dictionary<string, string> { { "url", data.ToString() } });
            }
            if (data != null && AppConstant.AppScheme.StartsWith(data.Scheme, StringComparison.CurrentCultureIgnoreCase) && currentFragment is BankIdOptionsFragment && loginPresenter != null)
            {
                loginPresenter.CheckAuthResponse(data.ToString());
            }
            else
            {
                Xamarin.Essentials.Platform.Init(this, savedInstanceState);
                Initialize();
                SetContentView(Resource.Layout.activity_main);
                rootLayout = FindViewById(Resource.Id.rootView) as ViewGroup;
                bottomNavigation = FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation);
                bottomNavigation.Visibility = ViewStates.Gone;
                bottomNavigation.SetShiftMode(false, false);
                bottomNavigation.NavigationItemSelected += BottomNavigation_NavigationItemSelected;
                bottomNavigation.SelectedItemId = Resource.Id.menu_dashboard;
                Start();
                Common.CommonImpl.NotificationHandler.Instance.Register();
            }
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);

            var data = intent.Data;

            if (data != null)
            {
                EventTracker.TrackEvent("BankID callback", new Dictionary<string, string> { { "url", data.ToString() } });
            }
            if (data != null && AppConstant.AppScheme.StartsWith(data.Scheme, StringComparison.CurrentCultureIgnoreCase) && currentFragment is BankIdOptionsFragment && loginPresenter != null)
            {
                loginPresenter.CheckAuthResponse(data.ToString());
            }
        }

        public override Android.Views.View OnCreateView(string name, Context context, IAttributeSet attrs)
        {
            return base.OnCreateView(name, context, attrs);
        }

        protected override void OnResume()
        {
            base.OnResume();
            inactivityHandler.ReportApplicationResume();
            if(isUserMovedAfterLogin)
            {
                inactivityHandler.ExtendSession();
            }
        }

        protected override void OnPause()
        {
            base.OnPause();
            inactivityHandler.ReportApplicationPause();
        }

        public override void OnUserInteraction()
        {
            base.OnUserInteraction();
            if(isUserMovedAfterLogin)
            {
                inactivityHandler.ExtendSession();
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public void AttachKeyboardListner()
        {
            if (keyboardListenersAttached)
            {
                return;
            }

            rootLayout.ViewTreeObserver.AddOnGlobalLayoutListener(this);
            keyboardListenersAttached = true;
        }

        public void RemoveKeyboardListner()
        {
            rootLayout.ViewTreeObserver.RemoveOnGlobalLayoutListener(this);
            keyboardListenersAttached = false;
        }

        public void OnGlobalLayout()
        {
            // navigation bar height
            int navigationBarHeight = 0;
            int resourceId = Resources.GetIdentifier("navigation_bar_height", "dimen", "android");
            if (resourceId > 0)
            {
                navigationBarHeight = Resources.GetDimensionPixelSize(resourceId);
            }

            // status bar height
            int statusBarHeight = 0;
            resourceId = Resources.GetIdentifier("status_bar_height", "dimen", "android");
            if (resourceId > 0)
            {
                statusBarHeight = Resources.GetDimensionPixelSize(resourceId);
            }

            // display window size for the app layout
            Rect rect = new Rect();
            Window.DecorView.GetWindowVisibleDisplayFrame(rect);

            // screen height - (user app height + status + nav) ..... if non-zero, then there is a soft keyboard
            int keyboardHeight = rootLayout.RootView.Height - (statusBarHeight + navigationBarHeight + rect.Height());

            if (keyboardHeight <= 0)
            {
                KeyboardHide?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                KeyboardVisible?.Invoke(this, keyboardHeight);
            }
        }

        private void BottomNavigation_NavigationItemSelected(object sender, BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
            if (bottomNavigation.Visibility == ViewStates.Visible)
            {
                SetMenuIcon(e.Item);
                if (e.Item.ItemId == Resource.Id.menu_dashboard)
                {
                    NavigateTo(GetDashboardFragment());
                }
                else if (e.Item.ItemId == Resource.Id.menu_appointment)
                {
                    if (appointmentRouter == null)
                    {
                        appointmentRouter = new AppointmentRouter(this);
                    }
                    NavigateTo(appointmentRouter.GetCurrentFragment());
                }
                else if (e.Item.ItemId == Resource.Id.menu_chat)
                {
                    if (chatRouter == null)
                    {
                        chatRouter = new ChatRouter(this);
                    }
                    NavigateTo(chatRouter.GetCurrentFragment());
                }
                else if (e.Item.ItemId == Resource.Id.menu_medicine)
                {
                    if (medicineRouter == null)
                    {
                        medicineRouter = new MedicineRouter(this);
                    }
                    NavigateTo(medicineRouter.GetCurrentFragment());
                }
                else if (e.Item.ItemId == Resource.Id.menu_profile)
                {
                    var fragment = new ProfileHomeFragment(profilePresenter);
                    NavigateTo(fragment);
                }
            }
        }

        private void SetMenuIcon(IMenuItem selectedMenuItem)
        {
            for (int index = 0; index < bottomNavigation.Menu.Size(); index++)
            {
                var menuItem = bottomNavigation.Menu.GetItem(index);

                if (menuItem.ItemId == selectedMenuItem.ItemId)
                {
                    menuItem.SetIcon(GetActiveMenuIcon(menuItem.ItemId));
                }
                else
                {
                    menuItem.SetIcon(GetInActiveMenuIcon(menuItem.ItemId));
                }
            }
        }

        private int GetInActiveMenuIcon(int id)
        {
            switch (id)
            {
                case Resource.Id.menu_dashboard: return Resource.Drawable.dashboard_inactive;
                case Resource.Id.menu_appointment: return Resource.Drawable.appointment_inactive;
                case Resource.Id.menu_medicine: return Resource.Drawable.medicine_inactive;
                case Resource.Id.menu_chat: return Resource.Drawable.chat_inactive;
                default: return Resource.Drawable.profile_inactive;
            }
        }

        private int GetActiveMenuIcon(int id)
        {
            switch (id)
            {
                case Resource.Id.menu_dashboard: return Resource.Drawable.dashboard_active;
                case Resource.Id.menu_appointment: return Resource.Drawable.appointment_active;
                case Resource.Id.menu_medicine: return Resource.Drawable.medicine_active;
                case Resource.Id.menu_chat: return Resource.Drawable.chat_active;
                default: return Resource.Drawable.profile_active;
            }
        }


        private void Initialize()
        {
            Common.CommonImpl.NotificationHandler.Instance.SetContext(this);
            ApplicationCore.Container.RegisterType<ISecureDataStorage, SecureStorageHandler>(this);
            ApplicationCore.Container.RegisterType<IDeviceHandler, DeviceHandler>(this);
            ApplicationCore.Container.RegisterInstance<INotificationService>(Common.CommonImpl.NotificationHandler.Instance);
            ApplicationCore.Container.RegisterSingletonType<IAnalytics, AnalyticsHandler>();
            ApplicationCore.Instance.Initialize();

            Common.CommonImpl.NotificationHandler.Instance.Register();
            ApplicationCore.Container.Resolve<IAnalytics>().Initialize();

            splashPresenter = new SplashPresenter(this);
            urlPresenter = new UrlPresenter(this);
            termsPresenter = new TermsPresenter(this as IStartupRouter);
            loginPresenter = new LoginPresenter(this);
            signupPresenter = new SignupPresenter(this);
            profilePresenter = new ProfilePresenter(this);
            mobilePhoneNumberPresenter = new MobilePhoneNumberPresenter(this);
            updatePresenter = new UpdatePresenter(this);
        }

        private DashboardFragment GetDashboardFragment()
        {
            if (_dashboard == null)
            {
                _dashboard = new DashboardFragment(new DashboardPresenter());
            }

            return _dashboard;
        }

        void InactivityHandler_InactivityLogout(object sender, EventArgs e)
        {
            InactivityLogout();
        }


        public void NavigateAfterPINSelection(bool isFromMyProfile)
        {
            if(isNavigationFromMyProfile)
            {
                isNavigationFromMyProfile = false;
                (this as IProfileRouter).GoBackToHome();
            }
            else
            {
                //check terms and mobile and go to doctor selection view
                CheckTermsAndMobile();
            }
        }

        public void NavigateToHome()
        {
            inactivityHandler.ExtendSession();
            isUserMovedAfterLogin = true;
            isRegistrationFlow = false;
            bottomNavigation.Visibility = ViewStates.Visible;
            bottomNavigation.SelectedItemId = Resource.Id.menu_dashboard;
            NavigateTo(GetDashboardFragment());
        }

        void IStartupRouter.NavigateToTerms()
        {          
            termsPresenter.IsFromProfile = false;
            NavigateTo(new TermsFragment(termsPresenter));          
        }

        void IProfileRouter.NavigateToTerms()
        {
            var termsPresenterForProfile = new TermsPresenter(this as IProfileRouter);
            termsPresenterForProfile.IsFromProfile = true;
            HideToolbar();
            NavigateTo(new TermsFragment(termsPresenterForProfile),TransitionEffect.Push);
        }

        public void NavigateToPINRegistration(bool isFromMyProfile)
        {
            NavigateTo(new ConfirmationPinFragment(signupPresenter));
        }

        public void ShowOnboardingView()
        {
            NavigateTo(new OnboardingFragment(splashPresenter));
        }

        public void Start()
        {
            DeviceDetails.Instance.AppVersion = GetAppVersion();
#if DEBUG
            NavigateTo(new UrlFragment(urlPresenter));
#else
            StartAfterDevSettings();
#endif     
        }

        public void StartAfterDevSettings()
        {
            CheckForUpdates().Forget();
        }

        public async Task CheckForUpdates()
        {
            await updatePresenter.UpdateVersionCheckDetails();
        }

        public void StartBankIdLogin()
        {
            isRegistrationFlow = false;
            NavigateTo(new BankIdOptionsFragment(loginPresenter));
        }

        public void StartBioLogin()
        {
            isUserMovedAfterLogin = false;
            isRegistrationFlow = false;
            NavigateTo(new BankIdOptionsFragment(loginPresenter, LoginMode.Biometric));
        }
        public void StartPINLogin()
        {
            isUserMovedAfterLogin = false;
            isRegistrationFlow = false;
            NavigateTo(new LoginFragment(loginPresenter));
        }

        public void StartLoginAgainAfterLogout()
        {
            isUserMovedAfterLogin = false;
            isRegistrationFlow = false;
            bottomNavigation.Visibility = ViewStates.Gone;
            NavigateTo(new SplashFragment(splashPresenter));
        }

        public void NavigateToBankIdWebView()
        {
            EventTracker.TrackEvent("BankID started");

            // In App web view
            //NavigateTo(new BankIdWebViewFragment(loginPresenter));

            // external browser
            var url = loginPresenter.GetAuthURL();
            Android.Net.Uri authUri = Android.Net.Uri.Parse(url);
            var browserIntent = new Intent(Intent.ActionView, authUri);
            StartActivity(browserIntent);

            // chrome custom tab
            //var url = loginPresenter.GetAuthURL();
            //Android.Net.Uri authUri = Android.Net.Uri.Parse(url);
            //try
            //{
            //    var customTabs = new CustomTabsActivityManager(this);
            //    customTabs.CustomTabsServiceConnected += (name, client) =>
            //    {
            //        var builder = new CustomTabsIntent.Builder(customTabs.Session)
            //            .SetToolbarColor(Color.Argb(255, 82, 0, 160))
            //            .SetShowTitle(true)
            //            .SetStartAnimations(this, Resource.Animation.slide_in_from_right, Resource.Animation.slide_out_to_left)
            //            .SetExitAnimations(this, Resource.Animation.slide_in_from_left, Resource.Animation.slide_out_to_right);
            //        var customTabsIntent = builder.Build();
            //        CustomTabsHelper.AddKeepAliveExtra(this, customTabsIntent.Intent);
            //        customTabsIntent.Intent.AddFlags(ActivityFlags.NoHistory);
            //        customTabsIntent.Intent.AddFlags(ActivityFlags.ClearTop);
            //        customTabsIntent.Intent.AddFlags(ActivityFlags.NewTask);
            //        customTabsIntent.LaunchUrl(this, authUri);
            //        EventTracker.TrackEvent(HelsebokaEvent.CustomTabSupported);
            //    };
            //    customTabs.BindService();

            //}
            //catch (Exception ex)
            //{
            //    EventTracker.TrackEvent(HelsebokaEvent.CustomTabNotSupported);
            //    EventTracker.TrackError(ex);
            //    var browserIntent = new Intent(Intent.ActionView, authUri);
            //    StartActivity(browserIntent);
            //}
        }


        public void GoBackToBankIdOption()
        {
            NavigateTo(new BankIdOptionsFragment(loginPresenter));
        }

        public void ShowPinChange()
        {
            NavigateTo(new ConfirmationPinFragment(loginPresenter), TransitionEffect.Push);
        }

        public void NavigateToBankID()
        {
            NavigateTo(new BankIdOptionsFragment(loginPresenter), TransitionEffect.Push);
        }

        public void StartRegistrationFlow()
        {
            isRegistrationFlow = true;
            NavigateTo(new BankIdOptionsFragment(loginPresenter));
        }

        public void DoctorSelectionCompleted()
        {
            if(isNavigationFromMyProfile)
            {
                isNavigationFromMyProfile = false;
                var fragment = new DoctorDetailsFragment(profilePresenter);
                ShowToolbar();
                NavigateTo(fragment, TransitionEffect.Pop);
            }
            else
            {
                NavigateToHome();
            }
        }

        public void NavigateAfterAuthenticationCompleted(bool isPINSet)
        {
            inactivityHandler.ExtendSession();
            isUserMovedAfterLogin = true;
            if(isNavigationFromMyProfile)
            {
                if(isPINSet)
                {
                    isNavigationFromMyProfile = false;
                    (this as IProfileRouter).GoBackToHome();
                }
                else
                {
                    ShowPinChange();
                }
            }
            else
            {
                if (isRegistrationFlow)
                {
                    NavigateTo(new RegistrationOptionsFragment(signupPresenter));

                }
                else
                {
                    // NavigateToHome();
                    CheckTermsAndMobile();
                }
            }
        }

        void IProfileRouter.ShowUserInfoView()
        {
            var fragment = new PersonalSettingsFragment(profilePresenter);
            NavigateTo(fragment, TransitionEffect.Push);
        }

        public void NavigateToMobilePhoneNumber()
        {
            var alertDialog = new MobilePhoneNumberFragment(mobilePhoneNumberPresenter, this);
            alertDialog.Show();
        }

        void IProfileRouter.ShowDoctorSelectionView()
        {
            isNavigationFromMyProfile = true;
            HideToolbar();
            NavigateTo(new DoctorSelectionFragment(new DoctorSelectionPresenter(this)), TransitionEffect.Push);
        }

        void IProfileRouter.ShowDoctorAndOfficeDetailsView()
        {
            var fragment = new DoctorDetailsFragment(profilePresenter);
            NavigateTo(fragment, TransitionEffect.Push);
        }

        void IProfileRouter.ShowPINConfirmation()
        {
            isNavigationFromMyProfile = true;
            HideToolbar();
            NavigateTo(new LoginFragment(loginPresenter), TransitionEffect.Push);
        }

        void IProfileRouter.GoBackToHome()
        {
            ShowToolbar();
            var fragment = new ProfileHomeFragment(profilePresenter);
            NavigateTo(fragment, TransitionEffect.Pop);
        }

        void IProfileRouter.GoBackToLogin()
        {
            StartLoginAgainAfterLogout();
        }

        public void NavigateTo(BaseFragment fragment, TransitionEffect effect = TransitionEffect.None)
        {
            NavigateTo(Resource.Id.container, fragment, effect);
        }

        public void HideToolbar()
        {
            bottomNavigation.Visibility = ViewStates.Gone;
        }

        public void ShowToolbar()
        {
            bottomNavigation.Visibility = ViewStates.Visible;
        }

        public void InactivityLogout()
        {
            if(isUserMovedAfterLogin)
            {
                RunOnUiThread(() =>
                {
                    HideToolbar();
                    StartLoginAgainAfterLogout();
                });
            }
        }

        public void GoBackToPINFromForgotPINView()
        {
            NavigateTo(new LoginFragment(loginPresenter), TransitionEffect.Pop);
        }

        public void NavigateAfterDeleteProfile()
        {
            isUserMovedAfterLogin = false;
            isRegistrationFlow = false;
            bottomNavigation.Visibility = ViewStates.Gone;
            NavigateTo(new SplashFragment(splashPresenter));
        }

        public string GetAppVersion()
        {
            string VersionNumber = this.PackageManager.GetPackageInfo(this.PackageName, PackageInfoFlags.MetaData).VersionName;
            return VersionNumber;
        }

        public void StartAfterUpdateSettings(bool shouldUpdate, bool canUpdate, string descriptionText)
        {
            if(shouldUpdate || canUpdate)
            {
                NavigateTo(new ForceUpdateFragment(updatePresenter, shouldUpdate, descriptionText), TransitionEffect.Push);
            }
            else
            {
                NavigateTo(new SplashFragment(splashPresenter), TransitionEffect.Push);
            }
        }

        public void NavigateAfterTermsAndMobileCheck(bool isDoctorPresent)
        {
            if (!isRegistrationFlow && isDoctorPresent)
            {
                NavigateToHome();
            }
            else
            {
                NavigateToDoctorSelection();
            }
        }

        public void NavigateToDoctorSelection()
        {
            NavigateTo(new DoctorSelectionFragment(new DoctorSelectionPresenter(this)));
        }

        public void CheckTermsAndMobile()
        {
             termsPresenter.CheckUserTermsDetails();
        }
    }
}
