
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Helseboka.Core.Common.Interfaces;
using Helseboka.Core.Common.Model;
using Helseboka.Core.Common.Extension;
using Android.Views.InputMethods;
using Helseboka.Core.Common.EnumDefinitions;
using Helseboka.Core.HelpAndFAQ.Model;
using Android.Animation;
using Android.Views.Animations;
using Helseboka.Core.Resources.StringResources;

namespace Helseboka.Droid.Common.Views
{
    public class BaseFragment : Android.Support.V4.App.Fragment
    {
        CustomProgressIndicator dialog;

        protected IBasePresenter presenter;
        private IAnalytics EventTracker => ApplicationCore.Container.Resolve<IAnalytics>();

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            if (presenter != null)
            {
                RegisterBaseEvents();
            }

            var screenName = this.GetType().Name;
            EventTracker.TrackEvent(screenName);
        }

        public virtual bool OnBackKeyPressed()
        {
            return false;
        }

        private void RegisterBaseEvents()
        {
            if (presenter != null)
            {
                DeRegisterBaseEvents();

                presenter.ErrorOccured += Presenter_ErrorOccured;
                presenter.LoadingStarted += Presenter_LoadingStarted;
                presenter.LoadingCompleted += Presenter_LoadingCompleted;
            }
        }

        private void DeRegisterBaseEvents()
        {
            if (presenter != null)
            {
                presenter.ErrorOccured -= Presenter_ErrorOccured;
                presenter.LoadingStarted -= Presenter_LoadingStarted;
                presenter.LoadingCompleted -= Presenter_LoadingCompleted;

                presenter.ClearEventInvocationList();
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
            ProcessAPIError(e);
        }

        public void ProcessAPIError(Response response, Action onCompleted = null)
        {
            if (response.ResponseInfo is BaseErrorResponseInfo error)
            {
                ProcessAPIError(error, onCompleted);
            }
        }

        public void ProcessAPIError(BaseErrorResponseInfo error, Action onCompleted = null)
        {
            String message = Resources.GetString(Resource.String.general_error_api_generic); //"General.Error.API.Generic".Translate();
            if (error is BaseAPIErrorResponseInfo apiErrorResponse)
            {
                if (apiErrorResponse.Error == APIError.TimeOut)
                {
                    message = Resources.GetString(Resource.String.general_error_api_timeout);
                }
            }
            else if (error is BaseClientErrorResponseInfo clientError && !String.IsNullOrEmpty(clientError.Message))
            {
                message = clientError.Message;
            }
            var title = AppResources.ErrorInfoTitle;
            Activity.RunOnUiThread(() => ShowInfoDialog(message, title, onCompleted));
        }

        protected virtual void ShowLoader()
        {
            dialog = new CustomProgressIndicator(this.Activity);
            dialog.Show();

        }

        protected virtual void HideLoader()
        {
            if (dialog != null)
            {
                dialog.Dismiss();
            }
        }

        public void ShowInfoDialog(String message, String title = "", Action onCompleted = null)
        {
            var alertDialog = new BasicAlertDialog(Activity, message, title, onCompleted: onCompleted);
            alertDialog.Show();
        }

        public void CheckDoctorAndProceed(Action forwardAction)
        {
            if (ApplicationCore.Instance.CurrentUser != null && ApplicationCore.Instance.CurrentUser.AssignedDoctor != null && !ApplicationCore.Instance.CurrentUser.AssignedDoctor.Enabled)
            {
                var title = AppResources.BasicInfoTitle;
                ShowInfoDialog(ApplicationCore.Instance.CurrentUser.AssignedDoctor.Remarks, title, null);
            }
            else
            {
                forwardAction?.Invoke();
            }
        }

        public void ShowKeyboard(EditText editText)
        {
            if(editText != null)
            {
                editText.RequestFocus();
                editText.PostDelayed(() =>
                {
                    var keyboard = Activity.GetSystemService(Context.InputMethodService) as InputMethodManager;
                    if (keyboard != null)
                    {
                        keyboard.ShowSoftInput(editText, 0);
                    }
                }, 50);
            }
        }

        public void HideKeyboard(EditText editText)
        {
            if (editText != null)
            {
                editText.PostDelayed(() =>
                {
                    if (Activity != null)
                    {
                        var keyboard = Activity.GetSystemService(Context.InputMethodService) as InputMethodManager;
                        if (keyboard != null)
                        {
                            keyboard.HideSoftInputFromWindow(editText.WindowToken, 0);
                        }
                    }
                }, 50);
            }
        }

        public void HideKeyboard()
        {
            if(Activity != null && Activity.CurrentFocus != null)
            {
                var keyboard = Activity.GetSystemService(Context.InputMethodService) as InputMethodManager;
                if (keyboard != null)
                {
                    keyboard.HideSoftInputFromWindow(Activity.CurrentFocus.WindowToken, 0);
                }
            }
        }

        public async Task DesignHelpView(LinearLayout helpView, HelpFAQType helpType)
        {
            var dataList = await new HelpFAQManager().GetHelpFAQList(helpType);
            if (dataList != null)
            {
                foreach (var item in dataList)
                {
                    var view = Activity.LayoutInflater.Inflate(Resource.Layout.template_home_help_view, null);
                    var titleView = view.FindViewById<TextView>(Resource.Id.helpTitle);
                    var descriptionView = view.FindViewById<TextView>(Resource.Id.helpDescription);
                    var imageView = view.FindViewById<ImageView>(Resource.Id.downArrow);

                    titleView.Text = item.Title;
                    descriptionView.Text = item.Description;

                    titleView.Click += (sender, e) =>
                    {
                        if (descriptionView.Visibility.Equals(ViewStates.Gone))
                        {
                            view.Selected = true;
                            descriptionView.Visibility = ViewStates.Visible;

                            var actualHeight = descriptionView.Paint.GetFontMetrics().Bottom = descriptionView.Paint.GetFontMetrics().Top;

                            ValueAnimator mAnimator = GetSlideAnimator(0, (int)Math.Ceiling(actualHeight), descriptionView);
                            mAnimator.Start();

                            imageView.Animate().Rotation(180f);

                        }
                        else
                        {
                            view.Selected = false;
                            int finalHeight = descriptionView.Height;

                            ValueAnimator mAnimator = GetSlideAnimator(finalHeight, 0, descriptionView);
                            mAnimator.Start();
                            mAnimator.AnimationEnd += (object IntentSender, EventArgs arg) => {
                                descriptionView.Visibility = ViewStates.Gone;
                            };

                            imageView.Animate().Rotation(0.0f);
                        }
                    };

                    helpView.AddView(view);
                }
            }
        }

        public ValueAnimator GetSlideAnimator(int start, int end, View view)
        {
            ValueAnimator animator = ValueAnimator.OfInt(start, end);
            animator.Update +=
                (object sender, ValueAnimator.AnimatorUpdateEventArgs e) =>
                {
                    var value = (int)animator.AnimatedValue;
                    ViewGroup.LayoutParams layoutParams = view.LayoutParameters;
                    layoutParams.Height = value;
                    view.LayoutParameters = layoutParams;

                };
            return animator;
        }

        public async Task ShowHelpView(HelpFAQType helpFAQType)
        {
            ShowLoader();
            var helpView = new ModalHelpView(Activity, helpFAQType);
            await helpView.Show();
            HideLoader();
        }

        public void NavigateToBrowser(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                Android.Net.Uri uri = Android.Net.Uri.Parse(url);
                var browserIntent = new Intent(Intent.ActionView, uri);
                StartActivity(browserIntent);
            }     
        }

    }
}
