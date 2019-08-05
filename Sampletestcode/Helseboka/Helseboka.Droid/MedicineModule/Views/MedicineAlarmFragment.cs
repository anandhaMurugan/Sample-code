
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
using Helseboka.Core.Common.Extension;
using Helseboka.Core.MedicineModule.Interface;
using Helseboka.Core.MedicineModule.Model;
using Helseboka.Droid.Common.Utils;
using Helseboka.Droid.Common.Views;

namespace Helseboka.Droid.MedicineModule.Views
{
    public class MedicineAlarmFragment : BaseFragment
    {
        ImageView back;
        TextView pageTitle;
        TextView pageSubTitle;
        Button saveAlarmButton;
        Switch toggleAlarmSwitch;
        LinearLayout selectedMedicineContainer;
        TextView addMoreMedicineTitle;
        ImageView addMoreMedicineImageView;
        LinearLayout weekViewContainer;
        LinearLayout selectedTimesContainer;
        TextView addMoreTimesTitle;
        ImageView addMoreTimesImageView;
        ScrollView containerScrollView;

        private HashSet<MedicineInfo> medicineList = new HashSet<MedicineInfo>();
        private List<DateTime> frequencies = new List<DateTime>();
        private List<DayOfWeek> SelectedDays = new List<DayOfWeek>();
        private Boolean AlarmToggleStatus = true;

        private IMedicineAlarmPresenter Presenter
        {
            get => presenter as IMedicineAlarmPresenter;
        }
        private MedicineReminder Medicine { get; set; }

        public MedicineAlarmFragment(IMedicineAlarmPresenter presenter, MedicineReminder medicineDetails)
        {
            this.presenter = presenter;
            this.Medicine = medicineDetails;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_medicine_alarmview, null);

            back = view.FindViewById<ImageView>(Resource.Id.back);
            pageTitle = view.FindViewById<TextView>(Resource.Id.pageTitle);
            pageSubTitle = view.FindViewById<TextView>(Resource.Id.pageSubTitle);
            saveAlarmButton = view.FindViewById<Button>(Resource.Id.saveAlarmButton);
            toggleAlarmSwitch = view.FindViewById<Switch>(Resource.Id.toggleAlarmSwitch);
            selectedMedicineContainer = view.FindViewById<LinearLayout>(Resource.Id.selectedMedicineContainer);
            addMoreMedicineTitle = view.FindViewById<TextView>(Resource.Id.addMoreMedicineTitle);
            addMoreMedicineImageView = view.FindViewById<ImageView>(Resource.Id.addMoreMedicineImageView);
            weekViewContainer = view.FindViewById<LinearLayout>(Resource.Id.weekViewContainer);
            selectedTimesContainer = view.FindViewById<LinearLayout>(Resource.Id.selectedTimesContainer);
            addMoreTimesTitle = view.FindViewById<TextView>(Resource.Id.addMoreTimesTitle);
            addMoreTimesImageView = view.FindViewById<ImageView>(Resource.Id.addMoreTimesImageView);
            containerScrollView = view.FindViewById<ScrollView>(Resource.Id.containerScrollView);

            back.Click += Back_Click;
            saveAlarmButton.Click += SaveAlarmButton_Click;
            addMoreMedicineTitle.Click += AddMoreMedicine_Click;
            addMoreMedicineImageView.Click += AddMoreMedicine_Click;
            addMoreTimesTitle.Click += AddMoreTimes_Click;
            addMoreTimesImageView.Click += AddMoreTimes_Click;
            toggleAlarmSwitch.CheckedChange += ToggleAlarmSwitch_CheckedChange;

            pageTitle.Text = Medicine.Medicine.Name;
            pageSubTitle.Text = Medicine.Medicine.Strength;

            LoadExsistingReminderDetails();
            LoadFrequencies();
            LoadAlarmView();

            return view;
        }

