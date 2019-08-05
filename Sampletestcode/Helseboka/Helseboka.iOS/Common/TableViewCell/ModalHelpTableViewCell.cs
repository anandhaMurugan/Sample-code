using System;

using Foundation;
using Helseboka.iOS.Common.View;
using UIKit;
using Xamarin.TTTAttributedLabel;

namespace Helseboka.iOS.Common.TableViewCell
{
    public partial class ModalHelpTableViewCell : BaseTableViewCell
    {
        public static readonly NSString Key = new NSString("ModalHelpTableViewCell");
        public static readonly UINib Nib;

        static ModalHelpTableViewCell()
        {
            Nib = UINib.FromName("ModalHelpTableViewCell", NSBundle.MainBundle);
        }

        public static ModalHelpTableViewCell Create()
        {
            return (ModalHelpTableViewCell)Nib.Instantiate(null, null)[0];
        }

        protected ModalHelpTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public void Configure(String title, String description, Action<NSUrl> onLinkTap)
        {
            HelpDescriptionLabel.Delegate = new LinkDelegate(onLinkTap);
            HelpTitleLabel.Text = title;
            HelpDescriptionLabel.Text = description;
        }
    }


}
