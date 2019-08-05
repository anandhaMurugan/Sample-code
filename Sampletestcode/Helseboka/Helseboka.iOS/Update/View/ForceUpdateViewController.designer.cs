// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Helseboka.iOS
{
    [Register ("ForceUpdateViewController")]
    partial class ForceUpdateViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton CloseOrNotNowButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel DescriptionLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel TitleLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Helseboka.iOS.Common.View.MediumActionButton UpdateButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (CloseOrNotNowButton != null) {
                CloseOrNotNowButton.Dispose ();
                CloseOrNotNowButton = null;
            }

            if (DescriptionLabel != null) {
                DescriptionLabel.Dispose ();
                DescriptionLabel = null;
            }

            if (TitleLabel != null) {
                TitleLabel.Dispose ();
                TitleLabel = null;
            }

            if (UpdateButton != null) {
                UpdateButton.Dispose ();
                UpdateButton = null;
            }
        }
    }
}