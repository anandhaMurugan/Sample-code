using System;

using Foundation;
using UIKit;

namespace Helseboka.iOS.Common.TableViewCell
{
    public partial class HelpTableViewImageCell : BaseTableViewCell
    {
        public static readonly NSString Key = new NSString("HelpTableViewImageCell");
        public static readonly UINib Nib;

        static HelpTableViewImageCell()
        {
            Nib = UINib.FromName("HelpTableViewImageCell", NSBundle.MainBundle);
        }

        protected HelpTableViewImageCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public static HelpTableViewImageCell Create()
        {
            return (HelpTableViewImageCell)Nib.Instantiate(null, null)[0];
        }

        public void Configure(UIImage image)
        {
            HelpImageView.Image = image;
            SelectionStyle = UITableViewCellSelectionStyle.None;
        }
    }
}
