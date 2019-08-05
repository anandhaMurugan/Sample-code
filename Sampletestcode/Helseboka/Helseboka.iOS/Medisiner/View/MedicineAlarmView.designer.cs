// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Helseboka.iOS.Medisiner.View
{
    [Register ("MedicineAlarmView")]
    partial class MedicineAlarmView
    {
        [Outlet]
        UIKit.UITableView AlarmTableView { get; set; }


        [Outlet]
        Helseboka.iOS.Common.View.PageTitleLabel MedicineName { get; set; }


        [Outlet]
        Helseboka.iOS.Common.View.DescriptionLabel MedicineStrength { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Helseboka.iOS.Common.View.MediumActionButton SaveButton { get; set; }


        [Action ("Back_Tapped:")]
        partial void Back_Tapped (UIKit.UIButton sender);


        [Action ("Save_Tapped:")]
        partial void Save_Tapped (Helseboka.iOS.Common.View.MediumActionButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (AlarmTableView != null) {
                AlarmTableView.Dispose ();
                AlarmTableView = null;
            }

            if (MedicineName != null) {
                MedicineName.Dispose ();
                MedicineName = null;
            }

            if (MedicineStrength != null) {
                MedicineStrength.Dispose ();
                MedicineStrength = null;
            }

            if (SaveButton != null) {
                SaveButton.Dispose ();
                SaveButton = null;
            }
        }
    }
}