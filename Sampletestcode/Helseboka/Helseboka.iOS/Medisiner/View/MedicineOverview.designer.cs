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
    [Register ("MedicineOverview")]
    partial class MedicineOverview
    {
        [Outlet]
        Helseboka.iOS.Common.View.PrimaryActionButton AddToMyProfileButton { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint AlarmDetailsBottomConstraint { get; set; }


        [Outlet]
        UIKit.UILabel AlarmDetailsLabel { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint AlarmDetailsTopConstraint { get; set; }


        [Outlet]
        UIKit.UILabel AlarmTextLabel { get; set; }


        [Outlet]
        UIKit.UIView AlarmView { get; set; }


        [Outlet]
        Helseboka.iOS.Common.View.DescriptionLabel MedicineDoseLabel { get; set; }


        [Outlet]
        Helseboka.iOS.Common.View.PageTitleLabel MedicineNameLabel { get; set; }


        [Outlet]
        UIKit.UIView ReadMoreView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton DeleteButton { get; set; }


        [Action ("AddToMyProfile_Tapped:")]
        partial void AddToMyProfile_Tapped (Helseboka.iOS.Common.View.PrimaryActionButton sender);


        [Action ("Back_Tapped:")]
        partial void Back_Tapped (UIKit.UIButton sender);


        [Action ("Delete_Tapped:")]
        partial void Delete_Tapped (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (AddToMyProfileButton != null) {
                AddToMyProfileButton.Dispose ();
                AddToMyProfileButton = null;
            }

            if (AlarmDetailsBottomConstraint != null) {
                AlarmDetailsBottomConstraint.Dispose ();
                AlarmDetailsBottomConstraint = null;
            }

            if (AlarmDetailsLabel != null) {
                AlarmDetailsLabel.Dispose ();
                AlarmDetailsLabel = null;
            }

            if (AlarmDetailsTopConstraint != null) {
                AlarmDetailsTopConstraint.Dispose ();
                AlarmDetailsTopConstraint = null;
            }

            if (AlarmTextLabel != null) {
                AlarmTextLabel.Dispose ();
                AlarmTextLabel = null;
            }

            if (AlarmView != null) {
                AlarmView.Dispose ();
                AlarmView = null;
            }

            if (DeleteButton != null) {
                DeleteButton.Dispose ();
                DeleteButton = null;
            }

            if (MedicineDoseLabel != null) {
                MedicineDoseLabel.Dispose ();
                MedicineDoseLabel = null;
            }

            if (MedicineNameLabel != null) {
                MedicineNameLabel.Dispose ();
                MedicineNameLabel = null;
            }

            if (ReadMoreView != null) {
                ReadMoreView.Dispose ();
                ReadMoreView = null;
            }
        }
    }
}