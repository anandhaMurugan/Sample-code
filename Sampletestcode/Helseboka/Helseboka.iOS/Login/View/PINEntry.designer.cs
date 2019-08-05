// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Helseboka.iOS.Login.View
{
    [Register ("PINEntry")]
    partial class PINEntry
    {
        [Outlet]
        UIKit.UIActivityIndicatorView ActivityIndicator { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint EnterPINLabelTopConstraint { get; set; }


        [Outlet]
        UIKit.UILabel ErrorLabel { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint ErrorLabelTopConstraint { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint ForgotPINTopConstraint { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint LoginContainerLeadingConstraint { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint LoginContainerTopConstraint { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint LoginContainerTrailingConstraint { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint PINViewTopConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Helseboka.iOS.Common.View.CircularPinView PINView { get; set; }


        [Action ("ForgotPIN_Tapped:")]
        partial void ForgotPIN_Tapped (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (ActivityIndicator != null) {
                ActivityIndicator.Dispose ();
                ActivityIndicator = null;
            }

            if (EnterPINLabelTopConstraint != null) {
                EnterPINLabelTopConstraint.Dispose ();
                EnterPINLabelTopConstraint = null;
            }

            if (ErrorLabel != null) {
                ErrorLabel.Dispose ();
                ErrorLabel = null;
            }

            if (ErrorLabelTopConstraint != null) {
                ErrorLabelTopConstraint.Dispose ();
                ErrorLabelTopConstraint = null;
            }

            if (ForgotPINTopConstraint != null) {
                ForgotPINTopConstraint.Dispose ();
                ForgotPINTopConstraint = null;
            }

            if (LoginContainerLeadingConstraint != null) {
                LoginContainerLeadingConstraint.Dispose ();
                LoginContainerLeadingConstraint = null;
            }

            if (LoginContainerTopConstraint != null) {
                LoginContainerTopConstraint.Dispose ();
                LoginContainerTopConstraint = null;
            }

            if (LoginContainerTrailingConstraint != null) {
                LoginContainerTrailingConstraint.Dispose ();
                LoginContainerTrailingConstraint = null;
            }

            if (PINView != null) {
                PINView.Dispose ();
                PINView = null;
            }

            if (PINViewTopConstraint != null) {
                PINViewTopConstraint.Dispose ();
                PINViewTopConstraint = null;
            }
        }
    }
}