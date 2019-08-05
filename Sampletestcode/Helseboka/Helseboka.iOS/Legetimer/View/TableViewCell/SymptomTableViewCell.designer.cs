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
    [Register ("SymptomTableViewCell")]
    partial class SymptomTableViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel symptomDescriptionLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel symptomNameLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (symptomDescriptionLabel != null) {
                symptomDescriptionLabel.Dispose ();
                symptomDescriptionLabel = null;
            }

            if (symptomNameLabel != null) {
                symptomNameLabel.Dispose ();
                symptomNameLabel = null;
            }
        }
    }
}