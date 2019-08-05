// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Helseboka.iOS
{
    [Register ("MobilePhoneNumbers")]
    partial class MobilePhoneNumbers
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView BoxView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel DescriptionText { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView DialogView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField EditTextBox { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ErrorMessageText { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ErrorText { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Helseboka.iOS.Common.View.MediumActionButton OkButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint OkButtonBottomConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint PopupBottomConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel PopupTitle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint PopupTopConstraint { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (BoxView != null) {
                BoxView.Dispose ();
                BoxView = null;
            }

            if (DescriptionText != null) {
                DescriptionText.Dispose ();
                DescriptionText = null;
            }

            if (DialogView != null) {
                DialogView.Dispose ();
                DialogView = null;
            }

            if (EditTextBox != null) {
                EditTextBox.Dispose ();
                EditTextBox = null;
            }

            if (ErrorMessageText != null) {
                ErrorMessageText.Dispose ();
                ErrorMessageText = null;
            }

            if (ErrorText != null) {
                ErrorText.Dispose ();
                ErrorText = null;
            }

            if (OkButton != null) {
                OkButton.Dispose ();
                OkButton = null;
            }

            if (OkButtonBottomConstraint != null) {
                OkButtonBottomConstraint.Dispose ();
                OkButtonBottomConstraint = null;
            }

            if (PopupBottomConstraint != null) {
                PopupBottomConstraint.Dispose ();
                PopupBottomConstraint = null;
            }

            if (PopupTitle != null) {
                PopupTitle.Dispose ();
                PopupTitle = null;
            }

            if (PopupTopConstraint != null) {
                PopupTopConstraint.Dispose ();
                PopupTopConstraint = null;
            }
        }
    }
}