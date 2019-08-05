// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Helseboka.iOS.Common.View.PopUpDialogs
{
    [Register ("BasicInfoAlert")]
    partial class BasicInfoAlert
    {
        [Outlet]
        UIKit.UILabel MessageLabel { get; set; }


        [Outlet]
        Helseboka.iOS.Common.View.MediumActionButton OkButtonLabel { get; set; }


        [Outlet]
        UIKit.UILabel TitleLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView ContentView { get; set; }


        [Action ("Ok_Tapped:")]
        partial void Ok_Tapped (Helseboka.iOS.Common.View.MediumActionButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (ContentView != null) {
                ContentView.Dispose ();
                ContentView = null;
            }

            if (MessageLabel != null) {
                MessageLabel.Dispose ();
                MessageLabel = null;
            }

            if (OkButtonLabel != null) {
                OkButtonLabel.Dispose ();
                OkButtonLabel = null;
            }

            if (TitleLabel != null) {
                TitleLabel.Dispose ();
                TitleLabel = null;
            }
        }
    }
}