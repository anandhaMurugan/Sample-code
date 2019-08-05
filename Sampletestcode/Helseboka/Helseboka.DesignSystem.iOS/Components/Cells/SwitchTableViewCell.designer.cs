// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Helseboka.DesignSystem.iOS.Components.Cells
{
	[Register ("SwitchTableViewCell")]
	partial class SwitchTableViewCell
	{
		[Outlet]
		UIKit.UILabel Label { get; set; }

		[Outlet]
		UIKit.UISwitch Switch { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (Label != null) {
				Label.Dispose ();
				Label = null;
			}

			if (Switch != null) {
				Switch.Dispose ();
				Switch = null;
			}
		}
	}
}
