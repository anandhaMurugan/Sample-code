
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
using Helseboka.Core.AppointmentModule.Interface;
using Helseboka.Core.AppointmentModule.Model;
using Helseboka.Droid.Common.Adapters;
using Helseboka.Droid.Common.Listners;
using Helseboka.Droid.Common.Views;
using Helseboka.Droid.Common.Utils;
using Helseboka.Core.Common.Extension;
using Android.Support.Constraints;
using Helseboka.Core.Common.EnumDefinitions;
using Helseboka.Core.Common.Model;
using Helseboka.Core.Resources.StringResources;
using Android.Text;

namespace Helseboka.Droid.AppointmentModule.Views
{
    public class AppointmentHomeFragment : BaseFragment, IMultipleItemUniversalAdapter
    {
        private Button newAppointment;
        private Button videoLinkButton;
        private SwipeRefreshLayout dataListViewRefreshContainer;
        private RecyclerView dataListView;
        private ProgressBar loading_progressbar;
        private ScrollView helpFAQview;
        private TextView helpTextView;
        private Doctor doctorDetails;

        private AppointmentCollection Appointments { get; set; }
        private bool isLoading = false;

        private IAppointmentPresenter Presenter
        {
            get => presenter as IAppointmentPresenter;
        }

        public AppointmentHomeFragment(IAppointmentPresenter presenter)
        {
            this.presenter = presenter;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_appointment_home, null);
       
            newAppointment = view.FindViewById<Button>(Resource.Id.newAppointment);
            videoLinkButton = view.FindViewById<Button>(Resource.Id.videoLinkButton);
            CheckForDoctorVideoUrl();
            dataListViewRefreshContainer = view.FindViewById<SwipeRefreshLayout>(Resource.Id.dataListViewRefreshContainer);
            dataListView = view.FindViewById<RecyclerView>(Resource.Id.dataListView);
            loading_progressbar = view.FindViewById<ProgressBar>(Resource.Id.loading_progressbar);
            helpFAQview = view.FindViewById<ScrollView>(Resource.Id.helpFAQview);
            helpTextView = view.FindViewById<TextView>(Resource.Id.helpTextView);

            var layoutManager = new LinearLayoutManager(Activity, LinearLayoutManager.Vertical, false);
            dataListView.SetLayoutManager(layoutManager);
            dataListView.SetAdapter(new GenericRecyclerAdapter(this));
            var scrollListner = new RecyclerViewScrollListener(layoutManager);
            scrollListner.LoadMore += ScrollListner_LoadMore;
            dataListView.AddOnScrollListener(scrollListner);

            dataListViewRefreshContainer.Refresh += DataListViewRefreshContainer_Refresh;
            newAppointment.Click += NewAppointment_Click;
            videoLinkButton.Click += VideoConsultation_Tapped;
            helpTextView.Text = String.Format(AppResources.AppointmentHomeHelpText, System.Environment.NewLine);

            ShowProgressLoading();
            RefreshData().Forget();

            return view;
        }

        void NewAppointment_Click(object sender, EventArgs e)
        {
            CheckDoctorAndProceed(() =>
            {
                Presenter.ShowAppointmentDateSelectionView();
            });
        }

        private async Task RefreshData()
        {
            isLoading = true;
            Appointments = await Presenter.GetAllAppointments();
            dataListViewRefreshContainer.Refreshing = false;
            isLoading = false;
            if (Appointments != null && Appointments.Count > 0)
            {
                dataListView.GetAdapter().NotifyDataSetChanged();
                ShowAppointmentList();
            }
            else
            {
                ShowNoData();
            }
        }

        private async Task LoadMoreData()
        {
            if (Presenter.HasMoreData && !isLoading)
            {
                isLoading = true;
                var response = await Presenter.LoadMore(Appointments);
                dataListViewRefreshContainer.Refreshing = false;
                isLoading = false;
                if (response)
                {
                    dataListView.GetAdapter().NotifyDataSetChanged();
                }
            }
        }


        private void ShowNoData()
        {
            helpFAQview.Visibility = ViewStates.Visible;
            loading_progressbar.Visibility = ViewStates.Gone;
        }

        private void ShowAppointmentList()
        {
            helpFAQview.Visibility = ViewStates.Gone;
            loading_progressbar.Visibility = ViewStates.Gone;
        }

        private void ShowProgressLoading()
        {
            helpFAQview.Visibility = ViewStates.Gone;
            loading_progressbar.Visibility = ViewStates.Visible;
        }

        private bool IsIndexInUpcommingItems(int position)
        {
            return Appointments != null && Appointments.UpcommingAppointments != null && position < Appointments.UpcommingAppointments.Count;
        }

