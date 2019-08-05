using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Constraints;
using Android.Util;
using Android.Views;
using Android.Widget;
using Helseboka.Droid.Common.Views;
using Helseboka.Droid.Startup;
using Helseboka.Droid.Startup.Views;
using Helseboka.Core.Common.Extension;
using Helseboka.Droid.Common.Utils;
using Helseboka.Core.Dashboard.Interface;
using Helseboka.Core.Common.EnumDefinitions;
using System.Threading.Tasks;
using Helseboka.Core.Common.Model;
using Android.Support.V7.Widget;
using Helseboka.Droid.Common.Adapters;
using Helseboka.Core.MedicineModule.Model;
using Helseboka.Core.Resources.StringResources;
using Android.Graphics;
using Helseboka.Core.AppointmentModule.Model;

namespace Helseboka.Droid.Dashboard.Views
{
    public class DashboardFragment : BaseFragment, IUniversalAdapter
    {
        ImageView leftArrow;
        ImageView rightArrow;
        TextView headerDay;
        TextView headerDate;
        TextView pageTitle;
        TextView pageSubTitle;
        TextView appointmentDate;
        TextView appointmentMessage;
        TextView appointmentTitle;
        View noDataContainer;
        View noDataCenterView;
        View appointmentDetailsContainer;
        View reminderDataViewContainer;
        RecyclerView dataListView;
        ProgressBar reminderLoadingProgressBar;
        ProgressBar appointmentLoadingProgressBar;
        ImageView videoCallButton;

        private AppointmentDetails appointment;
        private LinearSnapHelper snapHelper;
        private LinearLayoutManager layoutManager;
        private List<AlarmDetails> alarmList;
        private DateTime selectedDate = DateTime.Now;
        private IDashboardPresenter Presenter
        {
            get => presenter as IDashboardPresenter;
        }

        public DashboardFragment(IDashboardPresenter presenter)
        {
            this.presenter = presenter;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_dashboard, null);

            leftArrow = view.FindViewById<ImageView>(Resource.Id.leftArrow);
            rightArrow = view.FindViewById<ImageView>(Resource.Id.rightArrow);
            headerDay = view.FindViewById<TextView>(Resource.Id.headerDay);
            headerDate = view.FindViewById<TextView>(Resource.Id.headerDate);
            pageTitle = view.FindViewById<TextView>(Resource.Id.pageTitle);
            pageSubTitle = view.FindViewById<TextView>(Resource.Id.pageSubTitle);
            appointmentDate = view.FindViewById<TextView>(Resource.Id.appointmentDate);
            appointmentMessage = view.FindViewById<TextView>(Resource.Id.appointmentMessage);
            appointmentTitle = view.FindViewById<TextView>(Resource.Id.appointmentTitle);
            noDataContainer = view.FindViewById(Resource.Id.noDataContainer);
            noDataCenterView = view.FindViewById(Resource.Id.noDataCenterView);
            appointmentDetailsContainer = view.FindViewById(Resource.Id.appointmentDetailsContainer);
            reminderDataViewContainer = view.FindViewById(Resource.Id.reminderDataViewContainer);
            dataListView = view.FindViewById<RecyclerView>(Resource.Id.dataListView);
            reminderLoadingProgressBar = view.FindViewById<ProgressBar>(Resource.Id.reminderLoadingProgressBar);
            appointmentLoadingProgressBar = view.FindViewById<ProgressBar>(Resource.Id.appointmentLoadingProgressBar);
            videoCallButton = view.FindViewById<ImageView>(Resource.Id.videoCallButton);

            layoutManager = new LinearLayoutManager(Activity, LinearLayoutManager.Horizontal, false);
            dataListView.SetLayoutManager(layoutManager);
            dataListView.SetAdapter(new GenericRecyclerAdapter(this));
            snapHelper = new LinearSnapHelper();
            snapHelper.AttachToRecyclerView(dataListView);
            dataListView.AddItemDecoration(new SimpleItemDecoration(Activity));

