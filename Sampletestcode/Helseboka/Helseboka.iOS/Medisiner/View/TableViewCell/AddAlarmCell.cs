using System;

using Foundation;
using Helseboka.iOS.Common.Constant;
using Helseboka.iOS.Common.TableViewCell;
using UIKit;

namespace Helseboka.iOS.Medisiner.View.TableViewCell
{
    public partial class AddAlarmCell : BaseTableViewCell
    {
        public event EventHandler<String> DeleteTapped;

        public static readonly NSString Key = new NSString("AddAlarmCell");

        protected AddAlarmCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            ConfigureSelectionLabel(AlarmLabel);

            AlarmLabel.Font = Fonts.GetMediumFont(15);
            AlarmLabel.TextColor = Colors.AlarmLabelTextColor;

            SelectionStyle = UITableViewCellSelectionStyle.None;
        }

        public void Configure(String text)
        {
            AlarmLabel.Text = text;
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            AlarmLabel.Layer.CornerRadius = AlarmLabel.Frame.Height / 2;
        }

        partial void Delete_Tapped(UIButton sender)
        {
            DeleteTapped?.Invoke(this, AlarmLabel.Text);
        }
    }
}
