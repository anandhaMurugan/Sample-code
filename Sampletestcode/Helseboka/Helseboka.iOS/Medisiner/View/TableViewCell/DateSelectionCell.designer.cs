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
    [Register ("DateSelectionCell")]
    partial class DateSelectionCell
    {
        [Outlet]
        UIKit.UIButton AddMoreButton { get; set; }


        [Outlet]
        UIKit.UILabel AddMoreLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (AddMoreButton != null) {
                AddMoreButton.Dispose ();
                AddMoreButton = null;
            }

            if (AddMoreLabel != null) {
                AddMoreLabel.Dispose ();
                AddMoreLabel = null;
            }
        }
    }
}