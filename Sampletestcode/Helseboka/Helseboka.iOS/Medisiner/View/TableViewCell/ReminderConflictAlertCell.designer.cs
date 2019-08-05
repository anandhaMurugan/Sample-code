// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Helseboka.iOS.Medisiner.View.TableViewCell
{
    [Register ("ReminderConflictAlertCell")]
    partial class ReminderConflictAlertCell
    {
        [Outlet]
        Helseboka.iOS.Common.View.BaseUILabel MedicineNameLabel { get; set; }


        [Outlet]
        UIKit.UILabel ReminderLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (MedicineNameLabel != null) {
                MedicineNameLabel.Dispose ();
                MedicineNameLabel = null;
            }

            if (ReminderLabel != null) {
                ReminderLabel.Dispose ();
                ReminderLabel = null;
            }
        }
    }
}