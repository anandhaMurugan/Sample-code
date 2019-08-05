using System; using UIKit; using System.Threading.Tasks; using System.Collections.Generic; using System.Linq; using Helseboka.Core.Common.Extension; using Helseboka.Core.Common.Constant; using Helseboka.Core.Common.EnumDefinitions; using Helseboka.iOS.Common.Extension; using Helseboka.iOS.Common.Constant; using Helseboka.iOS.Common.View; using Helseboka.Core.Common.Interfaces; using Foundation; using CoreGraphics;
using Helseboka.Core.AppointmentModule.Interface;
using Helseboka.Core.AppointmentModule.Model;
using Helseboka.iOS.Common.TableViewDelegates;
using Helseboka.iOS.Legetimer.View.TableViewCell;
using Helseboka.Core.Common.Model;
using Helseboka.Core.Resources.StringResources;

namespace Helseboka.iOS.Legetimer.View
{
    public partial class LegetimerListView : BaseView, IUITableViewDataSource, IUITableViewDelegate
    {         public AppointmentCollection Appointments { get; protected set; }         private const int cellBuffer = 5;         private Doctor doctorDetails;
        public static readonly String Identifier = "LegetimerListView";         public IAppointmentPresenter Presenter         {             get => presenter as IAppointmentPresenter;             set => presenter = value;         }

        public LegetimerListView() { }

