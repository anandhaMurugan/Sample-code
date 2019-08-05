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
    [Register ("ChatDialogCell")]
    partial class ChatDialogCell
    {
        [Outlet]
        Helseboka.iOS.Common.View.LinkableLabel ChatMessage { get; set; }


        [Outlet]
        UIKit.UIView MessageBackgroundView { get; set; }


        [Outlet]
        Helseboka.iOS.Common.View.ChatDateLabel ReceivedMessageDate { get; set; }


        [Outlet]
        Helseboka.iOS.Common.View.ChatDateLabel SentMessageDate { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ChatMessage != null) {
                ChatMessage.Dispose ();
                ChatMessage = null;
            }

            if (MessageBackgroundView != null) {
                MessageBackgroundView.Dispose ();
                MessageBackgroundView = null;
            }

            if (ReceivedMessageDate != null) {
                ReceivedMessageDate.Dispose ();
                ReceivedMessageDate = null;
            }

            if (SentMessageDate != null) {
                SentMessageDate.Dispose ();
                SentMessageDate = null;
            }
        }
    }
}