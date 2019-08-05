// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Helseboka.iOS.Legetimer.View.TableViewCell
{
    [Register ("UpcomingLegetimerTableCell")]
    partial class UpcomingLegetimerTableCell
    {
        [Outlet]
        UIKit.UILabel AppointmentRequestTime { get; set; }


        [Outlet]
        UIKit.UIView BottomLine { get; set; }


        [Outlet]
        UIKit.UIView CircleView { get; set; }


        [Outlet]
        UIKit.UIImageView StatusImage { get; set; }


        [Outlet]
        UIKit.UIView TopLine { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView AppointmentDetailsView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton CancelButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel dateLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel descriptionLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel timeLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView VideoCallButton { get; set; }


        [Action ("Cancel_Tapped:")]
        partial void Cancel_Tapped (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (AppointmentDetailsView != null) {
                AppointmentDetailsView.Dispose ();
                AppointmentDetailsView = null;
            }

            if (AppointmentRequestTime != null) {
                AppointmentRequestTime.Dispose ();
                AppointmentRequestTime = null;
            }

            if (BottomLine != null) {
                BottomLine.Dispose ();
                BottomLine = null;
            }

            if (CancelButton != null) {
                CancelButton.Dispose ();
                CancelButton = null;
            }

            if (CircleView != null) {
                CircleView.Dispose ();
                CircleView = null;
            }

            if (dateLabel != null) {
                dateLabel.Dispose ();
                dateLabel = null;
            }

            if (descriptionLabel != null) {
                descriptionLabel.Dispose ();
                descriptionLabel = null;
            }

            if (StatusImage != null) {
                StatusImage.Dispose ();
                StatusImage = null;
            }

            if (timeLabel != null) {
                timeLabel.Dispose ();
                timeLabel = null;
            }

            if (TopLine != null) {
                TopLine.Dispose ();
                TopLine = null;
            }

            if (VideoCallButton != null) {
                VideoCallButton.Dispose ();
                VideoCallButton = null;
            }
        }
    }
}