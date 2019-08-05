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
    [Register ("YesNoDialogView")]
    partial class YesNoDialogView
    {
        [Outlet]
        UIKit.UIButton CloseButton { get; set; }


        [Outlet]
        UIKit.UIButton LeftButton { get; set; }


        [Outlet]
        UIKit.UILabel MessageLabel { get; set; }


        [Outlet]
        UIKit.UIButton RightButton { get; set; }


        [Outlet]
        UIKit.UILabel TitleLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView DialogView { get; set; }


        [Action ("Close_Tapped:")]
        partial void Close_Tapped (UIKit.UIButton sender);


        [Action ("LeftButton_Tapped:")]
        partial void LeftButton_Tapped (UIKit.UIButton sender);


        [Action ("RightButton_Tapped:")]
        partial void RightButton_Tapped (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (CloseButton != null) {
                CloseButton.Dispose ();
                CloseButton = null;
            }

            if (DialogView != null) {
                DialogView.Dispose ();
                DialogView = null;
            }

            if (LeftButton != null) {
                LeftButton.Dispose ();
                LeftButton = null;
            }

            if (MessageLabel != null) {
                MessageLabel.Dispose ();
                MessageLabel = null;
            }

            if (RightButton != null) {
                RightButton.Dispose ();
                RightButton = null;
            }

            if (TitleLabel != null) {
                TitleLabel.Dispose ();
                TitleLabel = null;
            }
        }
    }
}