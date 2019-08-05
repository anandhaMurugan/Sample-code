// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Helseboka.iOS.Legetimer.View
{
    [Register ("AppointmentDetailsView")]
    partial class AppointmentDetailsView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel AppointmentDateLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton CancelButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView DataTableView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel DoctorNameLabel { get; set; }


        [Action ("Back_Pressed:")]
        partial void Back_Pressed (UIKit.UIButton sender);


        [Action ("Cancel_Tapped:")]
        partial void Cancel_Tapped (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (AppointmentDateLabel != null) {
                AppointmentDateLabel.Dispose ();
                AppointmentDateLabel = null;
            }

            if (CancelButton != null) {
                CancelButton.Dispose ();
                CancelButton = null;
            }

            if (DataTableView != null) {
                DataTableView.Dispose ();
                DataTableView = null;
            }

            if (DoctorNameLabel != null) {
                DoctorNameLabel.Dispose ();
                DoctorNameLabel = null;
            }
        }
    }
}