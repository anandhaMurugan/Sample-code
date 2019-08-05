using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Foundation;
using Helseboka.Core.AppointmentModule.Interface;
using Helseboka.Core.AppointmentModule.Model;
using Helseboka.Core.Common.Extension;
using Helseboka.iOS.Common.Constant;
using Helseboka.iOS.Common.View;
using Helseboka.iOS.Common.Extension;
using Helseboka.iOS.Legetimer.View.TableViewCell;
using UIKit;
using Helseboka.Core.Resources.StringResources;

namespace Helseboka.iOS.Legetimer.View
{
	public partial class AppointmentDetailsView : BaseView, IUITableViewDataSource, IUITableViewDelegate
    {
        public static readonly String Identifier = "AppointmentDetailsView";
        public AppointmentDetails Appointment { get; set; }
        public IAppointmentPresenter Presenter
        {
            get => presenter as IAppointmentPresenter;
            set => presenter = value;
        }

        public AppointmentDetailsView() { }

        public AppointmentDetailsView(IntPtr ptr) : base(ptr) { }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            DesignUI().Forget();
            DataTableView.AllowsSelection = false;
			DataTableView.Delegate = this;
			DataTableView.DataSource = this;
            DataTableView.RowHeight = UITableView.AutomaticDimension;
            DataTableView.EstimatedRowHeight = 80;
            DataTableView.ReloadData();
        }

        partial void Cancel_Tapped(UIButton sender)
        {
            CancelAppointment().Forget();
        }

        partial void Back_Pressed(UIButton sender)
        {
            Presenter.GoBackToHome();
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
                var result = await ProcessAPIError(response);
                if(!result)
                {
                    Presenter.GoBackToHome();
                }
            }
        }

        private async Task DesignUI()
        {
            string firstString = "Appointment.Details.Feedback.Button".Translate();
            var strAttributedResult = new NSMutableAttributedString();
			strAttributedResult.Append(new NSAttributedString(firstString, Fonts.GetMediumFont(15), Colors.HyperLinkButtonTextColor,underlineStyle: NSUnderlineStyle.Single));
            CancelButton.SetAttributedTitle(strAttributedResult, UIControlState.Normal);

            CancelButton.Hidden = !Appointment.IsCancellationPossible;
            AppointmentDateLabel.Text = Appointment.AppointmentTime.HasValue ? Appointment.AppointmentTime.Value.ToString(AppResources.AppLongDateTimeFormat) : "Appointment.Home.Pending.Title".Translate();
            var doctor = await Presenter.GetDoctorDetails(Appointment);
            if (doctor != null)
            {
                DoctorNameLabel.Text = doctor.FullName;
            }
        }

        [Export("tableView:heightForRowAtIndexPath:")]
		public nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
		{
			if (indexPath.Row == 0)
			{
                return UITableView.AutomaticDimension;
			}
			else
			{
				return 80;
			}
		}

		public nint RowsInSection(UITableView tableView, nint section)
		{
            return (Appointment.Topics != null ? Appointment.Topics.Count : 0) + 1;
		}

		public UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			if (indexPath.Row == 0)
			{
                var cell = tableView.DequeueReusableCell(AppointmentDetailsHeaderCell.Key) as AppointmentDetailsHeaderCell;
                cell.UpdateCell(Appointment);
                return cell;
			}
			else
			{
                var cell = tableView.DequeueReusableCell(SymptomTableViewCell.Key, indexPath) as SymptomTableViewCell;
                cell.UpdateCell(indexPath.Row - 1, Appointment.Topics[indexPath.Row - 1]);
				return cell;
			}
		}
	}
}

