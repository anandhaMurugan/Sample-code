// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Helseboka.iOS.Profile.View
{
    [Register ("UserInfoViewController")]
    partial class UserInfoViewController
    {
        [Outlet]
        UIKit.NSLayoutConstraint SaveButtonBottomConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton backButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel NameLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView UserInfoTableView { get; set; }


        [Action ("BackButton_TouchUpInside:")]
        partial void BackButton_TouchUpInside (UIKit.UIButton sender);


        [Action ("Save_Tapped:")]
        partial void Save_Tapped (UIKit.UIButton sender);


        [Action ("SaveButtonClickEvent:")]
        partial void SaveButtonClickEvent (Foundation.NSObject sender);

        void ReleaseDesignerOutlets ()
        {
            if (backButton != null) {
                backButton.Dispose ();
                backButton = null;
            }

            if (NameLabel != null) {
                NameLabel.Dispose ();
                NameLabel = null;
            }

            if (SaveButtonBottomConstraint != null) {
                SaveButtonBottomConstraint.Dispose ();
                SaveButtonBottomConstraint = null;
            }

            if (UserInfoTableView != null) {
                UserInfoTableView.Dispose ();
                UserInfoTableView = null;
            }
        }
    }
}