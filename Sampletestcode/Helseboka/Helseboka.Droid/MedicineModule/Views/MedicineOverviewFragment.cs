
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Helseboka.Core.MedicineModule.Interface;
using Helseboka.Core.MedicineModule.Model;
using Helseboka.Droid.Common.Views;
using Helseboka.Droid.Common.Utils;
using Helseboka.Core.Common.Extension;
using Android.Text;
using Android.Text.Style;
using Android.Graphics;
using Helseboka.Core.Common.Constant;

namespace Helseboka.Droid.MedicineModule.Views
{
    public class MedicineOverviewFragment : BaseFragment
    {
        ImageView back;
        ImageView delete;
        TextView pageTitle;
        TextView pageSubTitle;
        TextView setReminderTitle;
        TextView reminderDetails;
        TextView externalLinkTitle;
        Button addMedicineToProfile;
        RelativeLayout reminderContainer;

        MedicineReminder MedicineDetails { get; set; }

        private IMedicineHomePresenter Presenter
        {
            get => presenter as IMedicineHomePresenter;
        }

        private Boolean IsFromSearch { get; set; }

        public MedicineOverviewFragment(IMedicineHomePresenter presenter, bool isfromSearch, MedicineReminder medicine)
        {
            this.presenter = presenter;
            this.IsFromSearch = isfromSearch;
            this.MedicineDetails = medicine;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_medicine_overview, null);

            back = view.FindViewById<ImageView>(Resource.Id.back);
            delete = view.FindViewById<ImageView>(Resource.Id.delete);
            pageTitle = view.FindViewById<TextView>(Resource.Id.pageTitle);
            pageSubTitle = view.FindViewById<TextView>(Resource.Id.pageSubTitle);
            setReminderTitle = view.FindViewById<TextView>(Resource.Id.setReminderTitle);
            reminderDetails = view.FindViewById<TextView>(Resource.Id.reminderDetails);
            externalLinkTitle = view.FindViewById<TextView>(Resource.Id.externalLinkTitle);
            addMedicineToProfile = view.FindViewById<Button>(Resource.Id.addMedicineToProfile);
            reminderContainer = view.FindViewById<RelativeLayout>(Resource.Id.reminderContainer);

            back.Click += Back_Click;
            reminderContainer.Click += ReminderTitle_Click;
            externalLinkTitle.Click += ExternalLinkTitle_Click;

            pageTitle.Text = MedicineDetails.Medicine.Name;
            pageSubTitle.Text = MedicineDetails.Medicine.Strength;

            if (MedicineDetails.HasReminder && MedicineDetails.Reminder != null && MedicineDetails.Reminder.Days != null && MedicineDetails.Reminder.FrequencyPerDay != null)
            {
                setReminderTitle.Text = Resources.GetText(Resource.String.medicine_overview_alarmlabel_edit_title);

                var localizedList = MedicineDetails.Reminder.Days.Select(x => $"medicine_overview_{x}".ToLower().Translate(Activity)).ToList();

                var reminderDays = String.Join(", ", localizedList);
                var frequencies = $" {Resources.GetString(Resource.String.general_view_timeprefix)} {String.Join(" ", MedicineDetails.Reminder.FrequencyPerDay)}";

                var totalText = reminderDays + frequencies;
                var formattedText = new SpannableStringBuilder(totalText);
                formattedText.SetSpan(new ForegroundColorSpan(Color.ParseColor("#121212")), reminderDays.Length, totalText.Length, SpanTypes.InclusiveInclusive);
                reminderDetails.TextFormatted = formattedText;
            }
            else
            {
                setReminderTitle.Text = Resources.GetText(Resource.String.medicine_overview_alarmlabel_add_title);
                reminderDetails.Visibility = ViewStates.Gone;
            }

            if (Presenter.IsMedicineExistInProfile(MedicineDetails.Medicine))
            {
                addMedicineToProfile.Visibility = ViewStates.Gone;
                delete.Visibility = ViewStates.Visible;

                addMedicineToProfile.Click += AddMedicineToProfile_Click;

                delete.Click -= Delete_Click;
                delete.Click += Delete_Click;
            }
            else
            {
                addMedicineToProfile.Visibility = ViewStates.Visible;
                delete.Visibility = ViewStates.Invisible;

                delete.Click -= Delete_Click;

                addMedicineToProfile.Click -= AddMedicineToProfile_Click;
                addMedicineToProfile.Click += AddMedicineToProfile_Click;
            }

            return view;
        }

        void Back_Click(object sender, EventArgs e)
        {
            Presenter.GoBack();
        }

        public override bool OnBackKeyPressed()
        {
            Presenter.GoBack();
            return true;
        }

        void Delete_Click(object sender, EventArgs e)
        {
            var title = Resources.GetString(Resource.String.medicine_overview_confirm_delete_title);
            var message = Resources.GetString(Resource.String.medicine_overview_confirm_delete_message);

            var dialog = new YesNoDialog(Activity, message, title, DeleteConfirmation_Yes_Selected);
            dialog.Show();
        }

        void DeleteConfirmation_Yes_Selected()
        {
            Presenter.DeleteMedicine(MedicineDetails).Forget();
        }


        void ReminderTitle_Click(object sender, EventArgs e)
        {
            Presenter.NavigateToMedicineAlarmView(MedicineDetails);
        }

        void ExternalLinkTitle_Click(object sender, EventArgs e)
        {
            Android.Net.Uri medicieUri;
            try
            {
                medicieUri = Android.Net.Uri.Parse(String.Format(AppConstant.ReadMoreUrlFormat, MedicineDetails.Medicine.Name.ToURLEncodedString()));
            }
            catch
            {
                medicieUri = Android.Net.Uri.Parse(AppConstant.ReadMoreUrl);
            }

            var browserIntent = new Intent(Intent.ActionView, medicieUri);
            StartActivity(browserIntent);
        }

        void AddMedicineToProfile_Click(object sender, EventArgs e)
        {
            Presenter.AddMedicineToProfile(MedicineDetails).Forget();
        }

    }
}
