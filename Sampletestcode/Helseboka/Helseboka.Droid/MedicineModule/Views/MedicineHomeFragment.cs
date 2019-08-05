
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using Helseboka.Core.Legedialog.Model;
using Helseboka.Core.MedicineModule.Interface;
using Helseboka.Core.MedicineModule.Model;
using Helseboka.Droid.Common.Adapters;
using Helseboka.Droid.Common.Views;
using Helseboka.Core.Common.Extension;
using Helseboka.Droid.Common.Utils;
using Android.Graphics;
using Android.Support.V4.Content;
using Android.Text;
using Android.Text.Style;
using Helseboka.Droid.Common.Interfaces;
using Helseboka.Core.Common.EnumDefinitions;
using Helseboka.Core.Common.Constant;
using Helseboka.Core.Common.Model;
using Android.Support.V4.Content.Res;
using Helseboka.Core.Resources.StringResources;

namespace Helseboka.Droid.MedicineModule.Views
{
    public class MedicineHomeFragment : BaseFragment, IUniversalAdapter
    {
        View helpFAQview;
        LinearLayout helpFAQContainer;
        ImageView helpButton;
        Button renewPrescription;
        ProgressBar progressBar;
        RecyclerView dataListView;
        Boolean isLoading;
        List<MedicineReminder> medicineDataList;
        SwipeRefreshLayout refreshView;
        EditText searchText;
        RelativeLayout searchContainer;
        LinearLayout searchMedicineResultContainer;
        RecyclerView searchResultDataListView;
        private HashSet<MedicineInfo> selectedMedicine = new HashSet<MedicineInfo>();
        private bool isSelecting = false;
        private System.Timers.Timer timer = new System.Timers.Timer(AppConstant.SearchDelay);

        private IMedicineHomePresenter Presenter
        {
            get => presenter as IMedicineHomePresenter;
        }

        public MedicineHomeFragment(IMedicineHomePresenter presenter)
        {
            this.presenter = presenter;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_medicine_home, null);

            helpFAQview = view.FindViewById<ScrollView>(Resource.Id.helpFAQview);
            helpFAQContainer = view.FindViewById<LinearLayout>(Resource.Id.helpFAQviewContainer);
            helpButton = view.FindViewById<ImageView>(Resource.Id.helpButton);
            renewPrescription = view.FindViewById<Button>(Resource.Id.renewPrescriptionButton);
            progressBar = view.FindViewById<ProgressBar>(Resource.Id.loading_progressbar);
            dataListView = view.FindViewById<RecyclerView>(Resource.Id.dataListView);
            refreshView = view.FindViewById<SwipeRefreshLayout>(Resource.Id.refreshView);
            searchText = view.FindViewById<EditText>(Resource.Id.searchText);
            searchContainer = view.FindViewById<RelativeLayout>(Resource.Id.searchContainer);
            searchMedicineResultContainer = view.FindViewById<LinearLayout>(Resource.Id.searchMedicineResultContainer);
            searchResultDataListView = view.FindViewById<RecyclerView>(Resource.Id.searchResultDataListView);

            helpButton.Click += HelpButton_Click;
            renewPrescription.Click += RenewPrescription_Click;
            refreshView.Refresh += RefreshView_Refresh;
            searchText.FocusChange += SearchText_FocusChange;
            searchText.TextChanged += SearchText_TextChanged;
            searchText.KeyPress += SearchText_KeyPress;
            timer.Elapsed += Timer_Elapsed;
            timer.Stop();

            DesignHelpView(helpFAQContainer, Core.Common.EnumDefinitions.HelpFAQType.MedicineHome).Forget();

            var layoutManager = new LinearLayoutManager(Activity, LinearLayoutManager.Vertical, false);
            dataListView.SetLayoutManager(layoutManager);
            dataListView.SetAdapter(new GenericRecyclerAdapter(this));

            //Activity.Window.SetSoftInputMode(SoftInput.AdjustNothing);
            SetEditTextStyle();

            if(Activity is IActivity mainActivity)
            {
                mainActivity.AttachKeyboardListner();
                mainActivity.KeyboardHide -= MainActivity_KeyboardHide;
                mainActivity.KeyboardVisible -= MainActivity_KeyboardVisible;
                mainActivity.KeyboardHide += MainActivity_KeyboardHide;
                mainActivity.KeyboardVisible += MainActivity_KeyboardVisible;
            }
            return view;
        }

        //public void ResetView()
        //{
        //    searchText.Text = String.Empty;
        //    HideKeyboard(searchText);
        //    ShowTableLoader();
        //    RefreshData().Forget();
        //}

