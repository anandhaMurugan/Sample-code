// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Helseboka.iOS.Startup.View
{
    [Register ("OnboardingContentView")]
    partial class OnboardingContentView
    {
        [Outlet]
        UIKit.UIImageView ImageView { get; set; }


        [Outlet]
        UIKit.UILabel TitleLabel { get; set; }


        [Action ("BackTapped:")]
        partial void BackTapped (UIKit.UIButton sender);


        [Action ("ForwardTapped:")]
        partial void ForwardTapped (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (ImageView != null) {
                ImageView.Dispose ();
                ImageView = null;
            }

            if (TitleLabel != null) {
                TitleLabel.Dispose ();
                TitleLabel = null;
            }
        }
    }
}