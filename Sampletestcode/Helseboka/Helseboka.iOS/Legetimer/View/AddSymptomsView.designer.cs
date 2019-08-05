// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Helseboka.iOS.Legetimer.View
{
    [Register ("AddSymptomsView")]
    partial class AddSymptomsView
    {
        [Outlet]
        UIKit.NSLayoutConstraint NextButtonBottomConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Helseboka.iOS.Common.View.MediumActionButton NextButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Helseboka.iOS.Common.View.BaseTableView SymptomListTableView { get; set; }


        [Action ("AddMore_Tapped:")]
        partial void AddMore_Tapped (UIKit.UIButton sender);


        [Action ("Back_Pressed:")]
        partial void Back_Pressed (UIKit.UIButton sender);


        [Action ("Help_Tapped:")]
        partial void Help_Tapped (UIKit.UIButton sender);


        [Action ("NextButton_Tapped:")]
        partial void NextButton_Tapped (Helseboka.iOS.Common.View.PrimaryActionButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (NextButton != null) {
                NextButton.Dispose ();
                NextButton = null;
            }

            if (NextButtonBottomConstraint != null) {
                NextButtonBottomConstraint.Dispose ();
                NextButtonBottomConstraint = null;
            }

            if (SymptomListTableView != null) {
                SymptomListTableView.Dispose ();
                SymptomListTableView = null;
            }
        }
    }
}