// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Helseboka.iOS.Components.Cells
{
    [Register ("SwitchTableViewCell")]
    partial class SwitchTableViewCell
    {
        [Outlet]
        UIKit.UILabel Label { get; set; }


        [Outlet]
        UIKit.UISwitch Switch { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIStackView SwitchTextStackView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (Label != null) {
                Label.Dispose ();
                Label = null;
            }

            if (Switch != null) {
                Switch.Dispose ();
                Switch = null;
            }

            if (SwitchTextStackView != null) {
                SwitchTextStackView.Dispose ();
                SwitchTextStackView = null;
            }
        }
    }
}