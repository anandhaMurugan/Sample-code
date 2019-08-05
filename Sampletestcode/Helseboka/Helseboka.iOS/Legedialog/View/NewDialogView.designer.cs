// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Helseboka.iOS.Legedialog.View
{
    [Register ("NewDialogView")]
    partial class NewDialogView
    {
        [Outlet]
        UIKit.UITextView BodyText { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint BodyViewBottomConstraint { get; set; }


        [Outlet]
        Helseboka.iOS.Common.View.ChatMessageLabel DoctorLabel { get; set; }


        [Outlet]
        UIKit.UIActivityIndicatorView DoctorSelectionLoadingIndicator { get; set; }


        [Outlet]
        Helseboka.iOS.Common.View.ChatMessageLabel MedicalCenterLabel { get; set; }


        [Outlet]
        UIKit.UIImageView SelectionDropDownImage { get; set; }


        [Outlet]
        Helseboka.iOS.Common.View.MediumActionButton SendButton { get; set; }


        [Outlet]
        Helseboka.iOS.Common.View.ChatMessageLabel senderLabel { get; set; }


        [Outlet]
        UIKit.UIView SenderSelectionView { get; set; }


        [Outlet]
        UIKit.UIView SenderSeparator { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint SenderSeparatorTopToSenderViewBottomConstraint { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint SenderSeparatorTopToSenderViewSelectionBottomConstraint { get; set; }


        [Outlet]
        UIKit.UIView SenderView { get; set; }


        [Outlet]
        UIKit.UITextField SubjectTextfield { get; set; }


        [Outlet]
        Helseboka.iOS.Common.View.ChatMessageLabel ToTextLabel { get; set; }


        [Action ("Attachment_Tapped:")]
        partial void Attachment_Tapped (UIKit.UIButton sender);


        [Action ("Back_Tapped:")]
        partial void Back_Tapped (UIKit.UIButton sender);


        [Action ("Send_Tapped:")]
        partial void Send_Tapped (UIKit.UIButton sender);


        [Action ("Subject_DidChanged:")]
        partial void Subject_DidChanged (UIKit.UITextField sender);

        void ReleaseDesignerOutlets ()
        {
            if (BodyText != null) {
                BodyText.Dispose ();
                BodyText = null;
            }

            if (BodyViewBottomConstraint != null) {
                BodyViewBottomConstraint.Dispose ();
                BodyViewBottomConstraint = null;
            }

            if (DoctorLabel != null) {
                DoctorLabel.Dispose ();
                DoctorLabel = null;
            }

            if (MedicalCenterLabel != null) {
                MedicalCenterLabel.Dispose ();
                MedicalCenterLabel = null;
            }

            if (SelectionDropDownImage != null) {
                SelectionDropDownImage.Dispose ();
                SelectionDropDownImage = null;
            }

            if (SendButton != null) {
                SendButton.Dispose ();
                SendButton = null;
            }

            if (senderLabel != null) {
                senderLabel.Dispose ();
                senderLabel = null;
            }

            if (SenderSelectionView != null) {
                SenderSelectionView.Dispose ();
                SenderSelectionView = null;
            }

            if (SenderSeparator != null) {
                SenderSeparator.Dispose ();
                SenderSeparator = null;
            }

            if (SenderSeparatorTopToSenderViewBottomConstraint != null) {
                SenderSeparatorTopToSenderViewBottomConstraint.Dispose ();
                SenderSeparatorTopToSenderViewBottomConstraint = null;
            }

            if (SenderSeparatorTopToSenderViewSelectionBottomConstraint != null) {
                SenderSeparatorTopToSenderViewSelectionBottomConstraint.Dispose ();
                SenderSeparatorTopToSenderViewSelectionBottomConstraint = null;
            }

            if (SenderView != null) {
                SenderView.Dispose ();
                SenderView = null;
            }

            if (SubjectTextfield != null) {
                SubjectTextfield.Dispose ();
                SubjectTextfield = null;
            }
        }
    }
}