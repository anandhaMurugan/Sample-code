// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Helseboka.iOS.Legedialog.View
{
    [Register ("LegeDialogListView")]
    partial class LegeDialogListView
    {
        [Outlet]
        Common.View.BaseTableView DataTableView { get; set; }


        [Outlet]
        UIKit.UIButton HelpButton { get; set; }


        [Outlet]
        UIKit.UIView NoDataView { get; set; }


        [Action ("Help_Tapped:")]
        partial void Help_Tapped (UIKit.UIButton sender);


        [Action ("NewDialog_Tapped:")]
        partial void NewDialog_Tapped (Helseboka.iOS.Common.View.PrimaryActionButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (DataTableView != null) {
                DataTableView.Dispose ();
                DataTableView = null;
            }

            if (HelpButton != null) {
                HelpButton.Dispose ();
                HelpButton = null;
            }

            if (NoDataView != null) {
                NoDataView.Dispose ();
                NoDataView = null;
            }
        }
    }
}