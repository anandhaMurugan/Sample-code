// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Helseboka.iOS.Profile.View.TableViewCell
{
    [Register ("UserProfileTableViewCell")]
    partial class UserProfileTableViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView arrowImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel fieldNameLable { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel fieldValueLable { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (arrowImage != null) {
                arrowImage.Dispose ();
                arrowImage = null;
            }

            if (fieldNameLable != null) {
                fieldNameLable.Dispose ();
                fieldNameLable = null;
            }

            if (fieldValueLable != null) {
                fieldValueLable.Dispose ();
                fieldValueLable = null;
            }
        }
    }
}