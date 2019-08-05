using System;

using Foundation;
using Helseboka.iOS.Common.Constant;
using Helseboka.iOS.Common.TableViewCell;
using Helseboka.iOS.Common.View;
using UIKit;

namespace Helseboka.iOS.Medisiner.View.TableViewCell
{
    public partial class DateSelectionCell : BaseTableViewCell
    {
        private UITapGestureRecognizer addmoreLabelTap;

        public event EventHandler AddMoreFrequencyTapped;

        public static readonly NSString Key = new NSString("DateSelectionCell");

        protected DateSelectionCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            if (addmoreLabelTap == null)
            {
                addmoreLabelTap = new UITapGestureRecognizer(AddMoreLabel_Tapped);
            }
            AddMoreLabel.RemoveGestureRecognizer(addmoreLabelTap);
            AddMoreLabel.AddGestureRecognizer(addmoreLabelTap);

            AddMoreButton.TouchUpInside -= AddMoreButton_TouchUpInside;
            AddMoreButton.TouchUpInside += AddMoreButton_TouchUpInside;
        }

        void AddMoreLabel_Tapped()
        {
            AddMoreFrequencyTapped?.Invoke(this, EventArgs.Empty);
        }

        void AddMoreButton_TouchUpInside(object sender, EventArgs e)
        {
            AddMoreFrequencyTapped?.Invoke(this, EventArgs.Empty);
        }
    }
}