		public LegetimerListView(IntPtr ptr) : base(ptr) { }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();              CheckForDoctorVideoUrl();             ShowAppointmentList();             AppointmentListView.DataSource = this;             AppointmentListView.Delegate = this;             AppointmentListView.AllowsSelection = false;             AppointmentListView.AddPullToRefresh(() => RefreshAppointmentList());              AppointmentListView.RowHeight = UITableView.AutomaticDimension;             AppointmentListView.EstimatedRowHeight = 140;              HelpTextView.Text = String.Format(AppResources.AppointmentHomeHelpText, Environment.NewLine);         }          public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);             RefreshAppointmentList().Forget();
        }

        private void DataSource_AppointmentSelected(object sender, AppointmentDetails e)
        {             Presenter.ShowAppointmentDetails(e);
        }  
        private void DataSource_CancelTapped(object sender, AppointmentDetails e)
        {             CancelAppointment(e).Forget();
        }          partial void NewAppointment_Tapped(PrimaryActionButton sender)
        {             CheckDoctorAndProceed(() =>             {                 Presenter.ShowAppointmentDateSelectionView();             });         }

        private async Task CancelAppointment(AppointmentDetails appointment)         {             ShowLoader();             var response = await Presenter.CancelAppointment(appointment);              if (!response.IsSuccess)             {                 HideLoader();                 var result = await ProcessAPIError(response);                 if(result)                 {                     return;                 }                 ShowLoader();             }              await RefreshAppointmentList();             HideLoader();         }
         private async Task RefreshAppointmentList()         {
            AppointmentListView.IsLoading = true;
            AppointmentListView.ShowLoader();
            Appointments = await Presenter.GetAllAppointments();
            AppointmentListView.IsLoading = false;
            AppointmentListView.HideLoader();
            if (Appointments != null && Appointments.Count > 0)
            {
                AppointmentListView.ReloadData();
                ShowAppointmentList();
            }
            else
            {
                ShowNoData();
            }
        }          private async Task LoadMoreData()         {             if (Presenter.HasMoreData && !AppointmentListView.IsLoading)             {                 AppointmentListView.IsLoading = true;                 var response = await Presenter.LoadMore(Appointments);                 AppointmentListView.IsLoading = false;                 if (response)                 {                     AppointmentListView.ReloadData();                 }              }         }          private void ShowNoData()         {             HelpTextView.Hidden = false;             AppointmentListView.Hidden = true;         }          private void ShowAppointmentList()         {             HelpTextView.Hidden = true;             AppointmentListView.Hidden = false;         }          [Export("tableView:heightForRowAtIndexPath:")]
        public nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return UITableView.AutomaticDimension;
        }          [Export("numberOfSectionsInTableView:")]
        public nint NumberOfSections(UITableView tableView)
        {
            return 2;
        }          public nint RowsInSection(UITableView tableview, nint section)         {             if (Appointments != null)             {                 if (section == 0)                 {                     return Appointments.UpcommingAppointments != null ? Appointments.UpcommingAppointments.Count : 0;                 }                 else if (section == 1)                 {                     return Appointments.OtherAppointments != null ? Appointments.OtherAppointments.Count : 0;                 }                 else                 {                     return 0;                 }             }             else             {                 return 0;             }         }          [Export("tableView:viewForHeaderInSection:")]
        public UIView GetViewForHeader(UITableView tableView, nint section)
        {
            var cell = tableView.DequeueReusableCell(AppointmentListHeader.Key) as AppointmentListHeader;             String title = section == 0 ? "Appointment.Home.Header.Upcomming" : "Appointment.Home.Header.Previous";             title = title.Translate();             bool isUpcommingAppointmentExist = Appointments.UpcommingAppointments != null && Appointments.UpcommingAppointments.Count > 0;             bool isNextSectionPresent = Appointments.OtherAppointments != null && Appointments.OtherAppointments.Count > 0;             cell.Configure(title, section != 0 && section != 0 && isUpcommingAppointmentExist && isNextSectionPresent);             return cell;
        }          [Export("tableView:heightForHeaderInSection:")]
        public nfloat GetHeightForHeader(UITableView tableView, nint section)
        {
            if (Appointments != null)             {                 if (section == 0)                 {                     return Appointments.UpcommingAppointments != null ? 45 : 0;                 }                 else if (section == 1)                 {                     return Appointments.OtherAppointments != null ? 45 : 0;                 }                 else                 {                     return 0;                 }             }             else             {                 return 0;             }
        }          [Export("scrollViewDidScroll:")]
        public void Scrolled(UIScrollView scrollView)
        {
            if (Appointments != null && Appointments.Count > 0)             {                 var lastVisibleRow = AppointmentListView.IndexPathsForVisibleRows.Last().Row;                  if (Appointments.Count - lastVisibleRow <= cellBuffer)                 {                     LoadMoreData().Forget();                 }             }
        }          public UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)         {             if (indexPath.Section == 0 && Appointments.UpcommingAppointments != null && indexPath.Row < Appointments.UpcommingAppointments.Count)             {                 var data = Appointments.UpcommingAppointments[indexPath.Row];                 var cell = tableView.DequeueReusableCell(UpcomingLegetimerTableCell.Key) as UpcomingLegetimerTableCell;                 cell.AppointmentSelected -= DataSource_AppointmentSelected;                 cell.CancelTapped -= DataSource_CancelTapped;                 cell.AppointmentSelected += DataSource_AppointmentSelected;                 cell.CancelTapped += DataSource_CancelTapped;                  bool isLastRow = indexPath.Row == Appointments.UpcommingAppointments.Count - 1;                 bool isNextSectionPresent = Appointments.OtherAppointments != null && Appointments.OtherAppointments.Count > 0;                  cell.UpdateCell(data, indexPath.Row == 0, isLastRow && !isNextSectionPresent);                 return cell;             }             else if (indexPath.Section == 1 && Appointments.OtherAppointments != null && indexPath.Row < Appointments.OtherAppointments.Count)             {                 var data = Appointments.OtherAppointments[indexPath.Row];                  var cell = tableView.DequeueReusableCell(PreviousLegetimerTableCell.Key) as PreviousLegetimerTableCell;                 cell.AppointmentSelected -= DataSource_AppointmentSelected;                 cell.AppointmentSelected += DataSource_AppointmentSelected;                  bool isPreviousSectionPresent = Appointments.UpcommingAppointments != null && Appointments.UpcommingAppointments.Count > 0;                  cell.UpdateCell(data, indexPath.Row == 0 && !isPreviousSectionPresent, indexPath.Row == Appointments.OtherAppointments.Count - 1);                 return cell;             }             else             {                 return new UITableViewCell();             }         }

        partial void VideoConsultation_Tapped(PrimaryActionButton sender)
        {
            UIApplication.SharedApplication.OpenUrl(NSUrl.FromString(doctorDetails.VideoUrl));         }          private void CheckForDoctorVideoUrl()
        {
            doctorDetails = Presenter.GetDoctor().Result.Result;
            if (string.IsNullOrEmpty(doctorDetails.VideoUrl))
            {
                VideoLinkButton.Hidden = true;
                ListViewBottomConstraint.Constant = -56;
                HelpTextViewBottomConstraint.Constant = -56;
                return;
            }
            VideoLinkButton.SetTitle(AppResources.AppointmentLinkButtonText, UIControlState.Normal);
        }      }
}

