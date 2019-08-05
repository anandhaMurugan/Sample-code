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
    [Register ("ModalHelpTableViewCell")]
    partial class ModalHelpTableViewCell
    {
        [Outlet]
        Helseboka.iOS.Common.View.LinkableLabel HelpDescriptionLabel { get; set; }


        [Outlet]
        UIKit.UILabel HelpTitleLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (HelpDescriptionLabel != null) {
                HelpDescriptionLabel.Dispose ();
                HelpDescriptionLabel = null;
            }

            if (HelpTitleLabel != null) {
                HelpTitleLabel.Dispose ();
                HelpTitleLabel = null;
            }
        }
    }
}