        void ToggleAlarmSwitch_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            AlarmToggleStatus = e.IsChecked;
            Validate();
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

        void SaveAlarmButton_Click(object sender, EventArgs e)
        {
            if (AlarmToggleStatus)
            {
                var conflictingList = Presenter.GetConflictingReminders(medicineList.ToList());

                if (conflictingList != null && conflictingList.Count > 0)
                {
                    var conflictDialog = new ConflictReminderDialog(Activity, conflictingList, ConflictDialog_Yes_Tapped, ConflictDialog_No_Tapped);
                    conflictDialog.Show();
                }
                else
                {
                    Save().Forget();
                }
            }
            else
            {
                DeleteReminder().Forget();
            }
        }

        void ConflictDialog_Yes_Tapped()
        {
            Save().Forget();
        }

        void ConflictDialog_No_Tapped()
        {
            medicineList.Clear();
            Save().Forget();
            LoadSelectedMedicineView();
        }


        void AddMoreMedicine_Click(object sender, EventArgs e)
        {
            var existingMedicineList = medicineList.ToList();
            existingMedicineList.Add(Medicine.Medicine);

            var selectableMedicineList = Presenter.GetNewMedicines(existingMedicineList);

            if (selectableMedicineList != null && selectableMedicineList.Count > 0)
            {
                var medicineSelectionView = new SelectMedicineFragment(Activity, selectableMedicineList, Medicine.Medicine, MedicineSelecteion_Completed);
                medicineSelectionView.Show();
            }
            else
            {
                ShowInfoDialog(Resources.GetString(Resource.String.medicine_reminder_alert_nomedicine));
            }
        }

        void MedicineSelecteion_Completed(HashSet<MedicineInfo> selectedMedicines)
        {
            selectedMedicines.Remove(Medicine.Medicine);
            medicineList.UnionWith(selectedMedicines);

            LoadSelectedMedicineView();
            Validate();
        }

        void DeleteMedicine_Click(object sender, EventArgs e)
        {
            if (sender is Android.Views.View deleteButton)
            {
                if(deleteButton.Tag is Android.Views.View parentView)
                {
                    var medicine = medicineList.Where(data => data.Id == (int)parentView.Tag).FirstOrDefault();
                    if (medicine != null)
                    {
                        medicineList.Remove(medicine);
                    }
                    selectedMedicineContainer.RemoveView(parentView);
                }
            }

            Validate();
        }


        void Time_Selected(DateTime selectedTime)
        {
            frequencies.Add(selectedTime);
            AddNewReminderTime(selectedTime);
            Validate();
        }

        void Time_Deleted(object sender, EventArgs e)
        {
            if(sender is View deleteButton)
            {
                if(deleteButton.Tag is View parentView)
                {
                    var time = (string)parentView.Tag;
                    if(!String.IsNullOrEmpty(time))
                    {
                        var itemToRemove = frequencies.Where((x) => x.GetTimeString() == time).ToList();

                        if (itemToRemove != null && itemToRemove.Count > 0)
                        {
                            var index = frequencies.IndexOf(itemToRemove[0]);
                            frequencies.Remove(itemToRemove[0]);
                            selectedTimesContainer.RemoveView(parentView);
                        }
                    }
                }
            }
            Validate();
        }


        void AddMoreTimes_Click(object sender, EventArgs e)
        {
            TimePickerFragment frag = TimePickerFragment.NewInstance(
                delegate (DateTime time)
                {
                    Activity.RunOnUiThread(()=>Time_Selected(time));
                });

            frag.Show(FragmentManager, TimePickerFragment.TAG);
        }

        void WeekView_Click(object sender, EventArgs e)
        {
            if(sender is View view)
            {
                var selectionBox = view.FindViewById<ImageView>(Resource.Id.selectionBox);
                selectionBox.Selected = !selectionBox.Selected;

                var day = (DayOfWeek)((int)view.Tag);
                var isExist = SelectedDays.Exists((obj) => obj == day);
                if(selectionBox.Selected)
                {
                    if (!isExist)
                    {
                        SelectedDays.Add(day);
                    }
                }
                else
                {
                    if (isExist)
                    {
                        SelectedDays.Remove(day);
                    }
                }
                Validate();
            }
        }


