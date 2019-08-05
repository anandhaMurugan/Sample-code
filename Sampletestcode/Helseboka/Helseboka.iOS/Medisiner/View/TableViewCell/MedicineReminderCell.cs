using System;
using System.Linq;
using Foundation;
using Helseboka.Core.MedicineModule.Model;
using Helseboka.iOS.Common.Constant;
using Helseboka.iOS.Common.TableViewCell;
using Helseboka.iOS.Common.Extension;
using UIKit;
using Helseboka.Core.Common.Extension;
using Helseboka.Core.Common.EnumDefinitions;
using System.Collections.Generic;
using Helseboka.Core.Common.Model;

namespace Helseboka.iOS.Medisiner.View.TableViewCell
{
    public class MedicineSelectionEventArgs : EventArgs
    {
        public MedicineInfo Medicine { get; private set; }
        public Boolean IsSelected { get; private set; }

        public MedicineSelectionEventArgs(MedicineInfo medicine, Boolean isSelected)
        {
            Medicine = medicine;
            IsSelected = isSelected;
        }
    }

    public partial class MedicineReminderCell : BaseTableViewCell
    {
        private SelectableEntity<MedicineReminder> selectableReminder;
        public static readonly NSString Key = new NSString("MedicineReminderCell");

        public event EventHandler<MedicineSelectionEventArgs> SelectionChanged;

        protected MedicineReminderCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public void Configure(SelectableEntity<MedicineReminder> selectableReminder, bool isEditing = false)
        {
            this.selectableReminder = selectableReminder;
            UpdateDesignForNormalState();

            if (isEditing)
            {
                SelectCheckBox.Hidden = false;
                ViewLeadingConstraintToCheckBox.Active = true;
                ViewLeadingConstraint.Active = false;

                SelectCheckBox.SelectionChanged -= SelectCheckBox_SelectionChanged;
                SelectCheckBox.Selected = selectableReminder.IsSelected;
                SelectCheckBox.SelectionChanged += SelectCheckBox_SelectionChanged;
            }
            else
            {
                SelectCheckBox.Hidden = true;
                ViewLeadingConstraint.Active = true;
                ViewLeadingConstraintToCheckBox.Active = false;
            }

            var atttibuted = new NSMutableAttributedString();
            atttibuted.Append(new NSAttributedString(selectableReminder.Entity.Medicine.Name, Fonts.GetBoldFont(18), Colors.LoginHelpTextColor));
            atttibuted.Append(new NSAttributedString($" {selectableReminder.Entity.Medicine.Strength}", Fonts.GetNormalFont(14), Colors.LoginHelpTextColor));

            MedicineNameLabel.AttributedText = atttibuted;

            var reminder = selectableReminder.Entity.Reminder;

            if (reminder != null)
            {
                List<String> frequency = new List<string>();
                var attributedText = new NSMutableAttributedString();

                attributedText.Append(new NSAttributedString($"{"General.View.Next".Translate()}: ", Fonts.GetMediumFont(15), Colors.DateSelectionLabelTextColor));
                String dayText;
                if (reminder.NextReminderDay.GetDay() == Day.Today)
                {
                    dayText = "Today".Translate();
                    frequency = reminder.GetNextFrequencies();
                }
                else if (reminder.NextReminderDay.GetDay() == Day.Tomorrow)
                {
                    dayText = "Tomorrow".Translate();
                }
                else
                {
                    dayText = reminder.NextReminderDay.ToString().Translate();
                }

                attributedText.Append(new NSAttributedString(dayText, Fonts.GetMediumFont(15), Colors.DateSelectionLabelTextColor));
                attributedText.Append(new NSAttributedString( $" {"General.View.TimePrefix".Translate()} ", Fonts.GetMediumFont(15), Colors.TitleTextColor));

                if (frequency.Count < 1)
                {
                    frequency = reminder.FrequencyPerDay;
                }

                var alarmText = String.Join(", ", frequency);
                attributedText.Append(new NSAttributedString(alarmText, Fonts.GetMediumFont(15), UIColor.Black));

                MedicineReminderLabel.AttributedText = attributedText;

                AlarmImageView.Hidden = false;
                MedicineReminderLabel.Hidden = false;
            }
            else
            {
                AlarmImageView.Hidden = true;
                MedicineReminderLabel.Hidden = true;
            }
        }

        private void UpdateDesignForNormalState()
        {
            MedicineView.BackgroundColor = UIColor.White;
            MedicineView.AddBorder(UIColor.White, 20);
            MedicineView.AddShadow(UIColor.FromRGB((byte)175, (byte)175, (byte)175), 4, 0, 0);
        }

        private void UpdateDesignForSelectedState()
        {
            MedicineView.BackgroundColor = UIColor.FromRGB((byte)231, (byte)255, (byte)255);
            MedicineView.AddBorder(UIColor.FromRGB((byte)226, (byte)226, (byte)226), 20);
            MedicineView.AddShadow(UIColor.FromRGB((byte)210, (byte)210, (byte)210), 4, 2, 0);
        }

        public void ToggleSelection()
        {
            SelectCheckBox.Selected = !SelectCheckBox.Selected;
            SelectCheckBox_SelectionChanged(SelectCheckBox, SelectCheckBox.Selected);
        }

        private void SelectCheckBox_SelectionChanged(object sender, bool e)
        {
            if(e)
            {
                UpdateDesignForSelectedState();
            }
            else
            {
                UpdateDesignForNormalState();
            }
            selectableReminder.IsSelected = e;
            SelectionChanged?.Invoke(this, new MedicineSelectionEventArgs(selectableReminder.Entity.Medicine, e));
        }

    }
}
