using Foundation;
using Helseboka.Core.Common.Model;
using Helseboka.iOS.Common.CommonImpl;
using Helseboka.iOS.Common.View;
using Helseboka.Core.Common.Extension;
using Helseboka.iOS.Startup;
using UIKit;
using Helseboka.iOS.Notification;
using Helseboka.Core.Login.Interface;
using Helseboka.iOS.Common.Utilities;
using Helseboka.Core.Startup.Interface;
using Helseboka.Core.Common.Interfaces;
using Xamarin.Forms;

namespace Helseboka.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {
        // class-level declarations
        protected LoadingOverlay overlay;
        private UIVisualEffectView blurView;

        public IStartupRouter InitialRouter { get; private set; } 

        public double lastActivityTime;

        public override UIWindow Window
        {
            get;
            set;
        }

        public ILoginPresenter LoginPresenter { get; set; }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            // Override point for customization after application launch.
            // If not required for your application you can safely delete this method

            Forms.Init();

            ApplicationCore.Container.RegisterType<ISecureDataStorage, SecureStorageHandler>();
            ApplicationCore.Container.RegisterType<IDeviceHandler, DeviceHandler>();
            ApplicationCore.Container.RegisterInstance<INotificationService>(Notification.NotificationService.Instance);
            ApplicationCore.Container.RegisterSingletonType<IAnalytics, AnalyticsHandler>();

            ApplicationCore.Instance.Initialize();
            ApplicationCore.Container.Resolve<IAnalytics>().Initialize();

			Window = new UIWindow(UIScreen.MainScreen.Bounds);

            InitialRouter = new StartupRouter(Window);
            InitialRouter.Start();

            NotificationService.Instance.Register();

            InActivityHandler.Instance.Reset();

            return true;
        }

        [Export("application:didRegisterForRemoteNotificationsWithDeviceToken:")]
        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            NotificationService.Instance.UpdateNotificationToken(deviceToken).Forget();
        }

        [Export("application:openURL:options:")]
        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            if (LoginPresenter!= null)
            {
                LoginPresenter.CheckAuthResponse(url.AbsoluteString);
                LoginPresenter = null;
            }

            return true;
        }

        public override void OnResignActivation(UIApplication application)
        {
            // Invoked when the application is about to move from active to inactive state.
            // This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
            // or when the user quits the application and it begins the transition to the background state.
            // Games should use this method to pause the game.
        }

        public override void DidEnterBackground(UIApplication application)
        {
            // Use this method to release shared resources, save user data, invalidate timers and store the application state.
            // If your application supports background exection this method is called instead of WillTerminate when the user quits.

            // Blur screen
            using (var blurEffect = UIBlurEffect.FromStyle(UIBlurEffectStyle.Dark))
            {
                blurView = new UIVisualEffectView(blurEffect);
                blurView.Frame = Window.RootViewController.View.Bounds;
                blurView.AddGestureRecognizer(new UITapGestureRecognizer((UITapGestureRecognizer obj) =>
                {
                    obj.View.RemoveFromSuperview();
                    blurView = null;
                }));
                Window.AddSubview(blurView);
            }
        }


        public override void WillEnterForeground(UIApplication application)
        {
            // Called as part of the transiton from background to active state.
            // Here you can undo many of the changes made on entering the background.
        }

        public override void OnActivated(UIApplication application)
        {
            // Restart any tasks that were paused (or not yet started) while the application was inactive. 
            // If the application was previously in the background, optionally refresh the user interface.

            if (blurView != null)
            {
                blurView.RemoveFromSuperview();
                blurView.Dispose();
                blurView = null;
            }
            InActivityHandler.Instance.ApplicationResumes();
        }

        public override void WillTerminate(UIApplication application)
        {
            // Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
        }

        public virtual void ShowLoader()
        {
            HideLoader();
            var bounds = UIScreen.MainScreen.Bounds;
            overlay = new LoadingOverlay(bounds);
            UIApplication.SharedApplication.KeyWindow.AddSubview(overlay);
        }

        public virtual void HideLoader()
        {
            if (overlay != null)
            {
                overlay.Hide();
                overlay.RemoveFromSuperview();
                overlay = null;
            }
        }

    }
}

