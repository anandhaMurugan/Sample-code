using System;
using Helseboka.iOS.Common.Extension;
using Foundation;
using Helseboka.Core.AppointmentModule.Model;
using UIKit;
using Helseboka.iOS.Common.Constant;
using Helseboka.Core.Common.EnumDefinitions;
using Helseboka.Core.Common.Extension;
using Helseboka.Core.Resources.StringResources;

namespace Helseboka.iOS.Legetimer.View.TableViewCell
{
    public partial class PreviousLegetimerTableCell : UITableViewCell
    {
        private AppointmentDetails appointment;
        public static readonly NSString Key = new NSString("PreviousLegetimerTableCell");
        public event EventHandler<AppointmentDetails> AppointmentSelected;

        protected PreviousLegetimerTableCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            AppointmentDetailsView.AddGestureRecognizer(new UITapGestureRecognizer(() => AppointmentSelected?.Invoke(this, appointment)));

            AppointmentDetailsView.AddBorder(Colors.AppointmentOtherViewBorderColor, 14);
            CircleView.Layer.BorderWidth = 1;
            CircleView.Layer.BorderColor = Colors.AppointmentOtherViewCircleBorderColor.CGColor;
        }

        public void UpdateCell(AppointmentDetails data, bool isFirstCell, bool isLastCell)
        {
            appointment = data;

            TopLine.Hidden = isFirstCell;
            BottomLine.Hidden = isLastCell;
            CancelledStatusLabel.Hidden = true;

            if (data.Status == AppointmentStatus.Confirmed)
            {
                StatusImageView.Image = UIImage.FromBundle("Appointment-status-confirmed-past");
            }
            else
            {
                StatusImageView.Image = UIImage.FromBundle("Appointment-status-canceled");
            }

            if (data.Status == AppointmentStatus.Cancelled)
            {
                CancelledStatusLabel.Text = AppResources.AppointmentHomeCancelledStatus;
                CancelledStatusLabel.Hidden = false;
            }

            descriptionLabel.Text = String.Join(", ", data.Topics);
            if (data.AppointmentTime.HasValue)
            {
                dateLabel.Text = data.AppointmentTime.Value.ToString("dd.MM.yy");
                timeLabel.Text = $"{"General.View.TimePrefix".Translate()} {data.AppointmentTime.Value.GetTimeString()}";
            }
            else
            {
                dateLabel.Text = AppResources.AppointmentHomeNoDateExpired;
                timeLabel.Text = String.Empty;
            }

            if (appointment.RequestedTime.HasValue)
            {
                AppointmentRequestTime.Hidden = false;
                AppointmentRequestTime.Text = appointment.RequestedTime.Value.ToString(AppResources.AppLongDateTimeFormat);
            }
            else
            {
                AppointmentRequestTime.Hidden = true;
            }

        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            CircleView.Layer.CornerRadius = CircleView.Frame.Height / 2;
        }
    }
}
