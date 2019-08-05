using System;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using Helseboka.Core.Common.EnumDefinitions;
using Helseboka.iOS.Common.View;
using UIKit;
using Helseboka.iOS.Common.Extension;
using System.ComponentModel;

namespace Helseboka.iOS.Legetimer.View
{
    [Register("WeekDayView"), DesignTimeVisible(true)]
    public class WeekDayView : UIView
    {
        public event EventHandler<DayOfWeek> DaySelected;

        public event EventHandler<DayOfWeek> DayUnSelected;

        private List<CheckBox> weekDays;

        private List<String> week = new List<string>() { "Appointment.New.SelectTime.Monday", "Appointment.New.SelectTime.Tuesday", "Appointment.New.SelectTime.Wednesday", "Appointment.New.SelectTime.Thursday", "Appointment.New.SelectTime.Friday" };

        protected WeekDayView(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
            CommonInitialization();
        }

        public WeekDayView()
        {
            CommonInitialization();
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            CommonInitialization();
        }

        protected void CommonInitialization()
        {
            weekDays = new List<CheckBox>();
            this.RemoveAllSubViews();

            for (int index = 0; index < 5; index++)
            {
                var checkBox = new CheckBox();
                checkBox.Tag = index;
                checkBox.SelectionChanged += CheckBox_SelectionChanged;
                checkBox.SetTitle(week[index].Translate(), UIControlState.Normal);

                weekDays.Add(checkBox);
                AddSubview(checkBox);
            }

        }

        private void CheckBox_SelectionChanged(object sender, bool e)
        {
            var index = (sender as UIView).Tag;

            var dayIntValue = index + 1;

            if (Enum.TryParse<DayOfWeek>(dayIntValue.ToString(), out var day))
            {
                if (e)
                {
                    DaySelected?.Invoke(this, day);
                }
                else
                {
                    DayUnSelected?.Invoke(this, day);
                }
            }

        }


        protected void ResizeElements()
        {
            var totalWidth = Frame.Width;
            var totalHeight = Frame.Height;

            var buttonWidth = totalWidth / 5;
            var buttonHeight = 60;
            var buttonTopPosition = (totalHeight - buttonHeight) / 2;

            var index = 0;
            foreach (var checkBox in weekDays)
            {
                var buttonLeft = index * buttonWidth;
                checkBox.Frame = new CGRect(buttonLeft, buttonTopPosition, buttonWidth, buttonHeight);
                index++;
            }
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            ResizeElements();
        }
    }
}
