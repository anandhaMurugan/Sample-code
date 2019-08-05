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
    [Register ("ReadMoreTextTableViewCell")]
    partial class ReadMoreTextTableViewCell
    {
        [Outlet]
        UIKit.UILabel Label { get; set; }


        [Outlet]
        UIKit.UIButton ShowMoreButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel _textLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_textLabel != null) {
                _textLabel.Dispose ();
                _textLabel = null;
            }

            if (Label != null) {
                Label.Dispose ();
                Label = null;
            }

            if (ShowMoreButton != null) {
                ShowMoreButton.Dispose ();
                ShowMoreButton = null;
            }
        }
    }
}