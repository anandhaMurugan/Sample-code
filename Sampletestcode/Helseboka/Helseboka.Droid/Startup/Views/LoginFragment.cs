
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Helseboka.Core.Common.Model;
using Helseboka.Core.Common.Extension;
using Helseboka.Core.Login.Interface;
using Helseboka.Droid.Common.Views;
using Android.Preferences;

namespace Helseboka.Droid.Startup.Views
{
    public class LoginFragment : BaseFragment
    {
        EditText hiddenTextField;
        LinearLayout pinView;
        TextView pageTitle;
        TextView errorTextView;
        ProgressBar activityProgress;
        Button forgotPIN;
        String pin;

        public ILoginPresenter Presenter
        {
            get => presenter as ILoginPresenter;
        }

        public LoginFragment(ILoginPresenter presenter)
        {
            this.presenter = presenter;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_pin_confirmation, null);

            hiddenTextField = view.FindViewById<EditText>(Resource.Id.hiddenPasswordField);
            pinView = view.FindViewById<LinearLayout>(Resource.Id.pinview);
            pageTitle = view.FindViewById<TextView>(Resource.Id.pageTitle);
            errorTextView = view.FindViewById<TextView>(Resource.Id.errorLabel);
            activityProgress = view.FindViewById<ProgressBar>(Resource.Id.loading_progressbar);
            forgotPIN = view.FindViewById<Button>(Resource.Id.forgotPIN);

            pageTitle.Text = Resources.GetString(Resource.String.login_pin_header);
            forgotPIN.Visibility = ViewStates.Visible;

            pinView.Click += PinView_Click;
            hiddenTextField.TextChanged += HiddenTextField_TextChanged;
            forgotPIN.Click += ForgotPIN_Click;

            Activity.Window.SetSoftInputMode(SoftInput.AdjustNothing);

            ShowKeyboard(hiddenTextField);

            return view;
        }

        public override void OnDestroyView()
        {
            base.OnDestroyView();

            Activity.Window.SetSoftInputMode(SoftInput.AdjustResize);
        }

        void ForgotPIN_Click(object sender, EventArgs e)
        {
            HideKeyboard(hiddenTextField);
            forgotPIN.Enabled = false;
            Presenter.DidTapForgotPIN();
        }


        void PinView_Click(object sender, EventArgs e)
        {
            ShowKeyboard(hiddenTextField);
        }

        void HiddenTextField_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            if(hiddenTextField.Text.Length > 0)
            {
                errorTextView.Visibility = ViewStates.Invisible;
            }

            for (int index = 0; index < 4; index++)
            {
                var view = pinView.FindViewWithTag(index.ToString());
                if (index < hiddenTextField.Text.Length)
                {
                    view.SetBackgroundResource(Resource.Drawable.shape_pin_fill);
                }
                else
                {
                    view.SetBackgroundResource(Resource.Drawable.shape_pin_empty);
                }
            }

            if (hiddenTextField.Text.Length == 4)
            {
                Login().Forget();
            }
        }

        private async Task Login()
        {
            await Task.Delay(TimeSpan.FromMilliseconds(200));
            HideKeyboard(hiddenTextField);
            pinView.Enabled = false;
            activityProgress.Visibility = ViewStates.Visible;
            //activityProgress.Animate();

            var response = await Presenter.LoginWithPIN(hiddenTextField.Text);

            activityProgress.Visibility = ViewStates.Gone;

            if (!response.IsSuccess)
            {
                if (response.ResponseInfo is BaseAPIErrorResponseInfo baseAPIError && baseAPIError.Error == Core.Common.EnumDefinitions.APIError.WrongPIN)
                {
                    errorTextView.Visibility = ViewStates.Visible;
                    errorTextView.Text = Resources.GetString(Resource.String.login_error_wrongpin);
                    hiddenTextField.Text = String.Empty;
                    pin = String.Empty;
                    ShowKeyboard(hiddenTextField);
                }
                else
                {
                    hiddenTextField.Text = String.Empty;
                    pin = String.Empty;
                    ProcessAPIError(response, OnErrorProcessingCompleted);
                }
                pinView.Enabled = true;
            }
        }

        void OnErrorProcessingCompleted()
        {
            ShowKeyboard(hiddenTextField);
        }

    }
}
