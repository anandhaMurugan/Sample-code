
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
using Helseboka.Droid.Common.Views;
using Helseboka.Droid.Common.Utils;
using Helseboka.Core.Common.Extension;
using Helseboka.Droid.Common.Adapters;
using Helseboka.Core.MedicineModule.Model;
using Android.Support.V7.Widget;
using Android.Graphics.Drawables;
using Android.Graphics;
using Android.Text;
using Android.Text.Style;
using Android.Support.V4.Content;

namespace Helseboka.Droid.MedicineModule.Views
{
    public class ConflictReminderDialog : IUniversalAdapter
    {
        private Activity activity;
        private Dialog dialog;
        private List<MedicineReminder> MedicineReminderList { get; set; }
        private Action onYesTapped;
        private Action onNoTapped;

        private MaxHeightRecyclerView dataListView;
        private ImageView closeButton;
        private Button okButton;
        private Button cancelButton;

        public ConflictReminderDialog(Activity activity, List<MedicineReminder> medicineReminderList, Action onYesTapped, Action onNoTapped)
        {
            this.activity = activity;
            this.MedicineReminderList = medicineReminderList;
            this.onYesTapped = onYesTapped;
            this.onNoTapped = onNoTapped;
        }

        public void Show()
        {
            var inflater = (LayoutInflater)activity.GetSystemService(Context.LayoutInflaterService);

            var view = inflater.Inflate(Resource.Layout.dialog_medicine_conflict_alert, null);

            dataListView = view.FindViewById<MaxHeightRecyclerView>(Resource.Id.dataListView);
            closeButton = view.FindViewById<ImageView>(Resource.Id.closeButton);
            okButton = view.FindViewById<Button>(Resource.Id.okButton);
            cancelButton = view.FindViewById<Button>(Resource.Id.cancelButton);

            closeButton.Click += CloseButton_Click;
            okButton.Click += OkButton_Click;
            cancelButton.Click += CancelButton_Click;

            dataListView.SetLayoutManager(new LinearLayoutManager(activity, LinearLayoutManager.Vertical, false));
            dataListView.SetAdapter(new GenericRecyclerAdapter(this));

            dialog = new Dialog(activity, Android.Resource.Style.ThemeLightNoTitleBar);
            dialog.Window.SetBackgroundDrawable(new ColorDrawable(Color.Transparent));
            dialog.SetContentView(view);
            dialog.SetCancelable(false);

            dialog.Show();

            dataListView.GetAdapter().NotifyDataSetChanged();
        }

        void OkButton_Click(object sender, EventArgs e)
        {
            onYesTapped?.Invoke();
            CloseDialog();
        }

        void CancelButton_Click(object sender, EventArgs e)
        {
            onNoTapped?.Invoke();
            CloseDialog();
        }


        void CloseButton_Click(object sender, EventArgs e)
        {
            CloseDialog();
        }

        private void CloseDialog()
        {
            if (dialog != null)
            {
                dialog.Dismiss();
            }
        }

        public void BindViewHolder(UniversalViewHolder holder, int position)
        {
            if (MedicineReminderList != null && position < MedicineReminderList.Count)
            {
                var medicineReminder = MedicineReminderList[position];
                var medicineName = holder.GetView<TextView>(Resource.Id.medicineName);
                var reminderDetails = holder.GetView<TextView>(Resource.Id.reminderDetails);

                var strengthText = $", {medicineReminder.Medicine.Strength}";
                var totalText = medicineReminder.Medicine.Name + strengthText;
                var formattedText = new SpannableStringBuilder(totalText);

                formattedText.SetSpan(new StyleSpan(TypefaceStyle.Bold), 0, medicineReminder.Medicine.Name.Length, SpanTypes.InclusiveInclusive);
                formattedText.SetSpan(new AbsoluteSizeSpan(18.ConvertToPixel(activity)), 0, medicineReminder.Medicine.Name.Length, SpanTypes.InclusiveInclusive);

                formattedText.SetSpan(new StyleSpan(TypefaceStyle.Normal), medicineReminder.Medicine.Name.Length, totalText.Length, SpanTypes.InclusiveInclusive);
                formattedText.SetSpan(new AbsoluteSizeSpan(14.ConvertToPixel(activity)), medicineReminder.Medicine.Name.Length, totalText.Length, SpanTypes.InclusiveInclusive);

                medicineName.TextFormatted = formattedText;

                if (medicineReminder.Reminder != null)
                {
                    var localizedList = medicineReminder.Reminder.Days.Select(x => x.ToString().ToLower().Translate(activity)).ToList();
                    var reminderDays = String.Join(", ", localizedList);
                    var frequencies = $" {activity.Resources.GetString(Resource.String.general_view_timeprefix)} {String.Join(" ", medicineReminder.Reminder.FrequencyPerDay)}";

                    var formattedReminderText = new SpannableStringBuilder(reminderDays + frequencies);
                    formattedReminderText.SetSpan(new ForegroundColorSpan(Color.ParseColor("#121212")), 0, frequencies.Length, SpanTypes.InclusiveInclusive);

                    reminderDetails.TextFormatted = formattedReminderText;
                }
            }
        }

        public UniversalViewHolder CreateItemView(ViewGroup parent, int viewType)
        {
            var inflater = LayoutInflater.FromContext(activity);
            var view = inflater.Inflate(Resource.Layout.template_reminder_conflict_item, null);

            var viewMap = new Dictionary<int, View>();
            viewMap.Add(Resource.Id.medicineName, view.FindViewById(Resource.Id.medicineName));
            viewMap.Add(Resource.Id.reminderDetails, view.FindViewById(Resource.Id.reminderDetails));

            return new UniversalViewHolder(view, viewMap);
        }

        public int GetItemCount()
        {
            return MedicineReminderList != null ? MedicineReminderList.Count : 0;
        }

        public void OnItemClick(int position)
        {
        }
    }
}
