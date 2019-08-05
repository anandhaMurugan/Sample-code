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
    [Register ("OnboardingView")]
    partial class OnboardingView
    {
        [Outlet]
        UIKit.UIButton BackwardButton { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint BackwardButtonCenterYConstraint { get; set; }


        [Outlet]
        UIKit.UIView ContainerView { get; set; }


        [Outlet]
        UIKit.UIButton ForwardButton { get; set; }


        [Outlet]
        Helseboka.iOS.Common.View.PrimaryActionButton HoppOverButton { get; set; }


        [Outlet]
        UIKit.UIPageControl PageControl { get; set; }


        [Action ("BackwardButton_Tapped:")]
        partial void BackwardButton_Tapped (UIKit.UIButton sender);


        [Action ("ForwardButton_Tapped:")]
        partial void ForwardButton_Tapped (UIKit.UIButton sender);


        [Action ("HoppOver_Tapped:")]
        partial void HoppOver_Tapped (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (BackwardButton != null) {
                BackwardButton.Dispose ();
                BackwardButton = null;
            }

            if (BackwardButtonCenterYConstraint != null) {
                BackwardButtonCenterYConstraint.Dispose ();
                BackwardButtonCenterYConstraint = null;
            }

            if (ContainerView != null) {
                ContainerView.Dispose ();
                ContainerView = null;
            }

            if (ForwardButton != null) {
                ForwardButton.Dispose ();
                ForwardButton = null;
            }

            if (HoppOverButton != null) {
                HoppOverButton.Dispose ();
                HoppOverButton = null;
            }

            if (PageControl != null) {
                PageControl.Dispose ();
                PageControl = null;
            }
        }
    }
}