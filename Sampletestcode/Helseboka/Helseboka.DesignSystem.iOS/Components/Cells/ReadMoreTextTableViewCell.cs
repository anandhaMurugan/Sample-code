using System;

using Foundation;
using Helseboka.DesignSystem.iOS.Constants;
using UIKit;

namespace Helseboka.DesignSystem.iOS.Components.Cells
{
    public interface IReadMoreTextTableViewCellDelegate
    {
        void ToggleState(ReadMoreTextTableViewCell cell);
    }

    public partial class ReadMoreTextTableViewCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("ReadMoreTextTableViewCell");
        public static readonly UINib Nib;

        public IReadMoreTextTableViewCellDelegate Delegate;

        static ReadMoreTextTableViewCell()
        {
            Nib = UINib.FromName("ReadMoreTextTableViewCell", NSBundle.MainBundle);
        }

        protected ReadMoreTextTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override UILabel TextLabel => Label;

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            TextLabel.Font = Fonts.Regular(14);
            TextLabel.TextColor = Colors.Grey600;
            ShowMoreButton.TitleLabel.Font = Fonts.Bold(14);
            ShowMoreButton.TouchUpInside += (_1, _2) => ToggleState();
            ShowMoreButton.TintColor = Colors.Turquoise;

            var tapGestureRecognizer = new UITapGestureRecognizer(ToggleState);
            ContentView.AddGestureRecognizer(tapGestureRecognizer);
        }

        public void SetTexts(string mainText, string moreText, string showMoreText, UIImage showMoreImage)
        {
            var text = mainText;
            if (moreText.Length > 0)
            {
                text += "\n\n" + moreText;
            }
            TextLabel.Text = text;

            var attributed = new NSMutableAttributedString(showMoreText + " ");
            var attachment = new NSTextAttachment { Image = showMoreImage };
            attributed.Append(NSAttributedString.FromAttachment(attachment));

            PerformWithoutAnimation(() =>
            {
                ShowMoreButton.SetAttributedTitle(attributed, UIControlState.Normal);
                ShowMoreButton.LayoutIfNeeded();
            });
        }

        private void ToggleState()
        {
            if (Delegate != null)
            {
                Delegate.ToggleState(this);
            }
        }
    }
}
