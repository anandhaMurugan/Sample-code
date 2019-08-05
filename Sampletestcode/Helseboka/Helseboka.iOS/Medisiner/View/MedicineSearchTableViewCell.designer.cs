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
    [Register ("MedicineSearchTableViewCell")]
    partial class MedicineSearchTableViewCell
    {
        [Outlet]
        Helseboka.iOS.Common.View.HelpTextLabel SearchLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (SearchLabel != null) {
                SearchLabel.Dispose ();
                SearchLabel = null;
            }
        }
    }
}