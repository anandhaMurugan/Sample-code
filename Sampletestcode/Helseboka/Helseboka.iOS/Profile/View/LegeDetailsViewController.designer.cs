// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Helseboka.iOS.Profile.View
{
    [Register ("LegeDetailsViewController")]
    partial class LegeDetailsViewController
    {
        [Outlet]
        UIKit.UILabel DoctorNameLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel AddressLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton backButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel DoctorOfficeLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel MobileLabel { get; set; }


        [Action ("BackButton_TouchUpInside:")]
        partial void BackButton_TouchUpInside (UIKit.UIButton sender);


        [Action ("ChangeDoctor_Tapped:")]
        partial void ChangeDoctor_Tapped (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (AddressLabel != null) {
                AddressLabel.Dispose ();
                AddressLabel = null;
            }

            if (backButton != null) {
                backButton.Dispose ();
                backButton = null;
            }

            if (DoctorNameLabel != null) {
                DoctorNameLabel.Dispose ();
                DoctorNameLabel = null;
            }

            if (DoctorOfficeLabel != null) {
                DoctorOfficeLabel.Dispose ();
                DoctorOfficeLabel = null;
            }

            if (MobileLabel != null) {
                MobileLabel.Dispose ();
                MobileLabel = null;
            }
        }
    }
}