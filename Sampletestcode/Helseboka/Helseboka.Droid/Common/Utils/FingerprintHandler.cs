using System;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.Content;
using Android.Support.V4.Hardware.Fingerprint;
using Android.Util;
using Android.Views;
using Android.Widget;
using Res = Android.Resource;
using Android.Hardware.Fingerprints;
using Java.Lang;
using Javax.Crypto;
using CancellationSignal = Android.Support.V4.OS.CancellationSignal;
using Android.Support.V4.App;
using Helseboka.Droid.Common.Views;

namespace Helseboka.Droid.Common.Utils
{
    public class FingerprintHandler
    {
        BaseActivity activity;
        FingerprintManagerCompat fingerprintManager;
        KeyguardManager keyguardManager;
        Android.Support.V4.App.FragmentManager fragmentManager;

        public FingerprintHandler(BaseActivity activity, Android.Support.V4.App.FragmentManager fragmentManager)
        {
            this.activity = activity;
            this.fragmentManager = fragmentManager;
        }

        public bool IsFingerprintSupported()
        {
            if (Android.OS.Build.VERSION.SdkInt < Android.OS.BuildVersionCodes.M)
            {
                return false;
            }

            if(fingerprintManager == null)
            {
                fingerprintManager = FingerprintManagerCompat.From(activity);
            }
            if (fingerprintManager == null || !fingerprintManager.IsHardwareDetected)
            {
                return false;
            }
            if (!fingerprintManager.HasEnrolledFingerprints)
            {
                return false;
            }

            if(keyguardManager == null)
            {
                keyguardManager = (KeyguardManager)activity.GetSystemService(Context.KeyguardService);
            }
            if (keyguardManager == null || !keyguardManager.IsKeyguardSecure)
            {
                return false;
            }

            return true;
        }

        public void StartFingerprintAuthentication(Action onAuthenticationsuccess, Action onAuthenticationFailure)
        {
            if(IsFingerprintSupported())
            {
                Permission permissionResult = ContextCompat.CheckSelfPermission(activity, Manifest.Permission.UseFingerprint);
                if(permissionResult == Permission.Granted)
                {
                    var dialogFragment = new FingerprintManagerApiDialogFragment(onAuthenticationsuccess, onAuthenticationFailure);
                    dialogFragment.Init();
                    dialogFragment.Show(fragmentManager, "fingerprint_auth_fragment");
                }
                else
                {
                    // As Fingerprint authentication is a normal permission so no need to ask explicitly permission for it.
                    onAuthenticationFailure?.Invoke();
                }
            }
            else
            {
                onAuthenticationFailure?.Invoke();
            }
        }
    }

    /// <summary>
    ///     This DialogFragment is displayed when the app is scanning for fingerprints.
    /// </summary>
    /// <remarks>
    ///     https://github.com/xamarin/monodroid-samples/tree/master/android-m/RuntimePermissions
    ///     This DialogFragment doesn't perform any checks to see if the device
    ///     is actually eligible for fingerprint authentication. All of those checks are performed by the
    ///     Activity.
    /// </remarks>
    public class FingerprintManagerApiDialogFragment : Android.Support.V4.App.DialogFragment
    {
        Button cancelButton;
        CancellationSignal cancellationSignal;
        FingerprintManagerCompat fingerprintManager;
        Action onAuthenticationsuccess;
        Action onAuthenticationFailure;

        bool ScanForFingerprintsInOnResume { get; set; } = true;

        bool UserCancelledScan { get; set; }

        CryptoObjectHelper CryptObjectHelper { get; set; }

        bool IsScanningForFingerprints
        {
            get { return cancellationSignal != null; }
        }

        public FingerprintManagerApiDialogFragment(Action onAuthenticationsuccess, Action onAuthenticationFailure)
        {
            this.onAuthenticationsuccess = onAuthenticationsuccess;
            this.onAuthenticationFailure = onAuthenticationFailure;
        }

