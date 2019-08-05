// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Helseboka.iOS.Startup.View.TableViewCell
{
    [Register ("SearchTableViewCell")]
    partial class SearchTableViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Helseboka.iOS.Common.View.HelpTextLabel DescriptionTextLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (DescriptionTextLabel != null) {
                DescriptionTextLabel.Dispose ();
                DescriptionTextLabel = null;
            }
        }
    }
}