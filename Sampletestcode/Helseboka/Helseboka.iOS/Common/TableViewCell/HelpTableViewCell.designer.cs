// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Helseboka.iOS.Common.TableViewCell
{
    [Register ("HelpTableViewCell")]
    partial class HelpTableViewCell
    {
        [Outlet]
        UIKit.UIImageView ArroeImage { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint DescriptionBottomConstraint { get; set; }


        [Outlet]
        Helseboka.iOS.Common.View.LinkableLabel HelpDescription { get; set; }


        [Outlet]
        Helseboka.iOS.Common.View.TitleLabel HelpTitle { get; set; }


        [Outlet]
        UIKit.UIView TextContainerView { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint TitleBottomConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ArrowImage { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ArrowImage != null) {
                ArrowImage.Dispose ();
                ArrowImage = null;
            }

            if (DescriptionBottomConstraint != null) {
                DescriptionBottomConstraint.Dispose ();
                DescriptionBottomConstraint = null;
            }

            if (HelpDescription != null) {
                HelpDescription.Dispose ();
                HelpDescription = null;
            }

            if (HelpTitle != null) {
                HelpTitle.Dispose ();
                HelpTitle = null;
            }

            if (TextContainerView != null) {
                TextContainerView.Dispose ();
                TextContainerView = null;
            }

            if (TitleBottomConstraint != null) {
                TitleBottomConstraint.Dispose ();
                TitleBottomConstraint = null;
            }
        }
    }
}