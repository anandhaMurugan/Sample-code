// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Helseboka.iOS.Common.TableViewCell
{
    [Register ("TableViewLabelCell")]
    partial class TableViewLabelCell
    {
        [Outlet]
        Helseboka.iOS.Common.View.HelpDescriptionLabel HelpTextLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (HelpTextLabel != null) {
                HelpTextLabel.Dispose ();
                HelpTextLabel = null;
            }
        }
    }
}