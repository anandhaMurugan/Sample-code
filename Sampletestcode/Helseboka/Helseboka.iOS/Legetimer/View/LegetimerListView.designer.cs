// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Helseboka.iOS.Legetimer.View
{
    [Register ("LegetimerListView")]
    partial class LegetimerListView
    {
        [Outlet]
        Helseboka.iOS.Common.View.BaseTableView AppointmentListView { get; set; }


        [Outlet]
        UIKit.UITextView HelpTextView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint HelpTextViewBottomConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint ListViewBottomConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Helseboka.iOS.Common.View.PrimaryActionButton VideoLinkButton { get; set; }


        [Action ("NewAppointment_Tapped:")]
        partial void NewAppointment_Tapped (Helseboka.iOS.Common.View.PrimaryActionButton sender);

        [Action ("VideoConsultation_Tapped:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void VideoConsultation_Tapped (Helseboka.iOS.Common.View.PrimaryActionButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (AppointmentListView != null) {
                AppointmentListView.Dispose ();
                AppointmentListView = null;
            }

            if (HelpTextView != null) {
                HelpTextView.Dispose ();
                HelpTextView = null;
            }

            if (HelpTextViewBottomConstraint != null) {
                HelpTextViewBottomConstraint.Dispose ();
                HelpTextViewBottomConstraint = null;
            }

            if (ListViewBottomConstraint != null) {
                ListViewBottomConstraint.Dispose ();
                ListViewBottomConstraint = null;
            }

            if (VideoLinkButton != null) {
                VideoLinkButton.Dispose ();
                VideoLinkButton = null;
            }
        }
    }
}