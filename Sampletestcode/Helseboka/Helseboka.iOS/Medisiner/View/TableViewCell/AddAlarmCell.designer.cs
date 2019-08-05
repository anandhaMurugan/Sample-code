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
    [Register ("AddAlarmCell")]
    partial class AddAlarmCell
    {
        [Outlet]
        Helseboka.iOS.Common.View.BaseUILabel AlarmLabel { get; set; }


        [Action ("Delete_Tapped:")]
        partial void Delete_Tapped (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (AlarmLabel != null) {
                AlarmLabel.Dispose ();
                AlarmLabel = null;
            }
        }
    }
}