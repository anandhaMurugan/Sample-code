// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Helseboka.iOS.Dashboard.View
{
    [Register ("DashboardHomeView")]
    partial class DashboardHomeView
    {
        [Outlet]
        UIKit.UIActivityIndicatorView AppointmentLoadingIndicator { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint BackgroundToHeaderConstraint { get; set; }


        [Outlet]
        UIKit.UILabel DateLabel { get; set; }


        [Outlet]
        UIKit.UIView HeaderView { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint HeaderViewHeightConstraint { get; set; }


        [Outlet]
        UIKit.UIActivityIndicatorView MedicineLoadingIndicator { get; set; }


        [Outlet]
        UIKit.UIView MedicineNoDataCenterView { get; set; }


        [Outlet]
        UIKit.UIView MedicineNoDataView { get; set; }


        [Outlet]
        UIKit.UIView MedicineView { get; set; }


        [Outlet]
        UIKit.UILabel NameLabel { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint NameLeadingConstraint { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint NameTopConstraint { get; set; }


        [Outlet]
        Helseboka.iOS.Common.View.BaseUILabel NextAppointmentAlertLabel { get; set; }


        [Outlet]
        UIKit.UILabel NextAppointmentDateLabel { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint NextAppointmentDateLabelBottomToTitleTopConstraint { get; set; }


        [Outlet]
        UIKit.UILabel NextAppointmentHeaderLabel { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint NextAppointmentTitleBottomToAlertTopConstraint { get; set; }


        [Outlet]
        UIKit.UILabel NextAppointmentTitleLabel { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint NextAppointmentTitleToBottomConstraint { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint NextAppointmentTitleToTopConstraint { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint NextAppointmentViewBottomConstraint { get; set; }


        [Outlet]
        UIKit.UILabel NextMedicineLabel { get; set; }


        [Outlet]
        UIKit.UIView NoDataCenterView { get; set; }


        [Outlet]
        UIKit.UIView NoDataLeftView { get; set; }


        [Outlet]
        UIKit.UIView NoDataRightView { get; set; }


        [Outlet]
        UIKit.UILabel WelcomeLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView NextAppointmentView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView VideoCallButton { get; set; }


        [Action ("NextDate_Tapped:")]
        partial void NextDate_Tapped (UIKit.UIButton sender);


        [Action ("PreviousDate_Tapped:")]
        partial void PreviousDate_Tapped (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (AppointmentLoadingIndicator != null) {
                AppointmentLoadingIndicator.Dispose ();
                AppointmentLoadingIndicator = null;
            }

            if (BackgroundToHeaderConstraint != null) {
                BackgroundToHeaderConstraint.Dispose ();
                BackgroundToHeaderConstraint = null;
            }

            if (DateLabel != null) {
                DateLabel.Dispose ();
                DateLabel = null;
            }

            if (HeaderView != null) {
                HeaderView.Dispose ();
                HeaderView = null;
            }

            if (HeaderViewHeightConstraint != null) {
                HeaderViewHeightConstraint.Dispose ();
                HeaderViewHeightConstraint = null;
            }

            if (MedicineLoadingIndicator != null) {
                MedicineLoadingIndicator.Dispose ();
                MedicineLoadingIndicator = null;
            }

            if (MedicineNoDataCenterView != null) {
                MedicineNoDataCenterView.Dispose ();
                MedicineNoDataCenterView = null;
            }

            if (MedicineNoDataView != null) {
                MedicineNoDataView.Dispose ();
                MedicineNoDataView = null;
            }

            if (MedicineView != null) {
                MedicineView.Dispose ();
                MedicineView = null;
            }

            if (NameLabel != null) {
                NameLabel.Dispose ();
                NameLabel = null;
            }

            if (NameLeadingConstraint != null) {
                NameLeadingConstraint.Dispose ();
                NameLeadingConstraint = null;
            }

            if (NameTopConstraint != null) {
                NameTopConstraint.Dispose ();
                NameTopConstraint = null;
            }

            if (NextAppointmentAlertLabel != null) {
                NextAppointmentAlertLabel.Dispose ();
                NextAppointmentAlertLabel = null;
            }

            if (NextAppointmentDateLabel != null) {
                NextAppointmentDateLabel.Dispose ();
                NextAppointmentDateLabel = null;
            }

            if (NextAppointmentDateLabelBottomToTitleTopConstraint != null) {
                NextAppointmentDateLabelBottomToTitleTopConstraint.Dispose ();
                NextAppointmentDateLabelBottomToTitleTopConstraint = null;
            }

            if (NextAppointmentTitleBottomToAlertTopConstraint != null) {
                NextAppointmentTitleBottomToAlertTopConstraint.Dispose ();
                NextAppointmentTitleBottomToAlertTopConstraint = null;
            }

            if (NextAppointmentTitleLabel != null) {
                NextAppointmentTitleLabel.Dispose ();
                NextAppointmentTitleLabel = null;
            }

            if (NextAppointmentTitleToBottomConstraint != null) {
                NextAppointmentTitleToBottomConstraint.Dispose ();
                NextAppointmentTitleToBottomConstraint = null;
            }

            if (NextAppointmentTitleToTopConstraint != null) {
                NextAppointmentTitleToTopConstraint.Dispose ();
                NextAppointmentTitleToTopConstraint = null;
            }

            if (NextAppointmentView != null) {
                NextAppointmentView.Dispose ();
                NextAppointmentView = null;
            }

            if (NextAppointmentViewBottomConstraint != null) {
                NextAppointmentViewBottomConstraint.Dispose ();
                NextAppointmentViewBottomConstraint = null;
            }

            if (NextMedicineLabel != null) {
                NextMedicineLabel.Dispose ();
                NextMedicineLabel = null;
            }

            if (NoDataCenterView != null) {
                NoDataCenterView.Dispose ();
                NoDataCenterView = null;
            }

            if (NoDataLeftView != null) {
                NoDataLeftView.Dispose ();
                NoDataLeftView = null;
            }

            if (NoDataRightView != null) {
                NoDataRightView.Dispose ();
                NoDataRightView = null;
            }

            if (VideoCallButton != null) {
                VideoCallButton.Dispose ();
                VideoCallButton = null;
            }

            if (WelcomeLabel != null) {
                WelcomeLabel.Dispose ();
                WelcomeLabel = null;
            }
        }
    }
}