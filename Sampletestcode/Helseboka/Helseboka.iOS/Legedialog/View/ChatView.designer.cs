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
    [Register ("ChatView")]
    partial class ChatView
    {
        [Outlet]
        UIKit.UITableView ChatDialogTableView { get; set; }


        [Outlet]
        Helseboka.iOS.Common.View.ChatTitleLabel ChatPageTitleLabel { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint ComposeMessageViewBottomConstraint { get; set; }


        [Outlet]
        UIKit.UITableView DataTableView { get; set; }


        [Outlet]
        UIKit.UITextField MessageComposeField { get; set; }


        [Outlet]
        Helseboka.iOS.Common.View.SmallActionButton SendButton { get; set; }


        [Action ("Attachment_Tapped:")]
        partial void Attachment_Tapped (UIKit.UIButton sender);


        [Action ("Back_Tapped:")]
        partial void Back_Tapped (UIKit.UIButton sender);


        [Action ("MessageEditing_Changed:forEvent:")]
        partial void MessageEditing_Changed (UIKit.UITextField sender, UIKit.UIEvent @event);


        [Action ("MessageSend_Tapped:")]
        partial void MessageSend_Tapped (Helseboka.iOS.Common.View.SmallActionButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (ChatDialogTableView != null) {
                ChatDialogTableView.Dispose ();
                ChatDialogTableView = null;
            }

            if (ChatPageTitleLabel != null) {
                ChatPageTitleLabel.Dispose ();
                ChatPageTitleLabel = null;
            }

            if (ComposeMessageViewBottomConstraint != null) {
                ComposeMessageViewBottomConstraint.Dispose ();
                ComposeMessageViewBottomConstraint = null;
            }

            if (DataTableView != null) {
                DataTableView.Dispose ();
                DataTableView = null;
            }

            if (MessageComposeField != null) {
                MessageComposeField.Dispose ();
                MessageComposeField = null;
            }

            if (SendButton != null) {
                SendButton.Dispose ();
                SendButton = null;
            }
        }
    }
}