
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using Helseboka.Core.Common.CommonImpl;
using Helseboka.Core.Common.Interfaces;
using Helseboka.Core.Common.Model;
using Helseboka.Core.Startup.Interface;
using Helseboka.Droid.Common.Utils;
using Helseboka.Droid.Common.Views;

namespace Helseboka.Droid.Startup.Views
{
    public class RegistrationOptionsFragment : BaseFragment
    {
        Button touchIDButton;
        Button pinButton;
        Button cancelButton;
        FingerprintHandler fingerprintHandler;

        private ISignupPresenter Presenter
        {
            get => presenter as ISignupPresenter;
        }

        private IDeviceHandler DeviceHandler
        {
            get => ApplicationCore.Container.Resolve<IDeviceHandler>();
        }

        public RegistrationOptionsFragment(ISignupPresenter presenter)
        {
            this.presenter = presenter;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            DeRegisterEvents();
            var view = inflater.Inflate(Resource.Layout.fragment_pin_registration_options, null);

            touchIDButton = view.FindViewById<Button>(Resource.Id.buttonTouchId);
            pinButton = view.FindViewById<Button>(Resource.Id.buttonPIN);
            cancelButton = view.FindViewById<Button>(Resource.Id.cancelButton);

            cancelButton.Visibility = ViewStates.Invisible;

            fingerprintHandler = new FingerprintHandler(Activity as BaseActivity, FragmentManager);

            RegisterEvents();

            if (!fingerprintHandler.IsFingerprintSupported())
            {
                touchIDButton.Visibility = ViewStates.Gone;
            }

            return view;
        }

        void RegisterEvents()
        {
            if (touchIDButton != null)
            {
                touchIDButton.Click += TouchIDButton_Click;
            }

            if (pinButton != null)
            {
                pinButton.Click += PinButton_Click;
            }

            //if (cancelButton != null)
            //{
            //    cancelButton.Click += CancelButton_Click;
            //}
        }

        void DeRegisterEvents()
        {
            if (touchIDButton != null)
            {
                touchIDButton.Click -= TouchIDButton_Click;
            }

            if (pinButton != null)
            {
                pinButton.Click -= PinButton_Click;
            }

            //if (cancelButton != null)
            //{
            //    cancelButton.Click -= CancelButton_Click;
            //}
        }

        //void CancelButton_Click(object sender, EventArgs e)
        //{
        //    Presenter.CancelPINBioRegister();
        //}


        void PinButton_Click(object sender, EventArgs e)
        {
            Presenter.ShowPINRegistration();
        }


        void TouchIDButton_Click(object sender, EventArgs e)
        {
            fingerprintHandler.StartFingerprintAuthentication(OnFingerprintAuthenticationSuccess, OnFingerprintAuthenticationFailure);
        }

        void OnFingerprintAuthenticationSuccess()
        {
            Presenter.RegisterBiometric();
        }

        void OnFingerprintAuthenticationFailure()
        {
            String message = Resources.GetString(Resource.String.touchid_registration_failure);
            Snackbar.Make((Activity as BaseActivity).RootView, message, Snackbar.LengthLong).Show();
        }
    }
}
