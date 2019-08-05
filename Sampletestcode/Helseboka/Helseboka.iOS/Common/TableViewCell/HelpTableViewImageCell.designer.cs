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
    [Register ("HelpTableViewImageCell")]
    partial class HelpTableViewImageCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView HelpImageView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (HelpImageView != null) {
                HelpImageView.Dispose ();
                HelpImageView = null;
            }
        }
    }
}