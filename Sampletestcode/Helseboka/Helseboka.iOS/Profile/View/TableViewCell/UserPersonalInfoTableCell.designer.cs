// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Helseboka.iOS.Profile.View.TableViewCell
{
    [Register ("UserPersonalInfoTableCell")]
    partial class UserPersonalInfoTableCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel InfoNameLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField InfoValueLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (InfoNameLabel != null) {
                InfoNameLabel.Dispose ();
                InfoNameLabel = null;
            }

            if (InfoValueLabel != null) {
                InfoValueLabel.Dispose ();
                InfoValueLabel = null;
            }
        }
    }
}