            pageTitle.Text = $"{AppResources.Salutation} {ApplicationCore.Instance.CurrentUser.FirstName.ToNameCase()}";

            leftArrow.Click += LeftArrow_Click;
            rightArrow.Click += RightArrow_Click;
            videoCallButton.Click += VideoCallButton_Tapped;
            ShowAlarmLoader();
            ShowAppointmentLoader();

            return view;
        }

        public override void OnResume()
        {
            base.OnResume();

            UpdateViewAccordingToDate();
            LoadAppointments().Forget();
        }

        void LeftArrow_Click(object sender, EventArgs e)
        {
            selectedDate = selectedDate.AddDays(-1);
            UpdateViewAccordingToDate();
        }

        void RightArrow_Click(object sender, EventArgs e)
        {
            selectedDate = selectedDate.AddDays(1);
            UpdateViewAccordingToDate();
        }


        public void UpdateViewAccordingToDate()
        {
            var dayPrefix = "";
            var day = selectedDate.GetDay();
            switch (day)
            {
                case Day.Today: dayPrefix = Resources.GetString(Resource.String.today); break;
                case Day.Tomorrow: dayPrefix = Resources.GetString(Resource.String.tomorrow); break;
                case Day.Yesterday: dayPrefix = Resources.GetString(Resource.String.yesterday); break;
                default: break;
            }

            if (String.IsNullOrEmpty(dayPrefix))
            {
                headerDay.Visibility = ViewStates.Gone;
            }
            else
            {
                headerDay.Text = dayPrefix;
                headerDay.Visibility = ViewStates.Visible;
            }

            headerDate.Text = selectedDate.ToString("dd.MM.yyyy");
            LoadAlarms().Forget();
        }

        public async Task LoadAppointments()
        {
            appointment = await Presenter.GetNextAppointmentDetails();
            if (appointment != null && appointment.AppointmentTime.HasValue)
            {
                var dateFormat = AppResources.AppLongDateTimeFormat;
                appointmentDate.Text = appointment.AppointmentTime.Value.ToString(dateFormat);
                appointmentTitle.Text = $"{Resources.GetString(Resource.String.dashboard_appointment_title)}\n{appointment.Doctor.FullName}";

                if (!String.IsNullOrEmpty(appointment.DoctorFocusedReply))
                {
                    appointmentMessage.Visibility = ViewStates.Visible;
                    appointmentMessage.Text = appointment.DoctorFocusedReply;
                }
                else
                {
                    appointmentMessage.Visibility = ViewStates.Invisible;
                }

                if(!string.IsNullOrEmpty(appointment.Doctor.VideoUrl) && appointment.IsVideoConsultationConfirmed)
                {
                    videoCallButton.Visibility = ViewStates.Visible;
                }

                ShowAppointmentView();
            }
            else
            {
                ShowNoAppointmentView();
            }
        }

        void VideoCallButton_Tapped(object sender, EventArgs e)
        {
            if(appointment != null)
            {
               NavigateToBrowser(appointment.Doctor.VideoUrl);
            }
        }

        public async Task LoadAlarms()
        {
            alarmList = await Presenter.GetAlarms(selectedDate);

            if (alarmList != null && alarmList.Count > 0)
            {
                dataListView.GetAdapter().NotifyDataSetChanged();

                ShowAlarmView();

                var nextAlarm = alarmList.Where(x => x.IsNextAlarm).FirstOrDefault();
                if (nextAlarm != null)
                {
                    var nextIndex = alarmList.IndexOf(nextAlarm);
                    dataListView.ScrollToPosition(nextIndex);
                    await Task.Delay(TimeSpan.FromSeconds(0.2));

                    View view = layoutManager.FindViewByPosition(nextIndex);
                    if (view != null)
                    {
                        var snapDistance = snapHelper.CalculateDistanceToFinalSnap(layoutManager, view);
                        if (snapDistance != null && (snapDistance[0] != 0 || snapDistance[1] != 0))
                        {
                            dataListView.SmoothScrollBy(snapDistance[0], snapDistance[1]);
                        }
                    }
                }
                else
                {
                    dataListView.ScrollToPosition(0);
                }
            }
            else
            {
                ShowNoAlarmView();
            }
        }

