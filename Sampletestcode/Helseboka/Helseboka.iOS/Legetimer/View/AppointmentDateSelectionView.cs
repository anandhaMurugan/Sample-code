using System;
using System.Linq;
using System.Collections.Generic;
using Foundation;
using Helseboka.Core.AppointmentModule.Interface;
using Helseboka.Core.Common.EnumDefinitions;
using Helseboka.Core.Common.Extension;
using Helseboka.iOS.Common.Constant;
using Helseboka.iOS.Common.PlatformEnums;
using Helseboka.iOS.Common.Utilities;
using Helseboka.iOS.Common.View;
using Helseboka.iOS.Common.Extension;
using UIKit;
using System.Threading.Tasks;

namespace Helseboka.iOS.Legetimer.View
{
    public partial class AppointmentDateSelectionView : BaseView
    {
        public static readonly String Identifier = "AppointmentDateSelectionView";
        private HashSet<DayOfWeek> selectedDays = new HashSet<DayOfWeek>();
        private HashSet<TimeOfDay> availableTimes = new HashSet<TimeOfDay>();
        public IAppointmentPresenter Presenter
        {
            get => presenter as IAppointmentPresenter;
            set => presenter = value;
        }

        public AppointmentDateSelectionView() : base() { }

		public AppointmentDateSelectionView(IntPtr handler) : base(handler) { }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
			DesignUI().Forget();

            WeekView.DaySelected += WeekView_DaySelected;
            WeekView.DayUnSelected += WeekView_DayUnSelected;
            EarlyCheckBox.SelectionChanged += EarlyCheckBox_SelectionChanged;
            MiddayCheckBox.SelectionChanged += MiddayCheckBox_SelectionChanged;     
            LateDayCheckbox.SelectionChanged += LateDayCheckbox_SelectionChanged;
        }

        partial void Back_Pressed(UIButton sender)
        {
            Presenter.GoBackToHome();
        }

        partial void Help_Tapped(UIButton sender)
        {
            new ModalHelpViewController(HelpFAQType.AppointmentDateSelection).Show();
        }

        partial void NextButton_Tapped(PrimaryActionButton sender)
        {
            Presenter.DidSelectAppointmentDateTime(selectedDays.ToList(), availableTimes.ToList());
        }

        private void WeekView_DaySelected(object sender, DayOfWeek e)
        {
            selectedDays.Add(e);
        }

        private void WeekView_DayUnSelected(object sender, DayOfWeek e)
        {
            selectedDays.Remove(e);
        }

        private void EarlyCheckBox_SelectionChanged(object sender, bool e)
        {
            AddRemoveAvailableTime(TimeOfDay.Early, e);
        }

        private void MiddayCheckBox_SelectionChanged(object sender, bool e)
        {
            AddRemoveAvailableTime(TimeOfDay.Midday, e);
        }

        private void LateDayCheckbox_SelectionChanged(object sender, bool e)
        {
            AddRemoveAvailableTime(TimeOfDay.Late, e);
        }

        private void AddRemoveAvailableTime(TimeOfDay time, bool isSelected)
        {
            if (isSelected)
            {
                availableTimes.Add(time);
            }
            else
            {
                availableTimes.Remove(time);
            }
        }

        private async Task DesignUI()
        {
            if (Device.DeviceType == DeviceType.iPhones_5_5s_5c_SE)
            {
                BackButtonTopConstraint.Constant = 15;
                PageTitleTopConstraint.Constant = 15;
                PageSubtitleTopConstraint.Constant = 10;
                WeekViewTopConstraint.Constant = 10;
                NextButtonHeightConstraint.Constant = 45;
                PageInfoTopConstraint.Constant = 5;
                WeekDayViewHeightConstraint.Constant = 60;
                ViewLeadingConstraint.Constant = 15;
                ViewTrailingConstraint.Constant = -15;
                EarlyTopConstraint.Constant = 5;
                EarlyBottomConstraint.Constant = 5;
                MiddayTopConstraint.Constant = 5;
                MiddayBottomConstraint.Constant = 5;
                LateDayTopConstraint.Constant = 5;
                LateDayBottomConstraint.Constant = 5;
            }
            else if (Device.DeviceType == DeviceType.iPhones_6_6s_7_8 || Device.DeviceType == DeviceType.iPhones_6Plus_6sPlus_7Plus_8Plus)
            {
                BackButtonTopConstraint.Constant = 15;
                PageTitleTopConstraint.Constant = 15;
                PageSubtitleTopConstraint.Constant = 10;
                WeekViewTopConstraint.Constant = 10;
                PageInfoTopConstraint.Constant = 10;
                WeekDayViewHeightConstraint.Constant = 80;
                //WeekDayViewHeightConstraint.Constant = 60;
            }

            var response = await Presenter.GetDoctor();
            if (response.IsSuccess && response.Result != null)
            {
                string firstString = "Appointment.New.SelectTime.HelpText".Translate();

                var strAttributedText = new NSMutableAttributedString();
                strAttributedText.Append(new NSAttributedString($"{firstString}", Fonts.GetMediumFont(12), Colors.GrayLabelTextColor));
                if(!String.IsNullOrEmpty(response.Result.EmergencyNumber))
                {
                    strAttributedText.Append(new NSAttributedString(" ", Fonts.GetMediumFont(12), Colors.GrayLabelTextColor));
                    strAttributedText.Append(new NSAttributedString($"{response.Result.EmergencyNumber}", Fonts.GetHeavyFont(12), Colors.HyperLinkLabelTextColor, underlineStyle: NSUnderlineStyle.Single));
                }

                strAttributedText.Append(new NSAttributedString(".", Fonts.GetMediumFont(12), Colors.GrayLabelTextColor));
                pageInfoLabel.AttributedText = strAttributedText;
            }
		}
    }
}

