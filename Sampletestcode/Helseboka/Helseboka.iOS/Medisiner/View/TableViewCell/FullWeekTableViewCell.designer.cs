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
    [Register ("FullWeekTableViewCell")]
    partial class FullWeekTableViewCell
    {
        [Outlet]
        UIKit.UIButton AddMoreButton { get; set; }


        [Outlet]
        UIKit.UILabel AddMoreLabel { get; set; }


        [Outlet]
        Helseboka.iOS.Medisiner.View.TableViewCell.FullWeekView WeekView { get; set; }

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

            if (WeekView != null) {
                WeekView.Dispose ();
                WeekView = null;
            }
        }
    }
}