        public void LoadExsistingReminderDetails()
        {
            if (Medicine != null && Medicine.Reminder != null)
            {
                var reminder = Medicine.Reminder;

                if (reminder.Days != null && reminder.Days.Count > 0)
                {
                    SelectedDays = reminder.Days;
                }

                if (reminder.FrequencyPerDay != null && reminder.FrequencyPerDay.Count > 0)
                {
                    foreach (var item in reminder.FrequencyPerDay)
                    {
                        var frequencyTime = item.ConvertTimeToDateTime();
                        if(frequencyTime.HasValue)
                        {
                            frequencies.Add(frequencyTime.Value);
                        }
                    }
                }
            }
        }

        private void LoadFrequencies()
        {
            if(frequencies != null)
            {
                foreach (var reminderTime in frequencies)
                {
                    AddNewReminderTime(reminderTime);
                }
            }
        }

        private void AddNewReminderTime(DateTime selectedTime)
        {
            var timeText = selectedTime.GetTimeString();
            var view = Activity.LayoutInflater.Inflate(Resource.Layout.template_medicine_alarmview_time, null);
            var layoutParams = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
            layoutParams.BottomMargin = 10.ConvertToPixel(Activity);
            view.LayoutParameters = layoutParams;

            view.FindViewById<TextView>(Resource.Id.reminderTime).Text = timeText;
            var deleteButton = view.FindViewById<ImageView>(Resource.Id.deleteTime);
            view.Tag = timeText;
            deleteButton.Tag = view;
            deleteButton.Click += Time_Deleted;

            selectedTimesContainer.AddView(view);

            containerScrollView.SmoothScrollBy(0, 50.ConvertToPixel(Activity));
        }

        private void LoadSelectedMedicineView()
        {
            selectedMedicineContainer.RemoveAllViews();
            if(medicineList != null)
            {
                foreach (var medicine in medicineList)
                {
                    AddMedicineToSelectedMedicineList(medicine);
                }
            }
        }

        private async Task DeleteReminder()
        {
            ShowLoader();
            var response = await Presenter.RemoveReminder(Medicine);
            HideLoader();
            if (response.IsSuccess)
            {
                Presenter.GoBack();
            }
            else
            {
                ProcessAPIError(response);
            }
        }

        private async Task Save()
        {
            ShowLoader();
            var response = await Presenter.AddReminder(Medicine, SelectedDays, frequencies, DateTime.Now, null, medicineList.ToList());
            HideLoader();
            if (response.IsSuccess)
            {
                Presenter.GoBackToHome();
            }
            else
            {
                ProcessAPIError(response);
            }
        }

        private void AddMedicineToSelectedMedicineList(MedicineInfo medicine)
        {
            var view = Activity.LayoutInflater.Inflate(Resource.Layout.template_medicine_alarmview_selected_medicine, null);

            var textView = view.FindViewById<TextView>(Resource.Id.medicineName);
            var deleteMedicine = view.FindViewById<ImageView>(Resource.Id.deleteMedicine);

            var strengthText = $", {medicine.Strength}";
            var totalText = medicine.Name + strengthText;
            var formattedText = new SpannableStringBuilder(totalText);

            formattedText.SetSpan(new StyleSpan(TypefaceStyle.Bold), 0, medicine.Name.Length, SpanTypes.InclusiveInclusive);
            formattedText.SetSpan(new AbsoluteSizeSpan(18.ConvertToPixel(Activity)), 0, medicine.Name.Length, SpanTypes.InclusiveInclusive);

            formattedText.SetSpan(new StyleSpan(TypefaceStyle.Normal), medicine.Name.Length, totalText.Length, SpanTypes.InclusiveInclusive);
            formattedText.SetSpan(new AbsoluteSizeSpan(14.ConvertToPixel(Activity)), medicine.Name.Length, totalText.Length, SpanTypes.InclusiveInclusive);

            textView.TextFormatted = formattedText;

            deleteMedicine.Tag = view;
            view.Tag = medicine.Id;

            deleteMedicine.Click += DeleteMedicine_Click;

            selectedMedicineContainer.AddView(view);
        }

