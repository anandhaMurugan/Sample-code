
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
using Helseboka.Core.Common.EnumDefinitions;
using Helseboka.Core.Common.Model;
using Helseboka.Core.Common.Extension;
using Helseboka.Core.Login.Interface;
using Helseboka.Core.Startup.Interface;
using Helseboka.Droid.Common.Utils;
using Helseboka.Droid.Common.Views;
using Android.Support.Design.Widget;

namespace Helseboka.Droid.Startup.Views
{
    public class BankIdOptionsFragment : BaseFragment
    {
        Button bankIDButton;
        Button bankIDMobileButton;
        FingerprintHandler fingerprintHandler;
        Snackbar snackbar;
        TextView helpTextView;

        LoginMode LoginMode { get; set; }

        private ILoginPresenter Presenter
        {
            get => presenter as ILoginPresenter;
        }

        public BankIdOptionsFragment(ILoginPresenter splashPresenter, LoginMode loginMode = LoginMode.BankID)
        {
            this.presenter = splashPresenter;
            this.LoginMode = loginMode;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_bankidOption, null);

            bankIDButton = view.FindViewById<Button>(Resource.Id.buttonBankId);
            bankIDMobileButton = view.FindViewById<Button>(Resource.Id.buttonBankIdMobile);
            helpTextView = view.FindViewById<TextView>(Resource.Id.helpTextView);

            bankIDButton.Click -= BankIDButton_Click;
            bankIDButton.Click += BankIDButton_Click;

            bankIDMobileButton.Click -= BankIDMobileButton_Click;
            bankIDMobileButton.Click += BankIDMobileButton_Click;

            if(Presenter.CanCancelBankID)
            {
                helpTextView.Visibility = ViewStates.Visible;
                helpTextView.Text = Resources.GetString(Resource.String.general_view_cancel);
                helpTextView.Click -= Cancel_Clink;
                helpTextView.Click += Cancel_Clink;
            }
            else
            {
                helpTextView.Visibility = ViewStates.Invisible;
            }

            return view;
        }

        public override void OnResume()
        {
            base.OnResume();
            bankIDButton.Enabled = true;
            bankIDMobileButton.Enabled = true;
            if (LoginMode == LoginMode.Biometric)
            {
                RequestFingerprintAuthentication();
            }
        }

        void Cancel_Clink(object sender, EventArgs e)
        {
            if(Presenter.CanCancelBankID)
            {
                Presenter.CancelBankID();
            }
        }


        private void RequestFingerprintAuthentication()
        {
            if (fingerprintHandler == null)
            {
                fingerprintHandler = new FingerprintHandler(Activity as BaseActivity, FragmentManager);
            }
            if (fingerprintHandler.IsFingerprintSupported())
            {
                fingerprintHandler.StartFingerprintAuthentication(OnFingerprintAuthenticationSuccess, OnFingerprintAuthenticationFailure);
            }
        }

        void OnFingerprintAuthenticationSuccess()
        {
            LoginWithBio().Forget();
        }

        void OnFingerprintAuthenticationFailure()
        {
            String message = Resources.GetString(Resource.String.touchid_login_failure);
            String yesTitle = Resources.GetString(Resource.String.general_view_yes);

            snackbar = Snackbar.Make((Activity as BaseActivity).RootView, message, Snackbar.LengthIndefinite);
            snackbar.SetAction(yesTitle, OnReFingerprintAuthenticationRequest);
            snackbar.Show();
        }

        void OnReFingerprintAuthenticationRequest(View obj)
        {
            snackbar.Dismiss();
            RequestFingerprintAuthentication();
        }


        private async Task LoginWithBio()
        {
            ShowLoader();
            var response = await Presenter.LoginWithTouchId();
            HideLoader();

            if (!response.IsSuccess)
            {

            }
        }

        public override void OnDestroyView()
        {
            base.OnDestroyView();
            CloseSnackbar();
        }

        private void CloseSnackbar()
        {
            if (snackbar != null)
            {
                snackbar.Dismiss();
            }
        }

        void BankIDButton_Click(object sender, EventArgs e)
        {
            bankIDButton.Enabled = false;
            bankIDMobileButton.Enabled = false;
            CloseSnackbar();
            Presenter.StartBankId();
        }

        void BankIDMobileButton_Click(object sender, EventArgs e)
        {
            bankIDButton.Enabled = false;
            bankIDMobileButton.Enabled = false;
            CloseSnackbar();
            Presenter.StartBankIdMobile();
        }

        protected override void Presenter_ErrorOccured(object sender, BaseErrorResponseInfo e)
        {
            base.Presenter_ErrorOccured(sender, e);
            bankIDButton.Enabled = true;
            bankIDMobileButton.Enabled = true;
        }

    }
}
