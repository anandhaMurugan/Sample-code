using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UIKit;
using Xamarin.iOS.iCarouselBinding;
using Helseboka.iOS.Common.Extension;
using Helseboka.Core.Common.Extension;
using CoreGraphics;
using Helseboka.iOS.Common.Utilities;
using Helseboka.iOS.Common.PlatformEnums;
using Helseboka.iOS.Common.Constant;
using System.Threading.Tasks;
using Helseboka.iOS.Common.View;
using Helseboka.Core.Dashboard.Interface;
using Helseboka.Core.Common.Model;
using Helseboka.Core.MedicineModule.Model;
using Helseboka.Core.Common.EnumDefinitions;
using Helseboka.Core.Resources.StringResources;
using Foundation;
using Helseboka.Core.AppointmentModule.Model;

namespace Helseboka.iOS.Dashboard.View
{
    public partial class DashboardHomeView : BaseView
    {
        private DateTime selectedDate = DateTime.Now;
        private iCarousel carousel;
        private AlarmDataSource alarmDataSource = new AlarmDataSource();
        private UITapGestureRecognizer MedicineReminderNoDataTapGestureRecognizer;
        private UITapGestureRecognizer NoAppointmentTapGestureRecognizer;
        private UITapGestureRecognizer videoCallButtonGesture;
        private AppointmentDetails appointment;

        public static readonly String Identifier = "DashboardHomeView";
        public IDashboardPresenter Presenter
        {
            get => presenter as IDashboardPresenter;
            set => presenter = value;
        }

        public DashboardHomeView() { }

        public DashboardHomeView(IntPtr ptr) : base(ptr) { }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            alarmDataSource.Presenter = Presenter;
            DesignView();
            MedicineReminderNoDataTapGestureRecognizer = new UITapGestureRecognizer(OnNoReminderCenterViewTapped);
            NoAppointmentTapGestureRecognizer = new UITapGestureRecognizer(OnNoAppointmentViewTapped);

            VideoCallButton.UserInteractionEnabled = true;
            if (videoCallButtonGesture == null)
            {
                videoCallButtonGesture = new UITapGestureRecognizer(VideoCallButton_Tapped);
            }
            VideoCallButton.RemoveGestureRecognizer(videoCallButtonGesture);
            VideoCallButton.AddGestureRecognizer(videoCallButtonGesture);

            ShowAlarmLoader();
            ShowAppointmentLoader();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            UpdateViewAccordingToDate();
            LoadAppointments().Forget();
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            NextAppointmentAlertLabel.Layer.CornerRadius = NextAppointmentAlertLabel.Frame.Height / 2;
        }

        partial void NextDate_Tapped(UIButton sender)
        {
            selectedDate = selectedDate.AddDays(1);
            UpdateViewAccordingToDate();
        }

        partial void PreviousDate_Tapped(UIButton sender)
        {
            selectedDate = selectedDate.AddDays(-1);
            UpdateViewAccordingToDate();
        }

        public void UpdateViewAccordingToDate()
        {
            var dayPrefix = "";
            if (selectedDate.GetDay() != Day.Others)
            {
                dayPrefix = selectedDate.GetDayString().ToLower();
            }

            DateLabel.Text = $"{dayPrefix.ToUpper()} {selectedDate.ToString("dd.MM.yyyy")}".Trim();
            ShowAlarmLoader();
            LoadAlarms().Forget();
        }

        public async Task LoadAppointments()
        {
            appointment = await Presenter.GetNextAppointmentDetails();
            if (appointment != null && appointment.AppointmentTime.HasValue)
            {
                var dateFormat = AppResources.AppLongDateTimeFormat;
                NextAppointmentDateLabel.Text = appointment.AppointmentTime.Value.ToString(dateFormat);
                NextAppointmentTitleLabel.Text = "Dashboard.Appointment.Title".Translate() + $"\n{appointment.Doctor.FullName}";
                ShowAppointmentView();
                if(!String.IsNullOrEmpty(appointment.DoctorFocusedReply))
                {
                    NextAppointmentAlertLabel.Text = appointment.DoctorFocusedReply;
                    NextAppointmentAlertLabel.Hidden = false;
                }
                else
                {
                    NextAppointmentAlertLabel.Hidden = true;
                }

                if (!string.IsNullOrEmpty(appointment.Doctor.VideoUrl) && appointment.IsVideoConsultationConfirmed)
                {
                    VideoCallButton.Hidden = false;
                }
            }
            else
            {
                ShowNoAppointmentView();
            }
        }


