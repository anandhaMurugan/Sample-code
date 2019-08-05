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
    [Register ("SymptomInfoTableViewCell")]
    partial class SymptomInfoTableViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Helseboka.iOS.Common.View.BaseTextfield descriptionBox { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel symtomCounterNameLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (descriptionBox != null) {
                descriptionBox.Dispose ();
                descriptionBox = null;
            }

            if (symtomCounterNameLabel != null) {
                symtomCounterNameLabel.Dispose ();
                symtomCounterNameLabel = null;
            }
        }
    }
}