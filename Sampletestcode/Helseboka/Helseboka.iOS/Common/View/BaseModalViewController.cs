using System;
using System.Threading.Tasks;
using UIKit;

namespace Helseboka.iOS.Common.View
{
    public partial class BaseModalViewController : BaseView
    {
        private UIViewController RootViewController;
        private TaskCompletionSource<Object> taskCompletionSource;
        public event EventHandler Closed;

        protected BaseModalViewController() : base()
        {
        }

        protected BaseModalViewController(String nibName) : base(nibName)
        {
        }

        public BaseModalViewController(IntPtr handler) : base(handler){ }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }

        public void Show()
        {
            RootViewController = TopViewController();
            RootViewController.AddChildViewController(this);

            View.TranslatesAutoresizingMaskIntoConstraints = false;
            View.Alpha = 0;
            RootViewController.View.AddSubview(View);

            DidMoveToParentViewController(RootViewController);

            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
            {
                View.LeadingAnchor.ConstraintEqualTo(RootViewController.View.SafeAreaLayoutGuide.LeadingAnchor).Active = true;
                View.TopAnchor.ConstraintEqualTo(RootViewController.View.SafeAreaLayoutGuide.TopAnchor).Active = true;
                View.TrailingAnchor.ConstraintEqualTo(RootViewController.View.SafeAreaLayoutGuide.TrailingAnchor).Active = true;
                View.BottomAnchor.ConstraintEqualTo(RootViewController.View.SafeAreaLayoutGuide.BottomAnchor).Active = true;
            }
            else
            {
                View.LeadingAnchor.ConstraintEqualTo(RootViewController.View.LeadingAnchor).Active = true;
                View.TopAnchor.ConstraintEqualTo(RootViewController.View.TopAnchor).Active = true;
                View.TrailingAnchor.ConstraintEqualTo(RootViewController.View.TrailingAnchor).Active = true;
                View.BottomAnchor.ConstraintEqualTo(RootViewController.View.BottomAnchor).Active = true;
            }

            UIView.Animate(0.2, 0, UIViewAnimationOptions.CurveEaseInOut, () => {
                View.Alpha = 1;
            }, null);
        }

        public Task ShowAsync()
        {
            taskCompletionSource = new TaskCompletionSource<object>();
            Show();
            return taskCompletionSource.Task;
        }

        public void Close()
        {
            WillMoveToParentViewController(null);

            UIView.Animate(0.2, 0, UIViewAnimationOptions.CurveEaseInOut, () => {
                View.Alpha = 0;
            }, null);

            View.RemoveFromSuperview();
            RemoveFromParentViewController();

            if (taskCompletionSource != null)
            {
                taskCompletionSource.TrySetResult(new Object());
            }

            Closed?.Invoke(this, EventArgs.Empty);
        }



        public static UIViewController TopViewController()
        {
            var rootViewController = UIApplication.SharedApplication.KeyWindow.RootViewController;
            return TopViewController(rootViewController);
        }

        private static UIViewController TopViewController(UIViewController rootViewContoller)
        {

            if (rootViewContoller.PresentedViewController == null)
                return rootViewContoller;

            var navigationController = rootViewContoller.PresentedViewController as UINavigationController;
            if (navigationController != null)
            {
                return navigationController;
            }

            var presentedViewController = rootViewContoller.PresentedViewController;
            return TopViewController(presentedViewController);
        }
    }
}

