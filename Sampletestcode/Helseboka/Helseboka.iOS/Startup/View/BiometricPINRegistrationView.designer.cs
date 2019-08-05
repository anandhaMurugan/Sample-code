// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Helseboka.iOS.Startup.View
{
    [Register ("BiometricPINRegistrationView")]
    partial class BiometricPINRegistrationView
    {
        [Outlet]
        UIKit.NSLayoutConstraint ContainerBottomConstraint { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint ContainerTopConstraint { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint PINHeightConstraint { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint PINWidthConstraint { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint TitleTopConstraint { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint TouchIdButtonWidthConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Helseboka.iOS.Common.View.SignUpSecondaryActionButton CancelButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Helseboka.iOS.Common.View.BasePrimaryActionButton PINButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Helseboka.iOS.Common.View.BasePrimaryActionButton TouchID { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint TouchIDBottomConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint TouchIDButtonHeight { get; set; }


        [Action ("PIN_tapped:")]
        partial void PIN_tapped (Helseboka.iOS.Common.View.BasePrimaryActionButton sender);


        [Action ("TouchID_Tapped:")]
        partial void TouchID_Tapped (Helseboka.iOS.Common.View.BasePrimaryActionButton sender);

        [Action ("Cancel_Tapped:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void Cancel_Tapped (Helseboka.iOS.Common.View.SignUpSecondaryActionButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (CancelButton != null) {
                CancelButton.Dispose ();
                CancelButton = null;
            }

            if (ContainerBottomConstraint != null) {
                ContainerBottomConstraint.Dispose ();
                ContainerBottomConstraint = null;
            }

            if (ContainerTopConstraint != null) {
                ContainerTopConstraint.Dispose ();
                ContainerTopConstraint = null;
            }

            if (PINButton != null) {
                PINButton.Dispose ();
                PINButton = null;
            }

            if (PINHeightConstraint != null) {
                PINHeightConstraint.Dispose ();
                PINHeightConstraint = null;
            }

            if (PINWidthConstraint != null) {
                PINWidthConstraint.Dispose ();
                PINWidthConstraint = null;
            }

            if (TitleTopConstraint != null) {
                TitleTopConstraint.Dispose ();
                TitleTopConstraint = null;
            }

            if (TouchID != null) {
                TouchID.Dispose ();
                TouchID = null;
            }

            if (TouchIDBottomConstraint != null) {
                TouchIDBottomConstraint.Dispose ();
                TouchIDBottomConstraint = null;
            }

            if (TouchIDButtonHeight != null) {
                TouchIDButtonHeight.Dispose ();
                TouchIDButtonHeight = null;
            }

            if (TouchIdButtonWidthConstraint != null) {
                TouchIdButtonWidthConstraint.Dispose ();
                TouchIdButtonWidthConstraint = null;
            }
        }
    }
}