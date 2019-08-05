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
    [Register ("AddMoreMedicineCell")]
    partial class AddMoreMedicineCell
    {
        [Outlet]
        UIKit.UILabel MedicineLabel { get; set; }


        [Action ("MedicineClose_Tapped:")]
        partial void MedicineClose_Tapped (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (MedicineLabel != null) {
                MedicineLabel.Dispose ();
                MedicineLabel = null;
            }
        }
    }
}