        public async Task LoadAlarms()
        {
            var alarms = await Presenter.GetAlarms(selectedDate);

            if (alarms != null && alarms.Count > 0)
            {
                alarmDataSource.DataList = alarms;
                carousel.ReloadData();

                ShowAlarmView();

                var nextAlarm = alarms.Where(x => x.IsNextAlarm).FirstOrDefault();
                if (nextAlarm != null)
                {
                    var nextIndex = alarms.IndexOf(nextAlarm);
                    carousel.ScrollToItemAtIndex(nextIndex, true);
                }
                else
                {
                    carousel.ScrollToItemAtIndex(0, true);
                }
            }
            else
            {
                ShowNoAlarmView();
            }
        }

        private void DesignView()
        {
            carousel = new iCarousel
            {
                ContentMode = UIViewContentMode.Center,
                Type = iCarouselType.Linear,
                Frame = MedicineView.Bounds,
                CenterItemWhenSelected = true,
                DataSource = alarmDataSource
            };

            NameLabel.Text = $"{AppResources.Salutation} {ApplicationCore.Instance.CurrentUser.FirstName.ToNameCase()}";

            MedicineView.AddSubview(carousel);

            NextAppointmentAlertLabel.Padding = new UIEdgeInsets(5, 30, 5, 30);
            NextAppointmentAlertLabel.Layer.BackgroundColor = Colors.DashboardAppointmentAlertBackgroundColor.CGColor;

            NextAppointmentView.AddBorder(Colors.DashboardAppointmentBorderColor, 12, 2);
            NextAppointmentView.AddShadow();

            NoDataLeftView.AddBorder(Colors.DashboardAlarmPlaceHolderBorderColor, 12);
            NoDataLeftView.AddShadow();

            NoDataRightView.AddBorder(Colors.DashboardAlarmPlaceHolderBorderColor, 12);
            NoDataRightView.AddShadow();

            NoDataCenterView.AddBorder(Colors.DashboardAlarmBorderColor, 12);
            NoDataCenterView.AddShadow();

            if (Device.DeviceType == DeviceType.iPhones_5_5s_5c_SE)
            {
                NameTopConstraint.Constant = 10;
                HeaderViewHeightConstraint.Constant = 45;
                NextAppointmentViewBottomConstraint.Constant = -20;
            }
            else if (Device.DeviceType == DeviceType.iPhones_6Plus_6sPlus_7Plus_8Plus || Device.DeviceType == DeviceType.iPhoneX)
            {
                NextAppointmentViewBottomConstraint.Constant = -90;
            }
        }

