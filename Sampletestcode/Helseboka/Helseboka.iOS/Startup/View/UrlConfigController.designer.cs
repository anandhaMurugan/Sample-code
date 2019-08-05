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
    [Register ("UrlConfigController")]
    partial class UrlConfigController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Helseboka.iOS.Common.View.PrimaryActionButton ContinueToLoginBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Helseboka.iOS.Common.View.CheckBox DevCheckBox { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ErrLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Helseboka.iOS.Common.View.CheckBox PreProdBankidCheckBox { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Helseboka.iOS.Common.View.CheckBox ProdBankidCheckBox { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Helseboka.iOS.Common.View.CheckBox ProdCheckBox { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Helseboka.iOS.Common.View.CheckBox StagCheckBox { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Helseboka.iOS.Common.View.CheckBox TestCheckBox { get; set; }

        [Action ("ContinueToLogin_Tapped:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void ContinueToLogin_Tapped (Helseboka.iOS.Common.View.PrimaryActionButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (ContinueToLoginBtn != null) {
                ContinueToLoginBtn.Dispose ();
                ContinueToLoginBtn = null;
            }

            if (DevCheckBox != null) {
                DevCheckBox.Dispose ();
                DevCheckBox = null;
            }

            if (ErrLabel != null) {
                ErrLabel.Dispose ();
                ErrLabel = null;
            }

            if (PreProdBankidCheckBox != null) {
                PreProdBankidCheckBox.Dispose ();
                PreProdBankidCheckBox = null;
            }

            if (ProdBankidCheckBox != null) {
                ProdBankidCheckBox.Dispose ();
                ProdBankidCheckBox = null;
            }

            if (ProdCheckBox != null) {
                ProdCheckBox.Dispose ();
                ProdCheckBox = null;
            }

            if (StagCheckBox != null) {
                StagCheckBox.Dispose ();
                StagCheckBox = null;
            }

            if (TestCheckBox != null) {
                TestCheckBox.Dispose ();
                TestCheckBox = null;
            }
        }
    }
}