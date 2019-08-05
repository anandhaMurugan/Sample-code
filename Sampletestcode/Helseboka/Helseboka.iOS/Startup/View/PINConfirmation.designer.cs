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
    [Register ("PINConfirmation")]
    partial class PINConfirmation
    {
        [Outlet]
        UIKit.NSLayoutConstraint ContainerTopConstraint { get; set; }


        [Outlet]
        UIKit.UILabel PageHeaderLabel { get; set; }


        [Outlet]
        Helseboka.iOS.Common.View.CircularPinView PINView { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint PINViewTopConstraint { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint TitleTopConstraint { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ContainerTopConstraint != null) {
                ContainerTopConstraint.Dispose ();
                ContainerTopConstraint = null;
            }

            if (PageHeaderLabel != null) {
                PageHeaderLabel.Dispose ();
                PageHeaderLabel = null;
            }

            if (PINView != null) {
                PINView.Dispose ();
                PINView = null;
            }

            if (PINViewTopConstraint != null) {
                PINViewTopConstraint.Dispose ();
                PINViewTopConstraint = null;
            }

            if (TitleTopConstraint != null) {
                TitleTopConstraint.Dispose ();
                TitleTopConstraint = null;
            }
        }
    }
}