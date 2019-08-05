// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Helseboka.iOS.Medisiner.View
{
    [Register ("ConflictReminderDialog")]
    partial class ConflictReminderDialog
    {
        [Outlet]
        Helseboka.iOS.Common.View.MediumActionButton CancelButton { get; set; }


        [Outlet]
        Helseboka.iOS.Common.View.BaseTableView MedicineListTableView { get; set; }


        [Outlet]
        Helseboka.iOS.Common.View.MediumActionButton OkButton { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint TableViewHeightConstraint { get; set; }


        [Action ("Cancel_Tapped:")]
        partial void Cancel_Tapped (Helseboka.iOS.Common.View.MediumActionButton sender);


        [Action ("Close_Tapped:")]
        partial void Close_Tapped (UIKit.UIButton sender);


        [Action ("Ok_Tapped:")]
        partial void Ok_Tapped (Helseboka.iOS.Common.View.MediumActionButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (CancelButton != null) {
                CancelButton.Dispose ();
                CancelButton = null;
            }

            if (MedicineListTableView != null) {
                MedicineListTableView.Dispose ();
                MedicineListTableView = null;
            }

            if (OkButton != null) {
                OkButton.Dispose ();
                OkButton = null;
            }

            if (TableViewHeightConstraint != null) {
                TableViewHeightConstraint.Dispose ();
                TableViewHeightConstraint = null;
            }
        }
    }
}