        private async Task CancelAppointment(AppointmentDetails appointment)
        {
            ShowLoader();
            var response = await Presenter.CancelAppointment(appointment);
            if (!response.IsSuccess)
            {
                ProcessAPIError(response);
            }

            await RefreshData();
            HideLoader();
        }

        public int GetItemCount()
        {
            int count = 0;

            if(Appointments != null)
            {
                if (Appointments.UpcommingAppointments != null)
                {
                    count += Appointments.UpcommingAppointments.Count;
                }

                if (Appointments.OtherAppointments != null)
                {
                    count += Appointments.OtherAppointments.Count;
                }
            }

            return count;
        }

        public int GetItemViewType(int position)
        {
            return IsIndexInUpcommingItems(position) ? 0 : 1;
        }

        public UniversalViewHolder CreateItemView(ViewGroup parent, int viewType)
        {
            var inflater = LayoutInflater.FromContext(Activity);

            var view = inflater.Inflate(viewType == 0 ? Resource.Layout.template_appointment_upcomming_home : Resource.Layout.template_appointment_previous_home, null);

            view.LayoutParameters = new ConstraintLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);

            var viewMap = new Dictionary<int, View>();
            viewMap.Add(Resource.Id.appointmentStatusImage, view.FindViewById(Resource.Id.appointmentStatusImage));
            viewMap.Add(Resource.Id.topLineImage, view.FindViewById(Resource.Id.topLineImage));
            viewMap.Add(Resource.Id.bottomLineImage, view.FindViewById(Resource.Id.bottomLineImage));
            viewMap.Add(Resource.Id.appointmentsHeader, view.FindViewById(Resource.Id.appointmentsHeader));
            viewMap.Add(Resource.Id.appointmentTitle, view.FindViewById(Resource.Id.appointmentTitle));
            viewMap.Add(Resource.Id.appointmentDate, view.FindViewById(Resource.Id.appointmentDate));
            viewMap.Add(Resource.Id.appointmentTime, view.FindViewById(Resource.Id.appointmentTime));
            viewMap.Add(Resource.Id.appointmentRequestTime, view.FindViewById(Resource.Id.appointmentRequestTime));

            if (viewType == 0)
            {
                viewMap.Add(Resource.Id.cancelAppointmentButton, view.FindViewById(Resource.Id.cancelAppointmentButton));
                viewMap.Add(Resource.Id.videoCallButton, view.FindViewById(Resource.Id.videoCallButton));
            }
            else
            {
                viewMap.Add(Resource.Id.cancelledStatusText, view.FindViewById(Resource.Id.cancelledStatusText));
            }

