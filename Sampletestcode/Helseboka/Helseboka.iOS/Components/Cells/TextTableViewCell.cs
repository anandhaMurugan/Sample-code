using System;

using Foundation;
using Helseboka.iOS.Constants;
using UIKit;

namespace Helseboka.iOS.Components.Cells
{
    public partial class TextTableViewCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("TextTableViewCell");
        public static readonly UINib Nib;

        public override UILabel TextLabel => Label;

        static TextTableViewCell()
        {
            Nib = UINib.FromName("TextTableViewCell", NSBundle.MainBundle);
        }

        protected TextTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            TextLabel.Font = Fonts.Regular(14);
            TextLabel.TextColor = Colors.Grey600;
        }

        public void SetText(string text)
        {
            TextLabel.Text = text;
        }

    }
}
