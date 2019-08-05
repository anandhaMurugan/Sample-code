using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Helseboka.Core.Common.Interfaces;
using Helseboka.Core.Common.Model;
using Helseboka.Core.Common.Extension;
using UIKit;
using Helseboka.iOS.Common.Extension;
using Helseboka.Core.Common.EnumDefinitions;
using Helseboka.iOS.Common.View.PopUpDialogs;
using Helseboka.Core.Resources.StringResources;
using Helseboka.Core.Auth.Model;

namespace Helseboka.iOS.Common.View
{
	public class BaseView : UIViewController, IBaseView   
    {		
		protected IBasePresenter presenter;
        private IAnalytics EventTracker => ApplicationCore.Container.Resolve<IAnalytics>();

        public BaseView() : base()
        {
        }

        public BaseView(String nibName) : base(nibName, null)
        {
        }

		public BaseView(IntPtr ptr) : base(ptr)
		{
			
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

            RegisterEvents();

            var screenName = this.GetType().Name;
            EventTracker.TrackEvent(screenName);
		}

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            if(NavigationController != null)
            {
               NavigationController.InteractivePopGestureRecognizer.Enabled = false;
            }
        }

        private void RegisterEvents()
        {
            if (presenter != null)
            {
                DeRegisterEvents();

                presenter.ErrorOccured += Presenter_ErrorOccured;
                presenter.LoadingStarted += Presenter_LoadingStarted;
                presenter.LoadingCompleted += Presenter_LoadingCompleted;
            }
        }

        private void DeRegisterEvents()
        {
            if (presenter != null)
            {
                presenter.ClearEventInvocationList();
                presenter.ErrorOccured -= Presenter_ErrorOccured;
                presenter.LoadingStarted -= Presenter_LoadingStarted;
                presenter.LoadingCompleted -= Presenter_LoadingCompleted;
            }
        }

        protected virtual void Presenter_LoadingStarted(object sender, EventArgs e)
		{
			ShowLoader();
		}

		protected virtual void Presenter_LoadingCompleted(object sender, EventArgs e)
		{
			HideLoader();
		}

		protected virtual void Presenter_ErrorOccured(object sender, Core.Common.Model.BaseErrorResponseInfo e)
		{
            ProcessAPIError(e).Forget();
		}

        public async Task<bool> ProcessAPIError(Response response)
        {
            if (response.ResponseInfo is BaseErrorResponseInfo error)
            {
                return await ProcessAPIError(error);
            }

            return false;
        }

        public async Task<bool> ProcessAPIError(BaseErrorResponseInfo error)
        {
            String message = "General.Error.API.Generic".Translate();
            if (error is BaseAPIErrorResponseInfo apiErrorResponse)
            {
                if (apiErrorResponse.Error == APIError.TimeOut)
                {
                    message = "General.Error.API.Timeout".Translate();
                }
                else if(apiErrorResponse.Error == APIError.UnAuthorized)
                {
                    AuthService.Instance.DeleteAllUserAuthData();
                    var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
                    appDelegate.InitialRouter.NavigateAfterDeleteProfile();
                    return true;
                }
            }
            else if(error is BaseClientErrorResponseInfo clientError && !String.IsNullOrEmpty(clientError.Message))
            {
                message = clientError.Message;
            }

            var title = AppResources.ErrorInfoTitle;
            var dialog = new BasicInfoAlert(message, title);
            await dialog.ShowAsync();

            return false;
        }

		protected virtual void ShowLoader()
		{
            (UIApplication.SharedApplication.Delegate as AppDelegate).ShowLoader();
		}

		protected virtual void HideLoader()
		{
            (UIApplication.SharedApplication.Delegate as AppDelegate).HideLoader();
		}

        public void EmbedView(UIView container, UIViewController containerViewController, UIViewController viewController)
        {
            containerViewController.AddChildViewController(viewController);
            container.AddSubview(viewController.View);

            viewController.View.AllEdgesToView(container);
            viewController.DidMoveToParentViewController(containerViewController);
        }

        public void ShowStatusBarActivityIndicator()
        {
            UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
        }

        public void HideStatusBarActivityIndicator()
        {
            UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
        }

        protected void DismissKeyboardOnBackgroundTap()
        {
            // Add gesture recognizer to hide keyboard
            var tap = new UITapGestureRecognizer { CancelsTouchesInView = false };
            tap.AddTarget(() => View.EndEditing(true));
            View.AddGestureRecognizer(tap);
        }

        public void ShowInfoDialog(String message, String title = "", Action onOkTapped = null)
        {
            var dialog = new BasicInfoAlert(message, title, onOkTapped: onOkTapped);
            dialog.Show();
        }

        public void CheckDoctorAndProceed(Action forwardAction)
        {
            if (ApplicationCore.Instance.CurrentUser != null && ApplicationCore.Instance.CurrentUser.AssignedDoctor != null && !ApplicationCore.Instance.CurrentUser.AssignedDoctor.Enabled)
            {
                ShowInfoDialog(ApplicationCore.Instance.CurrentUser.AssignedDoctor.Remarks);
            }
            else
            {
                forwardAction?.Invoke();
            }
        }
    }
}