            return new UniversalViewHolder(view, viewMap);
        }

        public void BindViewHolder(UniversalViewHolder holder, int position)
        {
            bool isUpcommingAppointments = holder.ItemViewType == 0;

            var data = isUpcommingAppointments ? Appointments.UpcommingAppointments[position] : Appointments.OtherAppointments[position - Appointments.UpcommingAppointments.Count];

            holder.GetView<TextView>(Resource.Id.appointmentTitle).Text = String.Join(", ", data.Topics);

            var appointmentRequestTime = holder.GetView<TextView>(Resource.Id.appointmentRequestTime);

            if (data.AppointmentTime.HasValue)
            {
                holder.GetView<TextView>(Resource.Id.appointmentDate).Text = data.AppointmentTime.Value.ToString("dd.MM.yy");
                holder.GetView<TextView>(Resource.Id.appointmentTime).Visibility = ViewStates.Visible;
                holder.GetView<TextView>(Resource.Id.appointmentTime).Text = $"{Resources.GetString(Resource.String.general_view_timeprefix)} {data.AppointmentTime.Value.GetTimeString()}";
            }
            else
            {
                holder.GetView<TextView>(Resource.Id.appointmentTime).Visibility = ViewStates.Gone;
                holder.GetView<TextView>(Resource.Id.appointmentDate).Text = isUpcommingAppointments ? Resources.GetString(Resource.String.appointment_home_pending_title) : AppResources.AppointmentHomeNoDateExpired;
            }

            if(data.RequestedTime.HasValue)
            {
                appointmentRequestTime.Visibility = ViewStates.Visible;
                appointmentRequestTime.Text = data.RequestedTime.Value.ToString(AppResources.AppLongDateTimeFormat);
            }
            else
            {
                appointmentRequestTime.Visibility = ViewStates.Gone;
            }

            if (isUpcommingAppointments)
            {
                bool isLastRow = position == Appointments.UpcommingAppointments.Count - 1;
                bool isNextSectionPresent = Appointments.OtherAppointments != null && Appointments.OtherAppointments.Count > 0;

                holder.GetView<View>(Resource.Id.appointmentsHeader).Visibility = position == 0 ? ViewStates.Visible : ViewStates.Gone;
                holder.GetView<View>(Resource.Id.topLineImage).Visibility = position == 0 ? ViewStates.Invisible : ViewStates.Visible;
                holder.GetView<View>(Resource.Id.bottomLineImage).Visibility = isLastRow && !isNextSectionPresent ? ViewStates.Invisible : ViewStates.Visible;

                holder.GetView<ImageView>(Resource.Id.appointmentStatusImage).SetImageResource(data.Status == AppointmentStatus.Confirmed ? Resource.Drawable.appointment_upcomming_status_confirmed : Resource.Drawable.appointment_upcomming_status_pending);

                holder.GetView<View>(Resource.Id.cancelAppointmentButton).Tag = position;
                holder.GetView<View>(Resource.Id.cancelAppointmentButton).Click += CancelAppointment_Click;

                if (!string.IsNullOrEmpty(data.Doctor.VideoUrl) && data.IsVideoConsultationConfirmed)
                {
                    var videoCallButton = holder.GetView<View>(Resource.Id.videoCallButton);
                    videoCallButton.Visibility = ViewStates.Visible;
                    videoCallButton.Tag = position;
                    videoCallButton.Click += VideoCallButton_Tapped;
                }
            }
            else
            {
                bool isPreviousSectionPresent = Appointments.UpcommingAppointments != null && Appointments.UpcommingAppointments.Count > 0;
                bool isNotFirstAppointmentInSection = position - Appointments.UpcommingAppointments.Count != 0;
                bool isFistItemInPreviousSection = position == Appointments.UpcommingAppointments.Count;

                holder.GetView<View>(Resource.Id.appointmentsHeader).Visibility = isFistItemInPreviousSection ? ViewStates.Visible : ViewStates.Gone;
                holder.GetView<View>(Resource.Id.topLineImage).Visibility = isPreviousSectionPresent || isNotFirstAppointmentInSection ? ViewStates.Visible : ViewStates.Invisible;
                holder.GetView<View>(Resource.Id.bottomLineImage).Visibility = position == Appointments.Count - 1 ? ViewStates.Invisible : ViewStates.Visible;

                holder.GetView<ImageView>(Resource.Id.appointmentStatusImage).SetImageResource(data.Status == AppointmentStatus.Confirmed ? Resource.Drawable.appointment_previous_status_confirmed : Resource.Drawable.appointment_previous_status_others);

                var cancelledStatusText = holder.GetView<TextView>(Resource.Id.cancelledStatusText);
                cancelledStatusText.Visibility = ViewStates.Invisible;

                if(data.Status == AppointmentStatus.Cancelled)
                {
                    cancelledStatusText.Text = AppResources.AppointmentHomeCancelledStatus;
                    cancelledStatusText.Visibility = ViewStates.Visible;
                }
            }
        }

        void CancelAppointment_Click(object sender, EventArgs e)
        {
            if(sender is View view)
            {
                var position = (int)view.Tag;
                CancelAppointment(Appointments.UpcommingAppointments[position]).Forget();
            }
        }


        public void OnItemClick(int position)
        {
            var data = IsIndexInUpcommingItems(position) ? Appointments.UpcommingAppointments[position] : Appointments.OtherAppointments[position - Appointments.UpcommingAppointments.Count];

            Presenter.ShowAppointmentDetails(data);
        }

        void ScrollListner_LoadMore(object sender, LoadMoreEventArgs e)
        {
            LoadMoreData().Forget();
        }

        void DataListViewRefreshContainer_Refresh(object sender, EventArgs e)
        {
            if(!isLoading)
            {
                RefreshData().Forget();
            }
            else
            {
                dataListViewRefreshContainer.Refreshing = false;
            }
        }

        void VideoCallButton_Tapped(object sender, EventArgs e)
        {
            if (sender is View view)
            {
                var position = (int)view.Tag;
                var data = Appointments.UpcommingAppointments[position];

                NavigateToBrowser(data.Doctor.VideoUrl);
            }
        }

        private void VideoConsultation_Tapped(object sender, EventArgs e)
        {
            NavigateToBrowser(doctorDetails.VideoUrl);
        }

        private void CheckForDoctorVideoUrl()
        {
            doctorDetails = Presenter.GetDoctor().Result.Result;
            if (!string.IsNullOrEmpty(doctorDetails.VideoUrl))
            {
                videoLinkButton.Visibility = ViewStates.Visible;
                videoLinkButton.Text = AppResources.AppointmentLinkButtonText;
            }
        }
    }
}