        public void Init(bool startScanning = true)
        {
            ScanForFingerprintsInOnResume = startScanning;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            fingerprintManager = FingerprintManagerCompat.From(Context);
            RetainInstance = true;
            CryptObjectHelper = new CryptoObjectHelper();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Dialog.SetTitle(Resources.GetString(Resource.String.touchid_dialog_title));

            View v = inflater.Inflate(Resource.Layout.dialog_scanning_for_fingerprint, container, false);

            cancelButton = v.FindViewById<Button>(Resource.Id.cancel_button);
            cancelButton.Click += (sender, args) =>
            {
                UserCancelledScan = true;
                StopListeningForFingerprints();
            };

            return v;
        }

        public override void OnResume()
        {
            base.OnResume();
            if (!ScanForFingerprintsInOnResume)
            {
                return;
            }

            UserCancelledScan = false;
            cancellationSignal = new CancellationSignal();
            fingerprintManager.Authenticate(CryptObjectHelper.BuildCryptoObject(),
                                            (int)FingerprintAuthenticationFlags.None, /* flags */
                                             cancellationSignal,
                                             new SimpleAuthCallbacks(this),
                                             null);
        }

        public override void OnPause()
        {
            base.OnPause();
            if (IsScanningForFingerprints)
            {
                StopListeningForFingerprints(true);
            }
        }

        void StopListeningForFingerprints(bool butStartListeningAgainInOnResume = false)
        {
            if (cancellationSignal != null)
            {
                cancellationSignal.Cancel();
                cancellationSignal = null;
            }
            ScanForFingerprintsInOnResume = butStartListeningAgainInOnResume;
        }

        public override void OnDestroyView()
        {
            // see https://code.google.com/p/android/issues/detail?id=17423
            if (Dialog != null && RetainInstance)
            {
                Dialog.SetDismissMessage(null);
            }
            base.OnDestroyView();
            onAuthenticationsuccess = null;
            onAuthenticationFailure = null;
        }

        class SimpleAuthCallbacks : FingerprintManagerCompat.AuthenticationCallback
        {
            static readonly byte[] SECRET_BYTES = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            readonly FingerprintManagerApiDialogFragment fragment;

            public SimpleAuthCallbacks(FingerprintManagerApiDialogFragment frag)
            {
                fragment = frag;
            }

            public override void OnAuthenticationSucceeded(FingerprintManagerCompat.AuthenticationResult result)
            {
                if (result.CryptoObject.Cipher != null)
                {
                    try
                    {
                        // Calling DoFinal on the Cipher ensures that the encryption worked.
                        byte[] doFinalResult = result.CryptoObject.Cipher.DoFinal(SECRET_BYTES);
                        ReportSuccess();
                    }
                    catch (BadPaddingException)
                    {
                        ReportAuthenticationFailed();
                    }
                    catch (IllegalBlockSizeException)
                    {
                        ReportAuthenticationFailed();
                    }
                }
                else
                {
                    // No cipher used, assume that everything went well and trust the results.
                    ReportSuccess();
                }
            }

            void ReportSuccess()
            {
                fragment.onAuthenticationsuccess?.Invoke();
                fragment.Dismiss();
            }

            void ReportScanFailure(int errMsgId)
            {
                fragment.onAuthenticationFailure?.Invoke();
                fragment.Dismiss();
            }

            void ReportAuthenticationFailed()
            {
                fragment.onAuthenticationFailure?.Invoke();
                fragment.Dismiss();
            }

            public override void OnAuthenticationError(int errMsgId, ICharSequence errString)
            {
                // There are some situations where we don't care about the error. For example, 
                // if the user cancelled the scan, this will raise errorID #5. We don't want to
                // report that, we'll just ignore it as that event is a part of the workflow.
                bool reportError = (errMsgId == (int)FingerprintState.ErrorCanceled) &&
                                   !fragment.ScanForFingerprintsInOnResume;

                if (fragment.UserCancelledScan)
                {
                    ReportScanFailure(-1);
                }
                else if (reportError)
                {
                    ReportScanFailure(errMsgId);
                }
                else
                {
                    ReportAuthenticationFailed();
                }
            }

            public override void OnAuthenticationFailed()
            {
                ReportAuthenticationFailed();
            }

            public override void OnAuthenticationHelp(int helpMsgId, ICharSequence helpString)
            {
                ReportScanFailure(helpMsgId);
            }
        }
    }
}
