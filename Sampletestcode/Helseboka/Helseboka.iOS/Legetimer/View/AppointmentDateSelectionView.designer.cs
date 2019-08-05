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
    [Register ("AppointmentDateSelectionView")]
    partial class AppointmentDateSelectionView
    {
        [Outlet]
        UIKit.NSLayoutConstraint BackButtonTopConstraint { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint EarlyBottomConstraint { get; set; }


        [Outlet]
        Helseboka.iOS.Common.View.CheckBox EarlyCheckBox { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint EarlyTopConstraint { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint LateDayBottomConstraint { get; set; }


        [Outlet]
        Helseboka.iOS.Common.View.CheckBox LateDayCheckbox { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint LateDayTopConstraint { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint MiddayBottomConstraint { get; set; }


        [Outlet]
        Helseboka.iOS.Common.View.CheckBox MiddayCheckBox { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint MiddayTopConstraint { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint NextButtonHeightConstraint { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint PageInfoTopConstraint { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint PageSubtitleTopConstraint { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint PageTitleTopConstraint { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint ViewLeadingConstraint { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint ViewTrailingConstraint { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint WeekDayViewHeightConstraint { get; set; }


        [Outlet]
        Helseboka.iOS.Legetimer.View.WeekDayView WeekView { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint WeekViewTopConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Helseboka.iOS.Common.View.PrimaryActionButton NextButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel pageInfoLabel { get; set; }


        [Action ("Back_Pressed:")]
        partial void Back_Pressed (UIKit.UIButton sender);


        [Action ("Help_Tapped:")]
        partial void Help_Tapped (UIKit.UIButton sender);


        [Action ("NextButton_Tapped:")]
        partial void NextButton_Tapped (Helseboka.iOS.Common.View.PrimaryActionButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (BackButtonTopConstraint != null) {
                BackButtonTopConstraint.Dispose ();
                BackButtonTopConstraint = null;
            }

            if (EarlyBottomConstraint != null) {
                EarlyBottomConstraint.Dispose ();
                EarlyBottomConstraint = null;
            }

            if (EarlyCheckBox != null) {
                EarlyCheckBox.Dispose ();
                EarlyCheckBox = null;
            }

            if (EarlyTopConstraint != null) {
                EarlyTopConstraint.Dispose ();
                EarlyTopConstraint = null;
            }

            if (LateDayBottomConstraint != null) {
                LateDayBottomConstraint.Dispose ();
                LateDayBottomConstraint = null;
            }

            if (LateDayCheckbox != null) {
                LateDayCheckbox.Dispose ();
                LateDayCheckbox = null;
            }

            if (LateDayTopConstraint != null) {
                LateDayTopConstraint.Dispose ();
                LateDayTopConstraint = null;
            }

            if (MiddayBottomConstraint != null) {
                MiddayBottomConstraint.Dispose ();
                MiddayBottomConstraint = null;
            }

            if (MiddayCheckBox != null) {
                MiddayCheckBox.Dispose ();
                MiddayCheckBox = null;
            }

            if (MiddayTopConstraint != null) {
                MiddayTopConstraint.Dispose ();
                MiddayTopConstraint = null;
            }

            if (NextButton != null) {
                NextButton.Dispose ();
                NextButton = null;
            }

            if (NextButtonHeightConstraint != null) {
                NextButtonHeightConstraint.Dispose ();
                NextButtonHeightConstraint = null;
            }

            if (pageInfoLabel != null) {
                pageInfoLabel.Dispose ();
                pageInfoLabel = null;
            }

            if (PageInfoTopConstraint != null) {
                PageInfoTopConstraint.Dispose ();
                PageInfoTopConstraint = null;
            }

            if (PageSubtitleTopConstraint != null) {
                PageSubtitleTopConstraint.Dispose ();
                PageSubtitleTopConstraint = null;
            }

            if (PageTitleTopConstraint != null) {
                PageTitleTopConstraint.Dispose ();
                PageTitleTopConstraint = null;
            }

            if (ViewLeadingConstraint != null) {
                ViewLeadingConstraint.Dispose ();
                ViewLeadingConstraint = null;
            }

            if (ViewTrailingConstraint != null) {
                ViewTrailingConstraint.Dispose ();
                ViewTrailingConstraint = null;
            }

            if (WeekDayViewHeightConstraint != null) {
                WeekDayViewHeightConstraint.Dispose ();
                WeekDayViewHeightConstraint = null;
            }

            if (WeekView != null) {
                WeekView.Dispose ();
                WeekView = null;
            }

            if (WeekViewTopConstraint != null) {
                WeekViewTopConstraint.Dispose ();
                WeekViewTopConstraint = null;
            }
        }
    }
}