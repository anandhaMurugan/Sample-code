using Foundation;
using Helseboka.Core.Common.EnumDefinitions;
using Helseboka.Core.Common.Extension;
using Helseboka.Core.Dashboard.Interface;
using Helseboka.Core.MedicineModule.Model;
using Helseboka.iOS.Common.Constant;
using Helseboka.iOS.Common.Extension;
using Helseboka.iOS.Common.View.PopUpDialogs;
using ObjCRuntime;
using System;
using System.Threading.Tasks;
using UIKit;

namespace Helseboka.iOS.Dashboard.View
{
    public partial class DashboardAlarmView : UIView
    {
        private IDashboardPresenter presenter;
        private AlarmDetails AlarmDetails { get; set; }
        public DashboardAlarmView (IntPtr handle) : base (handle)
        {
            
        }

        public static DashboardAlarmView Create(IDashboardPresenter presenter)
        {
            var view = Runtime.GetNSObject<DashboardAlarmView>(NSBundle.MainBundle.LoadNib("DashboardAlarmView", null, null).ValueAt(0));
            view.presenter = presenter;
            return view;
        }

        public override void AwakeFromNib()
        {
            Overlay.AddBorder(UIColor.White, 12);
            this.AddShadow();
        }

        public void Configure(AlarmDetails alarmDetails)
        {
            this.AlarmDetails = alarmDetails;

            if (AlarmDetails != null)
            {
                AlarmTimeLabel.Text = AlarmDetails.Time.GetTimeString();
                MedicineNameLabel.Text = AlarmDetails.Medicine.Name;
                MedicineStrengthLabel.Text = AlarmDetails.Medicine.Strength;
                AlarmDoneCheck.Selected = AlarmDetails.Status == AlarmStatus.Completed;

                if (AlarmDetails.Status == AlarmStatus.Completed)
                {
                    Overlay.Hidden = false;
                    AlarmDoneCheck.UserInteractionEnabled = false;
                    AlarmDoneCheck.SelectionChanged -= AlarmDoneCheck_SelectionChanged;
                }
                else
                {
                    Overlay.Hidden = true;
                    AlarmDoneCheck.UserInteractionEnabled = true;
                    AlarmDoneCheck.SelectionChanged -= AlarmDoneCheck_SelectionChanged;
                    AlarmDoneCheck.SelectionChanged += AlarmDoneCheck_SelectionChanged;
                }

                if (AlarmDetails.IsNextAlarm)
                {
                    this.AddBorder(Colors.DashboardAlarmBorderColor, 12);
                }
                else
                {
                    this.AddBorder(UIColor.White, 12);
                }

               
            }
        }

        private void AlarmDoneCheck_SelectionChanged(object sender, bool e)
        {
            if (e)
            {
                var title = "Dashboard.Alarm.Confirmation.Title".Translate();
                var message = "Dashboard.Alarm.Confirmation.Message".Translate();
                var yesText = "Dashboard.Alarm.Confirmation.Yes".Translate();

                var dialog = new YesNoDialogView(title, message, yesText);
                dialog.LeftButtonTapped += AlarmConfirmation_OkTapped;
                dialog.Closed += AppointmentConfirmation_Cancelled;
                dialog.Show();
            }
        }

        void AppointmentConfirmation_Cancelled(object sender, EventArgs e)
        {
            AlarmDoneCheck.Selected = false;
        }


        void AlarmConfirmation_OkTapped(object sender, EventArgs e)
        {
            MarkAlarmComplete().Forget();
        }


        private async Task MarkAlarmComplete()
        {
            if (presenter != null)
            {
                var alarm = AlarmDetails;
                var response = await presenter.MarkAlarmAsComplete(alarm);
                if (response.IsSuccess && alarm.Id == AlarmDetails.Id)
                {
                    Overlay.Hidden = false;
                    AlarmDoneCheck.Selected = true;
                    AlarmDoneCheck.UserInteractionEnabled = false;
                    AlarmDoneCheck.SelectionChanged -= AlarmDoneCheck_SelectionChanged;
                }
            }
        }

    }
}