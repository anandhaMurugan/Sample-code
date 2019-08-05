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
    [Register ("BankIdView")]
    partial class BankIdView
    {
        [Outlet]
        UIKit.NSLayoutConstraint BankIdMobilTopConstraint { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint BankIdTopConstraint { get; set; }


        [Outlet]
        UIKit.UIButton CancelButton { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint ContainerBottomConstraint { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint ContainerTopConstraint { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint TitleTopConstraint { get; set; }


        [Action ("BankID_Tapped:")]
        partial void BankID_Tapped (Helseboka.iOS.Common.View.PrimaryActionButton sender);


        [Action ("BankIDMobile_Tapped:")]
        partial void BankIDMobile_Tapped (Helseboka.iOS.Common.View.PrimaryActionButton sender);


        [Action ("Cancel_Tapped:")]
        partial void Cancel_Tapped (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (BankIdMobilTopConstraint != null) {
                BankIdMobilTopConstraint.Dispose ();
                BankIdMobilTopConstraint = null;
            }

            if (BankIdTopConstraint != null) {
                BankIdTopConstraint.Dispose ();
                BankIdTopConstraint = null;
            }

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

            if (TitleTopConstraint != null) {
                TitleTopConstraint.Dispose ();
                TitleTopConstraint = null;
            }
        }
    }
}