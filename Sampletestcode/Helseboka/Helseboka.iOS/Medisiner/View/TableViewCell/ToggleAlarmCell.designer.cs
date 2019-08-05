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
    [Register ("ToggleAlarmCell")]
    partial class ToggleAlarmCell
    {
        [Outlet]
        UIKit.UISwitch ToggleAlarmSwitch { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ToggleAlarmSwitch != null) {
                ToggleAlarmSwitch.Dispose ();
                ToggleAlarmSwitch = null;
            }
        }
    }
}