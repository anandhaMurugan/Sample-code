
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Text.Style;
using Android.Util;
using Android.Views;
using Android.Widget;
using Helseboka.Core.Common.Constant;
using Helseboka.Core.Common.Extension;
using Helseboka.Core.Profile.Interface;
using Helseboka.Core.Profile.Model;
using Helseboka.Core.Resources.StringResources;
using Helseboka.Droid.Common.Utils;
using Helseboka.Droid.Common.Views;

namespace Helseboka.Droid.ProfileModule.Views
{
    public class ProfileHomeFragment : BaseFragment
    {
        private TextView pageTitle;
        private TextView personalSettings;
        private TextView medicalOfficeValue;
        private TextView doctorValue;
        private TextView pinLabel;
        private TextView changePINLabel;
        private View pinSeparator;
        private TextView gdprLabel;
        private TextView deleteProfileLabel;
        private TextView logoutLabel;
        private TextView feedbackLabel;
        ImageView helpButton;

        private User currentUser;

        public IProfilePresenter Presenter
        {
            get => presenter as IProfilePresenter;
        }

        public ProfileHomeFragment(IProfilePresenter presenter)
        {
            this.presenter = presenter;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_profile_home, null);

            pageTitle = view.FindViewById<TextView>(Resource.Id.pageTitle);
            personalSettings = view.FindViewById<TextView>(Resource.Id.personalSettings);
            medicalOfficeValue = view.FindViewById<TextView>(Resource.Id.medicalOfficeValue);
            doctorValue = view.FindViewById<TextView>(Resource.Id.doctorValue);
            pinLabel = view.FindViewById<TextView>(Resource.Id.pinLabel);
            changePINLabel = view.FindViewById<TextView>(Resource.Id.changePINLabel);
            pinSeparator = view.FindViewById<Android.Views.View>(Resource.Id.pinSeparator);
            gdprLabel = view.FindViewById<TextView>(Resource.Id.gdprLabel);
            deleteProfileLabel = view.FindViewById<TextView>(Resource.Id.deleteProfileLabel);
            logoutLabel = view.FindViewById<TextView>(Resource.Id.logoutLabel);
            feedbackLabel = view.FindViewById<TextView>(Resource.Id.feedbackLabel);
            helpButton = view.FindViewById<ImageView>(Resource.Id.helpButton);

            helpButton.Click += HelpButton_Click;
            personalSettings.Click += PersonalSettings_Click;
            medicalOfficeValue.Click += DoctorOrMedicalCenter_Click;
            doctorValue.Click += DoctorOrMedicalCenter_Click;
            changePINLabel.Click += ChangePIN_Click;
            gdprLabel.Click += GDPR_Click;
            deleteProfileLabel.Click += DeleteProfile_Click;
            logoutLabel.Click += Logout_Click;
            feedbackLabel.Click += FeedbackLabel_Click;

            LoadData().Forget();

            return view;
        }

        void HelpButton_Click(object sender, EventArgs e)
        {
            ShowHelpView(Core.Common.EnumDefinitions.HelpFAQType.ProfileHome).Forget();
        }

        void DeleteProfile_Click(object sender, EventArgs e)
        {
            var dialog = new YesNoDialog(Activity, AppResources.DeleteProfileConfirmationMessage, AppResources.DeleteProfileConfirmationTitle, DeleteProfileConfirmation_YesTapped, AppResources.DeleteProfileConfirmationButton);
            dialog.Show();
        }

        void DeleteProfileConfirmation_YesTapped()
        {
            Presenter.DeleteProfile().Forget();
        }


        void PersonalSettings_Click(object sender, EventArgs e)
        {
            Presenter.ShowUserInfoView();
        }

        void DoctorOrMedicalCenter_Click(object sender, EventArgs e)
        {
            Presenter.ShowDoctorAndOfficeDetailsView();
        }

        void ChangePIN_Click(object sender, EventArgs e)
        {
            Presenter.ShowPINConfirmation();
        }

        void GDPR_Click(object sender, EventArgs e)
        {
            Presenter.ShowTermsPage();
        }

        void Logout_Click(object sender, EventArgs e)
        {
            Presenter.Logout();
        }

        void FeedbackLabel_Click(object sender, EventArgs e)
        {
            var message = String.Format(AppResources.FeedbackPopupMessage, AppConstant.FeedbackEmailAddress);
            var totalMessage = message + System.Environment.NewLine + System.Environment.NewLine + AppResources.FeedbackPopupAlert;
            var formattedText = new SpannableStringBuilder(totalMessage);
            formattedText.SetSpan(new ForegroundColorSpan(Color.Red), message.Length, totalMessage.Length - 1, SpanTypes.InclusiveInclusive);

            var dialog = new YesNoDialog(Activity, String.Empty, AppResources.FeedbackPopupTitle, FeedbackPopup_YesTapped, AppResources.GeneralTextContinue, AppResources.GeneralTextCancel);
            dialog.FormattedMessage = formattedText;
            dialog.Show();
        }

        void FeedbackPopup_YesTapped()
        {
            var emailIntent = new Intent(Android.Content.Intent.ActionSend);
            emailIntent.SetData(Android.Net.Uri.Parse("mailto:"));
            emailIntent.SetType("message/rfc822");
            emailIntent.PutExtra(Android.Content.Intent.ExtraEmail, new[] { AppConstant.FeedbackEmailAddress });
            emailIntent.PutExtra(Android.Content.Intent.ExtraSubject, AppResources.FeedbackMailSubject);
            StartActivity(Intent.CreateChooser(emailIntent, AppResources.FeedbackMailAppSelectorTitle));
        }


        private async Task LoadData()
        {
            currentUser = await Presenter.GetCurrentUserProfile();
            if (currentUser != null)
            {
                pageTitle.Text = $"{currentUser.FirstName} {currentUser.LastName}";
                medicalOfficeValue.Text = currentUser.AssignedDoctor.OfficeName.ToNameCase();
                doctorValue.Text = currentUser.AssignedDoctor.FullName;
                var loginMode = Presenter.GetLoginMode();
                if (loginMode.HasValue && loginMode.Value == Core.Common.EnumDefinitions.LoginMode.PIN)
                {
                    pinLabel.Visibility = ViewStates.Visible;
                    changePINLabel.Visibility = ViewStates.Visible;
                    pinSeparator.Visibility = ViewStates.Visible;
                }
                else
                {
                    pinLabel.Visibility = ViewStates.Gone;
                    changePINLabel.Visibility = ViewStates.Gone;
                    pinSeparator.Visibility = ViewStates.Gone;
                }
            }
        }

    }
}
