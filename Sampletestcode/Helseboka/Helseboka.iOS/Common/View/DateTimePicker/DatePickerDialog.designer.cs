// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Helseboka.iOS.Common.View.DateTimePicker
{
    [Register ("DatePickerDialog")]
    partial class DatePickerDialog
    {
        [Outlet]
        UIKit.UIView ContainerView { get; set; }


        [Outlet]
        UIKit.UIDatePicker DatePicker { get; set; }


        [Outlet]
        UIKit.UILabel TitleLabel { get; set; }


        [Action ("Cancel:")]
        partial void Cancel (Foundation.NSObject sender);


        [Action ("Done:")]
        partial void Done (Foundation.NSObject sender);

        void ReleaseDesignerOutlets ()
        {
            if (ContainerView != null) {
                ContainerView.Dispose ();
                ContainerView = null;
            }

            if (DatePicker != null) {
                DatePicker.Dispose ();
                DatePicker = null;
            }

            if (TitleLabel != null) {
                TitleLabel.Dispose ();
                TitleLabel = null;
            }
        }
    }
}