        public override void OnDestroyView()
        {
            base.OnDestroyView();

            //Activity.Window.SetSoftInputMode(SoftInput.AdjustResize);
            if (Activity is IActivity mainActivity)
            {
                mainActivity.RemoveKeyboardListner();
                mainActivity.KeyboardHide -= MainActivity_KeyboardHide;
                mainActivity.KeyboardVisible -= MainActivity_KeyboardVisible;
            }
        }

        public override void OnResume()
        {
            base.OnResume();
            ShowTableLoader();
            RefreshData().Forget();
        }

        void MainActivity_KeyboardHide(object sender, EventArgs e)
        {
            if (Activity is IActivity mainActivity)
            {
                mainActivity.ShowToolbar();
                renewPrescription.Visibility = ViewStates.Visible;
            }
        }

        void MainActivity_KeyboardVisible(object sender, int e)
        {
            if (Activity is IActivity mainActivity)
            {
                mainActivity.HideToolbar();
                renewPrescription.Visibility = ViewStates.Gone;
            }
        }

        void SearchText_KeyPress(object sender, View.KeyEventArgs e)
        {
            if (e.KeyCode == Keycode.Enter)
            {
                if (medicineDataList != null && medicineDataList.Count > 0)
                {
                    ShowDataTable();
                }
                else
                {
                    ShowNoData();
                }
                searchText.Text = String.Empty;
                HideKeyboard(searchText);
                e.Handled = true;
            }

            e.Handled = false;
        }


