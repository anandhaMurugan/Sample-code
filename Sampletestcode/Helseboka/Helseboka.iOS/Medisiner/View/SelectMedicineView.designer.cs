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
    [Register ("SelectMedicineView")]
    partial class SelectMedicineView
    {
        [Outlet]
        Helseboka.iOS.Common.View.BaseTableView MedicineListTableView { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint TableViewHeightConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel DialogTitleLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Helseboka.iOS.Common.View.MediumActionButton OkButton { get; set; }


        [Action ("Close_Tapped:")]
        partial void Close_Tapped (UIKit.UIButton sender);

        [Action ("Ok_Tapped:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void Ok_Tapped (Helseboka.iOS.Common.View.MediumActionButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (DialogTitleLabel != null) {
                DialogTitleLabel.Dispose ();
                DialogTitleLabel = null;
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