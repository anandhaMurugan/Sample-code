using System;

using Foundation;
using UIKit;

namespace Helseboka.iOS.Common.TableViewCell
{
    public partial class TableViewLabelCell : BaseTableViewCell
    {
        public static readonly NSString Key = new NSString("TableViewLabelCell");
        public static readonly UINib Nib;

        static TableViewLabelCell()
        {
            Nib = UINib.FromName("TableViewLabelCell", NSBundle.MainBundle);
        }

        protected TableViewLabelCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public static TableViewLabelCell Create()
        {
            return (TableViewLabelCell)Nib.Instantiate(null, null)[0];
        }

        public void Configure(String text)
        {
            HelpTextLabel.Text = text;
            SelectionStyle = UITableViewCellSelectionStyle.None;
        }
    }
}
