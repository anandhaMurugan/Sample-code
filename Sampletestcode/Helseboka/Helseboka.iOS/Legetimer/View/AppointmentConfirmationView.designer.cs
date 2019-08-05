// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Helseboka.iOS.Legetimer.View
{
    [Register ("AppointmentConfirmationView")]
    partial class AppointmentConfirmationView
    {
        [Outlet]
        UIKit.UILabel DescriptionText { get; set; }


        [Outlet]
        UIKit.UILabel PageTitle { get; set; }


        [Outlet]
        UIKit.UILabel RememberText { get; set; }


        [Action ("Help_Tapped:")]
        partial void Help_Tapped (UIKit.UIButton sender);


        [Action ("Ok_Tapped:")]
        partial void Ok_Tapped (Helseboka.iOS.Common.View.PrimaryActionButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (DescriptionText != null) {
                DescriptionText.Dispose ();
                DescriptionText = null;
            }

            if (PageTitle != null) {
                PageTitle.Dispose ();
                PageTitle = null;
            }

            if (RememberText != null) {
                RememberText.Dispose ();
                RememberText = null;
            }
        }
    }
}