using System;

using Foundation;
using Helseboka.Core.AppointmentModule.Model;
using Helseboka.iOS.Common.Extension;
using Helseboka.iOS.Common.Constant;
using Helseboka.iOS.Common.TableViewCell;
using UIKit;
using Helseboka.Core.Common.EnumDefinitions;
using Helseboka.Core.Common.Extension;
using Helseboka.Core.Resources.StringResources;
using SafariServices;

namespace Helseboka.iOS.Legetimer.View.TableViewCell
{
    public partial class UpcomingLegetimerTableCell : BaseTableViewCell
    {
        private AppointmentDetails appointment;
        private UITapGestureRecognizer videoCallButtonGesture;
        public static readonly NSString Key = new NSString("UpcomingLegetimerTableCell");
        public event EventHandler<AppointmentDetails> AppointmentSelected;
        public event EventHandler<AppointmentDetails> CancelTapped;


        protected UpcomingLegetimerTableCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            string firstString = "Appointment.Details.Feedback.Button".Translate();
            var strAttributedResult = new NSMutableAttributedString();
            strAttributedResult.Append(new NSAttributedString(firstString, Fonts.GetMediumFont(14), Colors.HyperLinkButtonTextColor, underlineStyle: NSUnderlineStyle.Single));
            CancelButton.SetAttributedTitle(strAttributedResult, UIControlState.Normal);
            AppointmentDetailsView.AddGestureRecognizer(new UITapGestureRecognizer(() => AppointmentSelected?.Invoke(this, appointment)));

            AppointmentDetailsView.AddBorder(Colors.UnselectedLabelBorderColor, 14);
            CircleView.Layer.BorderWidth = 1;
            CircleView.Layer.BorderColor = Colors.UnselectedLabelBorderColor.CGColor;

            VideoCallButton.UserInteractionEnabled = true;

            if (videoCallButtonGesture == null)
            {
                videoCallButtonGesture = new UITapGestureRecognizer(VideoCallButton_Tapped);
            }
            VideoCallButton.RemoveGestureRecognizer(videoCallButtonGesture);
            VideoCallButton.AddGestureRecognizer(videoCallButtonGesture);
        }

        void VideoCallButton_Tapped()
        {
            UIApplication.SharedApplication.OpenUrl(NSUrl.FromString(appointment.Doctor.VideoUrl));
        }

        partial void Cancel_Tapped(UIButton sender)
        {
            CancelTapped?.Invoke(this, appointment);
        }

        public void UpdateCell(AppointmentDetails data, bool isFirstCell, bool isLastCell)
        {
            appointment = data;

            TopLine.Hidden = isFirstCell;
            BottomLine.Hidden = isLastCell;

            if (data.Status == AppointmentStatus.Confirmed)
            {
                StatusImage.Image = UIImage.FromBundle("Appointment-status-confirmed");
            }
            else
            {
                StatusImage.Image = UIImage.FromBundle("Appointment-status-pending");
            }

            descriptionLabel.Text = String.Join(", ", data.Topics);
            if (data.AppointmentTime.HasValue)
            {
                dateLabel.Text = data.AppointmentTime.Value.ToString("dd.MM.yy");
                timeLabel.Text = $"{"General.View.TimePrefix".Translate()} {data.AppointmentTime.Value.GetTimeString()}";
            }
            else
            {
                dateLabel.Text = "Appointment.Home.Pending.Title".Translate();
                timeLabel.Text = String.Empty;
            }

            if(appointment.RequestedTime.HasValue)
            {
                AppointmentRequestTime.Hidden = false;
                AppointmentRequestTime.Text = appointment.RequestedTime.Value.ToString(AppResources.AppLongDateTimeFormat);
            }
            else
            {
                AppointmentRequestTime.Hidden = true;
            }

            if (!string.IsNullOrEmpty(data.Doctor.VideoUrl) && data.IsVideoConsultationConfirmed)
            {
                VideoCallButton.Hidden = false;
            }
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            CircleView.Layer.CornerRadius = CircleView.Frame.Height / 2;
        }
    }
}
