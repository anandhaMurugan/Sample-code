using System;

using Foundation;
using Helseboka.iOS.Common.Constant;
using Helseboka.iOS.Common.View;
using UIKit;

namespace Helseboka.iOS.Common.TableViewCell
{
    public partial class BaseTableViewCell : UITableViewCell
    {
        public BaseTableViewCell() : base() { }

        public BaseTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void MovedToSuperview()
        {
            base.MovedToSuperview();
            SelectionStyle = UITableViewCellSelectionStyle.None;
        }

        public void ConfigureSelectionLabel(BaseUILabel label)
        {
            label.Padding = new UIEdgeInsets(10, 20, 10, 20);
            label.Layer.BackgroundColor = Colors.FillColor.CGColor;
            label.Layer.BorderColor = Colors.UnselectedLabelBorderColor.CGColor;
            label.Layer.BorderWidth = 1;
        }
    }
}