        private void ShowNoAlarmView()
        {
            MedicineLoadingIndicator.StopAnimating();
            MedicineLoadingIndicator.Hidden = true;

            MedicineNoDataView.Hidden = false;
            MedicineView.Hidden = true;
            HeaderView.Hidden = false;;
            NextMedicineLabel.Hidden = true;

            //BackgroundToHeaderConstraint.Active = true;
            //NameTopConstraint.Active = true;
            MedicineNoDataCenterView.RemoveGestureRecognizer(MedicineReminderNoDataTapGestureRecognizer);
            MedicineNoDataCenterView.AddGestureRecognizer(MedicineReminderNoDataTapGestureRecognizer);
        }
        private void ShowAlarmView()
        {
            carousel.Hidden = false;
            MedicineLoadingIndicator.StopAnimating();
            MedicineLoadingIndicator.Hidden = true;

            MedicineNoDataView.Hidden = true;
            MedicineView.Hidden = false;
            HeaderView.Hidden = false;
            NextMedicineLabel.Hidden = false;
            MedicineNoDataCenterView.RemoveGestureRecognizer(MedicineReminderNoDataTapGestureRecognizer);
        }
        private void ShowNoAppointmentView()
        {
            AppointmentLoadingIndicator.StopAnimating();
            AppointmentLoadingIndicator.Hidden = true;

            NextAppointmentDateLabel.Hidden = true;
            NextAppointmentAlertLabel.Hidden = true;
            NextAppointmentTitleLabel.Hidden = false;

            NextAppointmentTitleLabel.Text = "Dashboard.Appointment.NoData".Translate();

            NextAppointmentTitleToTopConstraint.Active = true;
            NextAppointmentTitleToBottomConstraint.Active = true;

            NextAppointmentDateLabelBottomToTitleTopConstraint.Active = false;
            NextAppointmentTitleBottomToAlertTopConstraint.Active = false;

            NextAppointmentView.RemoveGestureRecognizer(NoAppointmentTapGestureRecognizer);
            NextAppointmentView.AddGestureRecognizer(NoAppointmentTapGestureRecognizer);

        }
        private void ShowAppointmentView()
        {
            AppointmentLoadingIndicator.StopAnimating();
            AppointmentLoadingIndicator.Hidden = true;

            NextAppointmentDateLabel.Hidden = false;
            NextAppointmentAlertLabel.Hidden = false;
            NextAppointmentTitleLabel.Hidden = false;

            NextAppointmentTitleToTopConstraint.Active = false;
            NextAppointmentTitleToBottomConstraint.Active = false;

            NextAppointmentDateLabelBottomToTitleTopConstraint.Active = true;
            NextAppointmentTitleBottomToAlertTopConstraint.Active = true;

            NextAppointmentView.RemoveGestureRecognizer(NoAppointmentTapGestureRecognizer);
        }

        private void ShowAlarmLoader()
        {
            ShowAlarmView();
            carousel.Hidden = true;
            MedicineLoadingIndicator.Hidden = false;
            MedicineLoadingIndicator.StartAnimating();
        }

        private void ShowAppointmentLoader()
        {
            ShowNoAppointmentView();
            NextAppointmentTitleLabel.Hidden = true;
            AppointmentLoadingIndicator.Hidden = false;
            AppointmentLoadingIndicator.StartAnimating();
        }

        void OnNoReminderCenterViewTapped()
        {
            ShowInfoDialog(AppResources.AppointmentHomeNoAlarmPopupMessage);
        }

        void OnNoAppointmentViewTapped()
        {
            ShowInfoDialog(AppResources.AppointmentHomeNoAppointmentPopupMessage);
        }

        void VideoCallButton_Tapped()
        {
            UIApplication.SharedApplication.OpenUrl(NSUrl.FromString(appointment.Doctor.VideoUrl));
        }

    }

    public class AlarmDataSource : iCarouselDataSource
    {
        public IDashboardPresenter Presenter { get; set; }
        public List<AlarmDetails> DataList { get; set; }

        public override nint NumberOfItemsInCarousel(iCarousel carousel) => DataList != null ? DataList.Count : 0;

        public override UIView ViewForItemAtIndex(iCarousel carousel, nint index, UIView view)
        {
            DashboardAlarmView alarmView;

            if (view == null)
            {
                var alarmItemWidth = 185;
                var alarmItemHeight = 116;
                var alarmItemSpacing = 5;

                view = new UIView(new RectangleF(0, 0, alarmItemWidth + alarmItemSpacing, alarmItemHeight));

                alarmView = DashboardAlarmView.Create(Presenter);
                alarmView.Frame = new CGRect(alarmItemSpacing, 0, alarmItemWidth, alarmItemHeight);
                alarmView.Tag = 7; // Magic number ha ha ha
                view.AddSubview(alarmView);
            }
            else
            {
                alarmView = view.ViewWithTag(7) as DashboardAlarmView;
            }



            if (DataList != null && index < DataList.Count && alarmView != null)
            {
                var data = DataList[(int)index];
                alarmView.Configure(data);
            }

            view.Layer.DoubleSided = false;
            return view;
        }

    }
}

