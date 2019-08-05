// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Helseboka.iOS.Profile.View
{
	[Register ("MyProfileView")]
	partial class MyProfileView
	{
		[Outlet]
		UIKit.UIButton HelpButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UITableView userDetailsTableView { get; set; }

		[Outlet]
		UIKit.UILabel UserNameLabel { get; set; }

		[Action ("Help_Tapped:")]
		partial void Help_Tapped (UIKit.UIButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (UserNameLabel != null) {
				UserNameLabel.Dispose ();
				UserNameLabel = null;
			}

			if (userDetailsTableView != null) {
				userDetailsTableView.Dispose ();
				userDetailsTableView = null;
			}

			if (HelpButton != null) {
				HelpButton.Dispose ();
				HelpButton = null;
			}
		}
	}
}
