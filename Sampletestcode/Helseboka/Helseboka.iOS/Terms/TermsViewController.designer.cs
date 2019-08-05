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
    [Register ("TermsViewController")]
    partial class TermsViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Helseboka.iOS.Common.View.PrimaryActionButton ContinueBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView FloatingBottomView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView TermsTableView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Helseboka.iOS.Common.View.PageTitleLabel TermsTitle { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ContinueBtn != null) {
                ContinueBtn.Dispose ();
                ContinueBtn = null;
            }

            if (FloatingBottomView != null) {
                FloatingBottomView.Dispose ();
                FloatingBottomView = null;
            }

            if (TermsTableView != null) {
                TermsTableView.Dispose ();
                TermsTableView = null;
            }

            if (TermsTitle != null) {
                TermsTitle.Dispose ();
                TermsTitle = null;
            }
        }
    }
}