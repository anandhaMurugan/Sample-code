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
    [Register ("MedicineReminderCell")]
    partial class MedicineReminderCell
    {
        [Outlet]
        UIKit.UIImageView AlarmImageView { get; set; }


        [Outlet]
        UIKit.UIImageView CellBackgroundImage { get; set; }


        [Outlet]
        UIKit.UILabel MedicineNameLabel { get; set; }


        [Outlet]
        UIKit.UILabel MedicineReminderLabel { get; set; }


        [Outlet]
        UIKit.UIView MedicineView { get; set; }


        [Outlet]
        Helseboka.iOS.Common.View.CheckBox SelectCheckBox { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint ViewLeadingConstraint { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint ViewLeadingConstraintToCheckBox { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (AlarmImageView != null) {
                AlarmImageView.Dispose ();
                AlarmImageView = null;
            }

            if (MedicineNameLabel != null) {
                MedicineNameLabel.Dispose ();
                MedicineNameLabel = null;
            }

            if (MedicineReminderLabel != null) {
                MedicineReminderLabel.Dispose ();
                MedicineReminderLabel = null;
            }

            if (MedicineView != null) {
                MedicineView.Dispose ();
                MedicineView = null;
            }

            if (SelectCheckBox != null) {
                SelectCheckBox.Dispose ();
                SelectCheckBox = null;
            }

            if (ViewLeadingConstraint != null) {
                ViewLeadingConstraint.Dispose ();
                ViewLeadingConstraint = null;
            }

            if (ViewLeadingConstraintToCheckBox != null) {
                ViewLeadingConstraintToCheckBox.Dispose ();
                ViewLeadingConstraintToCheckBox = null;
            }
        }
    }
}