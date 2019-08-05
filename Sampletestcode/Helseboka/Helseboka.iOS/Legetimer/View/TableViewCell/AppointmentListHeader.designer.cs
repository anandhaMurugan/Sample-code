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
    [Register ("AppointmentListHeader")]
    partial class AppointmentListHeader
    {
        [Outlet]
        UIKit.UILabel SectionHeaderTitleLabel { get; set; }


        [Outlet]
        UIKit.UIView SideLine { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (SectionHeaderTitleLabel != null) {
                SectionHeaderTitleLabel.Dispose ();
                SectionHeaderTitleLabel = null;
            }

            if (SideLine != null) {
                SideLine.Dispose ();
                SideLine = null;
            }
        }
    }
}