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
    [Register ("MedisinListView")]
    partial class MedisinListView
    {
        [Outlet]
        UIKit.UIButton HelpButton { get; set; }


        [Outlet]
        Helseboka.iOS.Common.View.BaseTableView MedicineListTableView { get; set; }


        [Outlet]
        UIKit.UIView NoDataContainerView { get; set; }


        [Outlet]
        Helseboka.iOS.Common.View.PrimaryActionButton RenewPrescriptionButton { get; set; }


        [Outlet]
        Helseboka.iOS.Common.View.BaseTableView SearchTableView { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint SearchTableViewBottomConstraint { get; set; }


        [Outlet]
        UIKit.UITextField SearchTextField { get; set; }


        [Outlet]
        UIKit.UIView SearchView { get; set; }


        [Action ("Help_Tapped:")]
        partial void Help_Tapped (UIKit.UIButton sender);


        [Action ("Renew_Tapped:")]
        partial void Renew_Tapped (Helseboka.iOS.Common.View.PrimaryActionButton sender);


        [Action ("SearchText_Changed:forEvent:")]
        partial void SearchText_Changed (UIKit.UITextField sender, UIKit.UIEvent @event);

        void ReleaseDesignerOutlets ()
        {
            if (HelpButton != null) {
                HelpButton.Dispose ();
                HelpButton = null;
            }

            if (MedicineListTableView != null) {
                MedicineListTableView.Dispose ();
                MedicineListTableView = null;
            }

            if (NoDataContainerView != null) {
                NoDataContainerView.Dispose ();
                NoDataContainerView = null;
            }

            if (RenewPrescriptionButton != null) {
                RenewPrescriptionButton.Dispose ();
                RenewPrescriptionButton = null;
            }

            if (SearchTableView != null) {
                SearchTableView.Dispose ();
                SearchTableView = null;
            }

            if (SearchTableViewBottomConstraint != null) {
                SearchTableViewBottomConstraint.Dispose ();
                SearchTableViewBottomConstraint = null;
            }

            if (SearchTextField != null) {
                SearchTextField.Dispose ();
                SearchTextField = null;
            }

            if (SearchView != null) {
                SearchView.Dispose ();
                SearchView = null;
            }
        }
    }
}