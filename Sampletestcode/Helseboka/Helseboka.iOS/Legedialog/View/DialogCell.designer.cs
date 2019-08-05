// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Helseboka.iOS.Legedialog.View
{
	[Register ("DialogCell")]
	partial class DialogCell
	{
		[Outlet]
		Helseboka.iOS.Common.View.TitleLabel HeadlineLabel { get; set; }

		[Outlet]
		UIKit.UILabel StatusLabel { get; set; }

		[Outlet]
		Helseboka.iOS.Common.View.DescriptionLabel SubtitleLabel { get; set; }

		[Outlet]
		Helseboka.iOS.Common.View.DescriptionLabel TimeLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (HeadlineLabel != null) {
				HeadlineLabel.Dispose ();
				HeadlineLabel = null;
			}

			if (SubtitleLabel != null) {
				SubtitleLabel.Dispose ();
				SubtitleLabel = null;
			}

			if (TimeLabel != null) {
				TimeLabel.Dispose ();
				TimeLabel = null;
			}

			if (StatusLabel != null) {
				StatusLabel.Dispose ();
				StatusLabel = null;
			}
		}
	}
}
