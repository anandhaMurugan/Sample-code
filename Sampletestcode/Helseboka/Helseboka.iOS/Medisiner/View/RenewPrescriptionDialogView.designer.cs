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
    [Register ("RenewPrescriptionDialogView")]
    partial class RenewPrescriptionDialogView
    {
        [Outlet]
        UIKit.UILabel MedicineNameLabel { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint ScrollViewHeightConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView ScrollView { get; set; }


        [Action ("Close_Tapped:")]
        partial void Close_Tapped (UIKit.UIButton sender);


        [Action ("Send_Tapped:")]
        partial void Send_Tapped (Helseboka.iOS.Common.View.MediumActionButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (MedicineNameLabel != null) {
                MedicineNameLabel.Dispose ();
                MedicineNameLabel = null;
            }

            if (ScrollView != null) {
                ScrollView.Dispose ();
                ScrollView = null;
            }

            if (ScrollViewHeightConstraint != null) {
                ScrollViewHeightConstraint.Dispose ();
                ScrollViewHeightConstraint = null;
            }
        }
    }
}