        private void LoadAlarmView()
        {
            for (int index = 1; index <= 7; index++)
            {
                DayOfWeek day = index != 7 ? (DayOfWeek)index : DayOfWeek.Sunday;
                String localizedDay = $"medicine_reminder_{day.ToString().ToLower()}".Translate(Activity);

                var view = Activity.LayoutInflater.Inflate(Resource.Layout.template_weekview, null);
                var selectionBox = view.FindViewById<ImageView>(Resource.Id.selectionBox);
                var parentLayoutParams = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
                parentLayoutParams.Gravity = GravityFlags.CenterVertical;
                view.LayoutParameters = parentLayoutParams;

                view.Tag = (int)day;
                view.FindViewById<TextView>(Resource.Id.dayLabel).Text = localizedDay;

                view.Click += WeekView_Click;

                if (SelectedDays != null && SelectedDays.Count > 0)
                {
                    selectionBox.Selected = SelectedDays.Contains(day);
                }

                weekViewContainer.AddView(view);
                if (index != 7)
                {
                    var space = new Space(Activity);
                    space.LayoutParameters = new LinearLayout.LayoutParams(0, 1, 1);
                    weekViewContainer.AddView(space);
                }
            }
        }

        private void Validate()
        {
            bool isValid = (frequencies != null && frequencies.Count > 0 && SelectedDays != null && SelectedDays.Count > 0) || (Medicine.HasReminder && !AlarmToggleStatus);
            saveAlarmButton.Enabled = isValid;
        }

    }

    // https://github.com/xamarin/monodroid-samples/blob/master/UserInterface/TimePickerDemo/TimePickerDemo/MainActivity.cs
    // TimePicker dialog fragment
    public class TimePickerFragment : Android.Support.V4.App.DialogFragment, TimePickerDialog.IOnTimeSetListener
    {
        // TAG used for logging
        public static readonly string TAG = "TimePickerFragment";

        // Initialize handler to an empty delegate to prevent null reference exceptions:
        Action<DateTime> timeSelectedHandler = delegate { };

        // Factory method used to create a new TimePickerFragment:
        public static TimePickerFragment NewInstance(Action<DateTime> onTimeSelected)
        {
            // Instantiate a new TimePickerFragment:
            TimePickerFragment frag = new TimePickerFragment();

            // Set its event handler to the passed-in delegate:
            frag.timeSelectedHandler = onTimeSelected;

            // Return the new TimePickerFragment:
            return frag;
        }

        // Create and return a TimePickerDemo:
        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            // Get the current time
            DateTime currentTime = DateTime.Now;

            // Instantiate a new TimePickerDemo, passing in the handler, the current 
            // time to display, and whether or not to use 24 hour format:
            TimePickerDialog dialog = new TimePickerDialog
                (Activity, this, currentTime.Hour, currentTime.Minute, true);

            // Return the created TimePickerDemo:
            return dialog;
        }

        // Called when the user sets the time in the TimePicker: 
        public void OnTimeSet(TimePicker view, int hourOfDay, int minute)
        {
            // Get the current time:
            DateTime currentTime = DateTime.Now;

            // Create a DateTime that contains today's date and the time selected by the user:
            DateTime selectedTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, hourOfDay, minute, 0);

            // Invoke the handler to update the Activity's time display to the selected time:
            timeSelectedHandler(selectedTime);
        }
    }
}