        private void ShowNoAlarmView()
        {
            reminderLoadingProgressBar.Visibility = ViewStates.Gone;
            reminderDataViewContainer.Visibility = ViewStates.Gone;
            noDataContainer.Visibility = ViewStates.Visible;

            noDataCenterView.Click -= NoDataCenterView_Click;
            noDataCenterView.Click += NoDataCenterView_Click;
        }

        private void ShowAlarmView()
        {
            reminderLoadingProgressBar.Visibility = ViewStates.Gone;
            reminderDataViewContainer.Visibility = ViewStates.Visible;
            noDataContainer.Visibility = ViewStates.Gone;

            noDataCenterView.Click -= NoDataCenterView_Click;
        }
        private void ShowNoAppointmentView()
        {
            appointmentLoadingProgressBar.Visibility = ViewStates.Gone;
            appointmentDate.Visibility = ViewStates.Gone;
            appointmentMessage.Visibility = ViewStates.Gone;
            appointmentTitle.Visibility = ViewStates.Visible;

            appointmentTitle.Text = Resources.GetString(Resource.String.dashboard_appointment_nodata);

            appointmentDetailsContainer.Click -= AppointmentDetailsContainer_Click;
            appointmentDetailsContainer.Click += AppointmentDetailsContainer_Click;
        }
        private void ShowAppointmentView()
        {
            appointmentLoadingProgressBar.Visibility = ViewStates.Gone;
            appointmentDate.Visibility = ViewStates.Visible;
            appointmentMessage.Visibility = ViewStates.Visible;
            appointmentTitle.Visibility = ViewStates.Visible;

            appointmentDetailsContainer.Click -= AppointmentDetailsContainer_Click;
        }

        private void ShowAlarmLoader()
        {
            reminderLoadingProgressBar.Visibility = ViewStates.Visible;
            reminderDataViewContainer.Visibility = ViewStates.Gone;
            noDataContainer.Visibility = ViewStates.Gone;
        }

        private void ShowAppointmentLoader()
        {
            appointmentLoadingProgressBar.Visibility = ViewStates.Visible;
            appointmentDate.Visibility = ViewStates.Gone;
            appointmentMessage.Visibility = ViewStates.Gone;
            appointmentTitle.Visibility = ViewStates.Gone;
        }

        void NoDataCenterView_Click(object sender, EventArgs e)
        {
            ShowInfoDialog(AppResources.AppointmentHomeNoAlarmPopupMessage);
        }


        void AppointmentDetailsContainer_Click(object sender, EventArgs e)
        {
            ShowInfoDialog(AppResources.AppointmentHomeNoAppointmentPopupMessage);
        }


        public int GetItemCount()
        {
            return alarmList != null ? alarmList.Count : 0;
        }

        public UniversalViewHolder CreateItemView(ViewGroup parent, int viewType)
        {
            var inflater = LayoutInflater.FromContext(Activity);
            var view = inflater.Inflate(Resource.Layout.template_dashboard_alarm, null);

            var viewMap = new Dictionary<int, View>();
            viewMap.Add(Resource.Id.backgroundImage, view.FindViewById(Resource.Id.backgroundImage));
            viewMap.Add(Resource.Id.reminderTime, view.FindViewById(Resource.Id.reminderTime));
            viewMap.Add(Resource.Id.selectBox, view.FindViewById(Resource.Id.selectBox));
            viewMap.Add(Resource.Id.medicineName, view.FindViewById(Resource.Id.medicineName));
            viewMap.Add(Resource.Id.medicineDetails, view.FindViewById(Resource.Id.medicineDetails));
            viewMap.Add(Resource.Id.overlayView, view.FindViewById(Resource.Id.overlayView));

            return new UniversalViewHolder(view, viewMap);
        }

