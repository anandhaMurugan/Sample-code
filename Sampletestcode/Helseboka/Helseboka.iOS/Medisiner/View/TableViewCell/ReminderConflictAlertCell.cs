using System;
using UIKit;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Helseboka.Core.Common.Extension;
using Helseboka.Core.Common.Constant;
using Helseboka.Core.Common.EnumDefinitions;
using Helseboka.iOS.Common.Extension;
using Helseboka.iOS.Common.Constant;
using Helseboka.iOS.Common.View;
using Helseboka.Core.Common.Interfaces;
using Foundation;
using CoreGraphics;
using Helseboka.iOS.Common.TableViewCell;
using Helseboka.Core.MedicineModule.Model;
using Helseboka.Core.Common.Model;

namespace Helseboka.iOS.Medisiner.View.TableViewCell
{
    public partial class ReminderConflictAlertCell : BaseTableViewCell
    {
        public static readonly NSString Key = new NSString("ReminderConflictAlertCell");

        protected ReminderConflictAlertCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public void Configure(MedicineReminder medicineReminder)
        {
            var atttibuted = new NSMutableAttributedString();
            atttibuted.Append(new NSAttributedString(medicineReminder.Medicine.Name, Fonts.GetBoldFont(18), Colors.LoginHelpTextColor));
            atttibuted.Append(new NSAttributedString($" {medicineReminder.Medicine.Strength}, {medicineReminder.Medicine.Form}", Fonts.GetNormalFont(14), Colors.LoginHelpTextColor));

            MedicineNameLabel.AttributedText = atttibuted;

            if (medicineReminder.Reminder != null)
            {
                var days = medicineReminder.Reminder.Days.Select(x => x.ToString().Translate()).ToList();
                var reminderText = $" {"General.View.TimePrefix".Translate()} {String.Join(", ", medicineReminder.Reminder.FrequencyPerDay)}";

                var attributedText = new NSMutableAttributedString();
                attributedText.Append(new NSAttributedString(String.Join(", ", days), Fonts.GetMediumFont(15), Colors.DateSelectionLabelTextColor));
                attributedText.Append(new NSAttributedString(reminderText, Fonts.GetMediumFont(15), Colors.TitleTextColor));

                ReminderLabel.AttributedText = attributedText;
            }
        }
    }
}
