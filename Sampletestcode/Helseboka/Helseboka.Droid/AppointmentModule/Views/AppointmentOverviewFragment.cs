
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using Helseboka.Core.AppointmentModule.Interface;
using Helseboka.Core.AppointmentModule.Model;
using Helseboka.Core.Common.Extension;
using Helseboka.Core.Resources.StringResources;
using Helseboka.Droid.Common.Adapters;
using Helseboka.Droid.Common.Utils;
using Helseboka.Droid.Common.Views;

namespace Helseboka.Droid.AppointmentModule.Views
{
    public class AppointmentOverviewFragment : BaseFragment, IMultipleItemUniversalAdapter
    {
        private ImageView backButton;
        private TextView cancelAppointmentButton;
        private TextView appointmentDate;
        private TextView pageSubTitle;
        private RecyclerView dataListView;

        private AppointmentDetails Appointment { get; set; }
        private IAppointmentPresenter Presenter
        {
            get => presenter as IAppointmentPresenter;
        }

        public AppointmentOverviewFragment(IAppointmentPresenter presenter, AppointmentDetails appointment)
        {
            this.presenter = presenter;
            this.Appointment = appointment;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_appointment_overview, null);

            backButton = view.FindViewById<ImageView>(Resource.Id.back);
            cancelAppointmentButton = view.FindViewById<TextView>(Resource.Id.cancelAppointmentButton);
            appointmentDate = view.FindViewById<TextView>(Resource.Id.appointmentDate);
            pageSubTitle = view.FindViewById<TextView>(Resource.Id.pageSubTitle);
            dataListView = view.FindViewById<RecyclerView>(Resource.Id.dataListView);

            dataListView.SetLayoutManager(new LinearLayoutManager(Activity, LinearLayoutManager.Vertical, false));
            dataListView.SetAdapter(new GenericRecyclerAdapter(this));

            backButton.Click += BackButton_Click;
            cancelAppointmentButton.Click += CancelAppointmentButton_Click;

            dataListView.GetAdapter().NotifyDataSetChanged();
            LoadData().Forget();

            return view;
        }

        private async Task LoadData()
        {
            cancelAppointmentButton.Visibility = Appointment.IsCancellationPossible ? ViewStates.Visible : ViewStates.Invisible;
            appointmentDate.Text = Appointment.AppointmentTime.HasValue ? Appointment.AppointmentTime.Value.ToString(AppResources.AppLongDateTimeFormat) : Resources.GetString(Resource.String.appointment_home_pending_title);
            var doctor = await Presenter.GetDoctorDetails(Appointment);
            if (doctor != null)
            {
                pageSubTitle.Text = doctor.FullName;
            }
        }

        private async Task CancelAppointment()
        {
            ShowLoader();
            var response = await Presenter.CancelAppointment(Appointment);
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

        void BackButton_Click(object sender, EventArgs e)
        {
            Presenter.GoBackToHome();
        }

        public override bool OnBackKeyPressed()
        {
            Presenter.GoBackToHome();
            return true;
        }

        void CancelAppointmentButton_Click(object sender, EventArgs e)
        {
            CancelAppointment().Forget();
        }

        public int GetItemCount()
        {
            if(Appointment != null && Appointment.Topics != null)
            {
                return Appointment.Topics.Count + 1;
            }
            else
            {
                return 1;
            }
        }

        public int GetItemViewType(int position)
        {
            return position == 0 ? 0 : 1;
        }

        public UniversalViewHolder CreateItemView(ViewGroup parent, int viewType)
        {
            var inflater = LayoutInflater.FromContext(Activity);
            var view = inflater.Inflate(viewType == 0 ? Resource.Layout.template_appointment_details_header : Resource.Layout.template_appointment_details_item, null);
            view.LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);

            var viewMap = new Dictionary<int, View>();

            if(viewType == 0)
            {
                viewMap.Add(Resource.Id.focusedMessageFromDoctor, view.FindViewById(Resource.Id.focusedMessageFromDoctor));
            }
            else
            {
                viewMap.Add(Resource.Id.titleText, view.FindViewById(Resource.Id.titleText));
                viewMap.Add(Resource.Id.messageBody, view.FindViewById(Resource.Id.messageBody));
                viewMap.Add(Resource.Id.separator, view.FindViewById(Resource.Id.separator));
            }

            return new UniversalViewHolder(view, viewMap);
        }

        public void BindViewHolder(UniversalViewHolder holder, int position)
        {
            if(holder.ItemViewType == 0)
            {
                var messageFromDoctor = holder.GetView<TextView>(Resource.Id.focusedMessageFromDoctor);

                if(!String.IsNullOrEmpty(Appointment.DoctorFocusedReply))
                {
                    messageFromDoctor.Text = Appointment.DoctorFocusedReply;
                }
                else
                {
                    messageFromDoctor.Text = AppResources.AppointmentNoMessage;
                }
            }
            else if (Appointment != null && Appointment.Topics != null && position <= Appointment.Topics.Count)
            {
                var data = Appointment.Topics[position - 1];

                holder.GetView<TextView>(Resource.Id.titleText).Text = $"{Resources.GetString(Resource.String.appointment_details_symptom_title)} {position}";
                holder.GetView<TextView>(Resource.Id.messageBody).Text = data;
                if (position == Appointment.Topics.Count)
                {
                    holder.GetView<View>(Resource.Id.separator).Visibility = ViewStates.Gone;
                }
            }
        }

        public void OnItemClick(int position)
        {
        }
    }
}
