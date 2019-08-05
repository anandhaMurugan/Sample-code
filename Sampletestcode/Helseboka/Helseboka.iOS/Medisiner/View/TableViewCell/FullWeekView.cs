using System;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using Helseboka.Core.Common.EnumDefinitions;
using Helseboka.iOS.Common.View;
using UIKit;
using Helseboka.iOS.Common.Extension;
using System.ComponentModel;

namespace Helseboka.iOS.Medisiner.View.TableViewCell
{
    [Register("FullWeekView"), DesignTimeVisible(true)]
    public class FullWeekView : UIView
    {
        public event EventHandler<DayOfWeek> DaySelected;

        public event EventHandler<DayOfWeek> DayUnSelected;

        private List<CheckBox> weekDays;

        private List<String> week = new List<string>() { "Medicine.Reminder.Monday", "Medicine.Reminder.Tuesday", "Medicine.Reminder.Wednesday", "Medicine.Reminder.Thursday", "Medicine.Reminder.Friday", "Medicine.Reminder.Saturday", "Medicine.Reminder.Sunday" };

        protected FullWeekView(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
            //CommonInitialization();
        }

        public FullWeekView()
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

            for (int index = 0; index < 7; index++)
            {
                var checkBox = new CheckBox();
                checkBox.Tag = index;
                checkBox.SelectionChanged += CheckBox_SelectionChanged;
                checkBox.SetTitle(week[index].Translate(), UIControlState.Normal);

                weekDays.Add(checkBox);
                this.AddSubview(checkBox);
            }
        }

        public void ConfigureSelected(List<DayOfWeek> selectedDays)
        {
            if (selectedDays != null && selectedDays.Count > 0)
            {
                foreach (var item in selectedDays)
                {
                    var dayIntValue = (int)item;
                    var index = dayIntValue == 0 ? 6 : dayIntValue - 1;

                    var checkBox = weekDays[index];
                    checkBox.Selected = true;
                }
            }
        }

        private void CheckBox_SelectionChanged(object sender, bool e)
        {
            var index = (sender as UIView).Tag;

            var dayIntValue = index == 6 ? 0 : index + 1;

            if (Enum.TryParse<DayOfWeek>(dayIntValue.ToString(),out var day))
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

            var buttonWidth = totalWidth / 7;
            var buttonHeight = 70;
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
