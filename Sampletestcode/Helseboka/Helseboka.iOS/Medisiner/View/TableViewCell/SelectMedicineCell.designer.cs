// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Helseboka.iOS.Medisiner.View.TableViewCell
{
    [Register ("SelectMedicineCell")]
    partial class SelectMedicineCell
    {
        [Outlet]
        UIKit.NSLayoutConstraint CheckboxWidthConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Helseboka.iOS.Common.View.BaseUILabel MedicineName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Helseboka.iOS.Common.View.CheckBox SelectCheckBox { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (MedicineName != null) {
                MedicineName.Dispose ();
                MedicineName = null;
            }

            if (SelectCheckBox != null) {
                SelectCheckBox.Dispose ();
                SelectCheckBox = null;
            }
        }
    }
}