        void SearchText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(searchText.Text))
            {
                timer.Stop();
                timer.Start();
            }
            else
            {
                timer.Stop();

                if (medicineDataList != null && medicineDataList.Count > 0)
                {
                    ShowDataTable();
                }
                else
                {
                    ShowNoData();
                }
            }
        }

        void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            timer.Stop();

            Activity.RunOnUiThread(async () =>
            {
                var response = await Presenter.SearchMedicine(searchText.Text);
                if (response.IsSuccess)
                {
                    var adapter = new SearchMedicineAdapter(Activity, response.Result);
                    adapter.MedicineTapped += Adapter_MedicineTapped;
                    searchResultDataListView.SetAdapter(new GenericRecyclerAdapter(adapter));
                    searchResultDataListView.SetLayoutManager(new LinearLayoutManager(Activity, LinearLayoutManager.Vertical, false));
                    ShowMedicineSearchView();
                }
            });
        }

        void Adapter_MedicineTapped(object sender, MedicineInfo e)
        {
            Presenter.SelectMedicineFromSearchResult(e);
        }


        void SearchText_FocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (e.HasFocus)
            {
                searchContainer.SetBackgroundResource(Resource.Drawable.shape_selectable_active_background);
            }
            else
            {
                searchContainer.SetBackgroundResource(Resource.Drawable.shape_selectable_nodata_background);
            }
        }


        void RefreshView_Refresh(object sender, EventArgs e)
        {
            RefreshData().Forget();
        }

        void HelpButton_Click(object sender, EventArgs e)
        {
            HideKeyboard(searchText);
            ShowHelpView(Core.Common.EnumDefinitions.HelpFAQType.MedicineHome).Forget();
        }


        private void RenewPrescription_Click(object sender, EventArgs e)
        {
            if (ApplicationCore.Instance.CurrentUser != null && ApplicationCore.Instance.CurrentUser.AssignedDoctor != null && !ApplicationCore.Instance.CurrentUser.AssignedDoctor.Enabled)
            {
                var title = AppResources.BasicInfoTitle;
                ShowInfoDialog(ApplicationCore.Instance.CurrentUser.AssignedDoctor.Remarks, title);
            }
            else
            {
                if (isSelecting)
                {
                    if (selectedMedicine.Count > 0)
                    {
                        var dialog = new RenewPrescriptionDialog(Activity, selectedMedicine.ToList());
                        dialog.SendTapped += Dialog_SendTapped;
                        dialog.Show();
                    }

                    renewPrescription.Text = Resources.GetString(Resource.String.medicine_home_button_renewprescription);
                }
                else
                {
                    selectedMedicine.Clear();
                    renewPrescription.Text = Resources.GetString(Resource.String.general_view_cancel);
                }

                isSelecting = !isSelecting;
                dataListView.GetAdapter().NotifyDataSetChanged();
            }
        }

        void Dialog_SendTapped(object sender, EventArgs e)
        {
            RenewPrescription().Forget();
        }

        private async Task RenewPrescription()
        {
            var response = await Presenter.RenewPrescription(selectedMedicine.ToList());
            selectedMedicine.Clear();
        }

        private void ShowNoData()
        {
            helpButton.Visibility = ViewStates.Invisible;
            progressBar.Visibility = ViewStates.Gone;
            refreshView.Visibility = ViewStates.Gone;
            helpFAQview.Visibility = ViewStates.Visible;
            searchMedicineResultContainer.Visibility = ViewStates.Gone;
        }

        private void ShowDataTable()
        {
            helpButton.Visibility = ViewStates.Visible;
            progressBar.Visibility = ViewStates.Gone;
            refreshView.Visibility = ViewStates.Visible;
            helpFAQview.Visibility = ViewStates.Gone;
            searchMedicineResultContainer.Visibility = ViewStates.Gone;
        }

        private void ShowTableLoader()
        {
            helpButton.Visibility = ViewStates.Visible;
            progressBar.Visibility = ViewStates.Visible;
            refreshView.Visibility = ViewStates.Gone;
            helpFAQview.Visibility = ViewStates.Gone;
            searchMedicineResultContainer.Visibility = ViewStates.Gone;
        }

        private void ShowMedicineSearchView()
        {
            helpButton.Visibility = ViewStates.Visible;
            progressBar.Visibility = ViewStates.Gone;
            refreshView.Visibility = ViewStates.Gone;
            helpFAQview.Visibility = ViewStates.Gone;
            searchMedicineResultContainer.Visibility = ViewStates.Visible;
            searchMedicineResultContainer.TranslationZ = 10;
        }

        private void SetEditTextStyle()
        {
            var placeholder = Resources.GetString(Resource.String.medicine_home_search_hint);
            SpannableStringBuilder formattedHint = new SpannableStringBuilder(placeholder);
            var placeholderFont = Typeface.CreateFromAsset(Activity.Assets, "fonts/avenir_next_lt_pro_demi.otf");
            formattedHint.SetSpan(new StyleSpan(TypefaceStyle.Normal), 0, placeholder.Length, SpanTypes.InclusiveInclusive);
            formattedHint.SetSpan(new CustomTypefaceSpan("", placeholderFont), 0, placeholder.Length, SpanTypes.InclusiveInclusive);
            formattedHint.SetSpan(new AbsoluteSizeSpan(16.ConvertToPixel(Activity)), 0, placeholder.Length, SpanTypes.InclusiveInclusive);
            formattedHint.SetSpan(new ForegroundColorSpan(new Color(ContextCompat.GetColor(Activity, Resource.Color.startup_subtitle))), 0, placeholder.Length, 0);
            searchText.HintFormatted = formattedHint;
        }

        private async Task RefreshData()
        {
            isLoading = true;
            var response = await Presenter.GetAllMedicineAndReminders();
            refreshView.Refreshing = false;
            isLoading = false;
            if (response.IsSuccess)
            {
                if(response.Result != null && response.Result.Count > 0)
                {
                    medicineDataList = response.Result;
                    dataListView.GetAdapter().NotifyDataSetChanged();
                    renewPrescription.Enabled = true;
                    ShowDataTable();
                }
                else
                {
                    ShowNoData();
                }
            }
            else
            {
                ShowNoData();
            }
        }

        public int GetItemCount()
        {
            return medicineDataList != null ? medicineDataList.Count : 0;
        }

        public UniversalViewHolder CreateItemView(ViewGroup parent, int viewType)
        {
            var inflater = LayoutInflater.FromContext(Activity);
            var view = inflater.Inflate(Resource.Layout.template_medicine_home, null);

            var viewMap = new Dictionary<int, View>();
            viewMap.Add(Resource.Id.medicineName, view.FindViewById(Resource.Id.medicineName));
            viewMap.Add(Resource.Id.medicineStrength, view.FindViewById(Resource.Id.medicineStrength));
            viewMap.Add(Resource.Id.selectionBox, view.FindViewById(Resource.Id.selectionBox));
            viewMap.Add(Resource.Id.medicineContainer, view.FindViewById(Resource.Id.medicineContainer));
            viewMap.Add(Resource.Id.reminderContainer, view.FindViewById(Resource.Id.reminderContainer));
            viewMap.Add(Resource.Id.reminderDetails, view.FindViewById(Resource.Id.reminderDetails));

            return new UniversalViewHolder(view, viewMap);
        }

        public void BindViewHolder(UniversalViewHolder holder, int position)
        {
            var data = medicineDataList[position];

            var selectionBox = holder.GetView<ImageView>(Resource.Id.selectionBox);
            var container = holder.GetView<RelativeLayout>(Resource.Id.medicineContainer);
            var reminderContainer = holder.GetView<LinearLayout>(Resource.Id.reminderContainer);
            var reminderDetails = holder.GetView<TextView>(Resource.Id.reminderDetails);

            holder.GetView<TextView>(Resource.Id.medicineName).Text = data.Medicine.Name;
            holder.GetView<TextView>(Resource.Id.medicineStrength).Text = $" {data.Medicine.Strength}";

            if (isSelecting)
            {
                selectionBox.Visibility = ViewStates.Visible;
                selectionBox.Selected = selectedMedicine.Contains(data.Medicine);
                container.Selected = selectionBox.Selected;

                selectionBox.Tag = holder;

                selectionBox.Click -= SelectionBox_Click;
                selectionBox.Click += SelectionBox_Click;
            }
            else
            {
                selectionBox.Visibility = ViewStates.Gone;
                container.Selected = false;
            }

            var reminder = data.Reminder;

            if (reminder != null)
            {
                List<String> frequency = new List<string>();
                var reminderPrefix = Resources.GetString(Resource.String.general_view_next);

                String dayText;
                if (reminder.NextReminderDay.GetDay() == Day.Today)
                {
                    dayText = Resources.GetString(Resource.String.today);
                    frequency = reminder.GetNextFrequencies();
                }
                else if (reminder.NextReminderDay.GetDay() == Day.Tomorrow)
                {
                    dayText = Resources.GetString(Resource.String.tomorrow);
                }
                else
                {
                    dayText = reminder.NextReminderDay.ToString().ToLower().Translate(Activity);
                }

                var timePrefix = Resources.GetString(Resource.String.general_view_timeprefix);

                if (frequency.Count < 1)
                {
                    frequency = reminder.FrequencyPerDay;
                }

                var alarmText = String.Join(", ", frequency);

                var finalDayText = $"{reminderPrefix} {dayText}";
                var finalAlarmText = $" {timePrefix} {alarmText}";

                var formattedText = new SpannableStringBuilder(finalDayText + finalAlarmText);
                formattedText.SetSpan(new ForegroundColorSpan(new Color(ContextCompat.GetColor(Activity, Resource.Color.medicine_home_reminder_day_color))), 0, finalDayText.Length, SpanTypes.InclusiveInclusive);
                reminderDetails.TextFormatted = formattedText;
                reminderContainer.Visibility = ViewStates.Visible;
            }
            else
            {
                reminderContainer.Visibility = ViewStates.Invisible;
            }
        }

        public void OnItemClick(int position)
        {
            if(isSelecting)
            {
                ToggleMedicineSelect(position);
            }
            else
            {
                if (medicineDataList != null && position < medicineDataList.Count)
                {
                    Presenter.NavigateToMedicineOverview(medicineDataList[position]);
                }
            }
        }

        void SelectionBox_Click(object sender, EventArgs e)
        {
            var selectionBox = sender as ImageView;
            var viewHolder = selectionBox.Tag as UniversalViewHolder;
            ToggleMedicineSelect(viewHolder.AdapterPosition);
        }

        private void ToggleMedicineSelect(int position)
        {
            var viewHolder = dataListView.FindViewHolderForAdapterPosition(position) as UniversalViewHolder;
            var selectionBox = viewHolder.GetView<ImageView>(Resource.Id.selectionBox);
            var container = viewHolder.GetView<RelativeLayout>(Resource.Id.medicineContainer);

            selectionBox.Selected = !selectionBox.Selected;
            var data = medicineDataList[viewHolder.AdapterPosition];
            container.Selected = selectionBox.Selected;
            if (selectionBox.Selected)
            {
                selectedMedicine.Add(data.Medicine);
            }
            else
            {
                selectedMedicine.Remove(data.Medicine);
            }

            if(isSelecting)
            {
                renewPrescription.Text = selectedMedicine.Count > 0 ? Resources.GetString(Resource.String.medicine_home_button_renewprescription) : Resources.GetString(Resource.String.general_view_cancel);
            }
        }
    }

    public class SearchMedicineAdapter : IUniversalAdapter
    {
        private Context context;
        private List<MedicineInfo> medicines;

        public event EventHandler<MedicineInfo> MedicineTapped;

        public SearchMedicineAdapter(Context context, List<MedicineInfo> medicines)
        {
            this.context = context;
            this.medicines = medicines;
        }

        public void BindViewHolder(UniversalViewHolder holder, int position)
        {
            if (medicines != null && medicines.Count > position)
            {
                holder.GetView<TextView>(Resource.Id.searchResultLabel).Text = medicines[position].NameFormStrength;
            }
        }

        public UniversalViewHolder CreateItemView(ViewGroup parent, int viewType)
        {
            var inflater = LayoutInflater.FromContext(context);
            var view = inflater.Inflate(Resource.Layout.template_search_result, null);

            var viewMap = new Dictionary<int, View>();
            viewMap.Add(Resource.Id.searchResultLabel, view.FindViewById(Resource.Id.searchResultLabel));

            return new UniversalViewHolder(view, viewMap);
        }

        public int GetItemCount()
        {
            return medicines != null ? medicines.Count : 0;
        }

        public void OnItemClick(int position)
        {
            if(medicines != null && position < medicines.Count)
            {
                MedicineTapped?.Invoke(this, medicines[position]);
            }
        }
    }
}