        public void BindViewHolder(UniversalViewHolder holder, int position)
        {
            if (alarmList != null && position < alarmList.Count)
            {
                var backgroundImage = holder.GetView<ImageView>(Resource.Id.backgroundImage);
                var reminderTime = holder.GetView<TextView>(Resource.Id.reminderTime);
                var selectBox = holder.GetView<ImageView>(Resource.Id.selectBox);
                var medicineName = holder.GetView<TextView>(Resource.Id.medicineName);
                var medicineDetails = holder.GetView<TextView>(Resource.Id.medicineDetails);
                var overlayView = holder.GetView<View>(Resource.Id.overlayView);

                var AlarmDetails = alarmList[position];

                reminderTime.Text = AlarmDetails.Time.GetTimeString();
                medicineName.Text = $"{AlarmDetails.Medicine.Name},";
                medicineDetails.Text = AlarmDetails.Medicine.Strength;
                selectBox.Selected = AlarmDetails.Status == AlarmStatus.Completed;

                if (AlarmDetails.Status == AlarmStatus.Completed)
                {
                    overlayView.Visibility = ViewStates.Visible;
                    selectBox.Enabled = false;
                    selectBox.Tag = -1;
                    selectBox.Click -= SelectBox_Click;
                }
                else
                {
                    overlayView.Visibility = ViewStates.Gone;
                    selectBox.Enabled = true;
                    selectBox.Tag = position;
                    selectBox.Click -= SelectBox_Click;
                    selectBox.Click += SelectBox_Click;
                }

                if (AlarmDetails.IsNextAlarm)
                {
                    backgroundImage.SetImageResource(Resource.Drawable.dashboard_reminder_next);
                }
                else
                {
                }
                backgroundImage.SetImageResource(Resource.Drawable.dashboard_reminder_background);
            }
        }

        void SelectBox_Click(object sender, EventArgs e)
        {
            if (sender is View selectionBox)
            {
                selectionBox.Selected = !selectionBox.Selected;
                int index = (int)selectionBox.Tag;
                if (alarmList != null && index < alarmList.Count)
                {
                    var alarm = alarmList[index];

                    var title = Resources.GetString(Resource.String.dashboard_alarm_confirmation_title);
                    var message = Resources.GetString(Resource.String.dashboard_alarm_confirmation_message);
                    var confirmButtonText = Resources.GetString(Resource.String.dashboard_alarm_confirmation_yes);

                    var dialog = new YesNoDialog(Activity, message, title, async () =>
                    {
                        var response = await Presenter.MarkAlarmAsComplete(alarm);
                        if (response.IsSuccess)
                        {
                            dataListView.GetAdapter().NotifyItemChanged(index);
                        }
                    }, confirmButtonText, onDialogClose: () =>
                     {
                         selectionBox.Selected = false;
                     });

                    dialog.Show();

                }
            }
        }

        public void OnItemClick(int position)
        {

        }
    }

    public class SimpleItemDecoration : RecyclerView.ItemDecoration
    {
        Context _context;
        public SimpleItemDecoration(Context context)
        {
            _context = context;
        }
        public override void GetItemOffsets(Rect outRect, View view, RecyclerView parent, RecyclerView.State state)
        {
            if(parent.GetAdapter().ItemCount == 1)
            {
                int totalWidth = parent.Width;
                int cardWidth = 206.ConvertToPixel(_context);
                int sidePadding = (totalWidth - cardWidth) / 2;
                sidePadding = Math.Max(0, sidePadding);
                outRect.Set(sidePadding, 0, sidePadding, 0);
            }
        }

    }
}
