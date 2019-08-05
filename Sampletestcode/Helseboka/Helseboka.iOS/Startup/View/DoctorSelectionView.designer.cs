// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Helseboka.iOS.Startup.View
{
	[Register ("DoctorSelectionView")]
	partial class DoctorSelectionView
	{
		[Outlet]
		UIKit.UITextView ErrorTextView { get; set; }

		[Outlet]
		Helseboka.iOS.Common.View.PrimaryActionButton OkButton { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint OkButtonBottomConstraint { get; set; }

		[Outlet]
		UIKit.UILabel PageSubtitleText { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint PageSubtitleTopConstraint { get; set; }

		[Outlet]
		Helseboka.iOS.Common.View.PageTitleLabel PageTitleLabel { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint PageTitleTopConstraint { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIView SearchContainer { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UITableView SearchResultTableView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		Helseboka.iOS.Common.View.BaseTextfield SearchText { get; set; }

		[Outlet]
		Helseboka.iOS.Common.View.SelectableLable SelectionLabel { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint SelectionlabelLeadingConstraint { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint SelectionLabelTopConstraint { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint SelectionLabelTrailingConstraint { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint TableViewBottomConstraint { get; set; }

		[Action ("Cancel_Tapped:")]
		partial void Cancel_Tapped (Helseboka.iOS.Common.View.SignUpSecondaryActionButton sender);

		[Action ("Close_Tapped:")]
		partial void Close_Tapped (UIKit.UIButton sender);

		[Action ("Ok_Tapped:forEvent:")]
		partial void Ok_Tapped (Helseboka.iOS.Common.View.PrimaryActionButton sender, UIKit.UIEvent @event);

		[Action ("SearchText_BeginEditing:")]
		partial void SearchText_BeginEditing (Helseboka.iOS.Common.View.BaseTextfield sender);

		[Action ("SearchText_Changed:")]
		partial void SearchText_Changed (Helseboka.iOS.Common.View.BaseTextfield sender);

		[Action ("SearchText_EndEditing:")]
		partial void SearchText_EndEditing (Helseboka.iOS.Common.View.BaseTextfield sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (SelectionLabelTrailingConstraint != null) {
				SelectionLabelTrailingConstraint.Dispose ();
				SelectionLabelTrailingConstraint = null;
			}

			if (SelectionlabelLeadingConstraint != null) {
				SelectionlabelLeadingConstraint.Dispose ();
				SelectionlabelLeadingConstraint = null;
			}

			if (ErrorTextView != null) {
				ErrorTextView.Dispose ();
				ErrorTextView = null;
			}

			if (OkButton != null) {
				OkButton.Dispose ();
				OkButton = null;
			}

			if (OkButtonBottomConstraint != null) {
				OkButtonBottomConstraint.Dispose ();
				OkButtonBottomConstraint = null;
			}

			if (PageSubtitleText != null) {
				PageSubtitleText.Dispose ();
				PageSubtitleText = null;
			}

			if (PageSubtitleTopConstraint != null) {
				PageSubtitleTopConstraint.Dispose ();
				PageSubtitleTopConstraint = null;
			}

			if (PageTitleLabel != null) {
				PageTitleLabel.Dispose ();
				PageTitleLabel = null;
			}

			if (PageTitleTopConstraint != null) {
				PageTitleTopConstraint.Dispose ();
				PageTitleTopConstraint = null;
			}

			if (SearchContainer != null) {
				SearchContainer.Dispose ();
				SearchContainer = null;
			}

			if (SearchResultTableView != null) {
				SearchResultTableView.Dispose ();
				SearchResultTableView = null;
			}

			if (SearchText != null) {
				SearchText.Dispose ();
				SearchText = null;
			}

			if (SelectionLabel != null) {
				SelectionLabel.Dispose ();
				SelectionLabel = null;
			}

			if (SelectionLabelTopConstraint != null) {
				SelectionLabelTopConstraint.Dispose ();
				SelectionLabelTopConstraint = null;
			}

			if (TableViewBottomConstraint != null) {
				TableViewBottomConstraint.Dispose ();
				TableViewBottomConstraint = null;
			}
		}
	}
}
