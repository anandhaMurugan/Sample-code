using System;
using System.Collections.Generic;
using Foundation;
using Helseboka.iOS.Common.TableViewCell;
using UIKit;

namespace Helseboka.iOS.Medisiner.View.TableViewCell
{
    public partial class FullWeekTableViewCell : BaseTableViewCell
    {
        private UITapGestureRecognizer addmoreLabelTap;

        public event EventHandler AddMoreMedicineTapped;

        public event EventHandler<DayOfWeek> DaySelected
        {
            add
            {
                WeekView.DaySelected += value;
            }
            remove
            {
                WeekView.DaySelected -= value;
            }
        }

        public event EventHandler<DayOfWeek> DayUnSelected
        {
            add
            {
                WeekView.DayUnSelected += value;
            }
            remove
            {
                WeekView.DayUnSelected -= value;
            }
        }

        public static readonly NSString Key = new NSString("FullWeekTableViewCell");

        protected FullWeekTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            if (addmoreLabelTap == null)
            {
                addmoreLabelTap = new UITapGestureRecognizer(AddMoreLabel_Tapped);
            }
            AddMoreLabel.RemoveGestureRecognizer(addmoreLabelTap);
            AddMoreLabel.AddGestureRecognizer(addmoreLabelTap);

            AddMoreButton.TouchUpInside -= AddMoreButton_TouchUpInside;
            AddMoreButton.TouchUpInside += AddMoreButton_TouchUpInside;
        }

        public void ConfigureSelected(List<DayOfWeek> selectedDays)
        {
            WeekView.ConfigureSelected(selectedDays);
        }

        void AddMoreLabel_Tapped()
        {
            AddMoreMedicineTapped?.Invoke(this, EventArgs.Empty);
        }

        void AddMoreButton_TouchUpInside(object sender, EventArgs e)
        {
            AddMoreMedicineTapped?.Invoke(this, EventArgs.Empty);
        }

    }
}
