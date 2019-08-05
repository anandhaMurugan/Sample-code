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
    [Register ("DashboardAlarmView")]
    partial class DashboardAlarmView
    {
        [Outlet]
        UIKit.UIView Overlay { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Helseboka.iOS.Common.View.CheckBox AlarmDoneCheck { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel AlarmTimeLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel MedicineNameLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel MedicineStrengthLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (AlarmDoneCheck != null) {
                AlarmDoneCheck.Dispose ();
                AlarmDoneCheck = null;
            }

            if (AlarmTimeLabel != null) {
                AlarmTimeLabel.Dispose ();
                AlarmTimeLabel = null;
            }

            if (MedicineNameLabel != null) {
                MedicineNameLabel.Dispose ();
                MedicineNameLabel = null;
            }

            if (MedicineStrengthLabel != null) {
                MedicineStrengthLabel.Dispose ();
                MedicineStrengthLabel = null;
            }

            if (Overlay != null) {
                Overlay.Dispose ();
                Overlay = null;
            }
        }
    }
}