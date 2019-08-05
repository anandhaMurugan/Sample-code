using System;
using Foundation;
using Helseboka.iOS.Common.Constant;
using Helseboka.iOS.Common.TableViewCell;
using UIKit;

namespace Helseboka.iOS.Medisiner.View.TableViewCell
{
    public partial class ToggleAlarmCell : BaseTableViewCell
    {
        public static readonly NSString Key = new NSString("ToggleAlarmCell");
        protected ToggleAlarmCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public event EventHandler<Boolean> ToggleChanged;

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            ToggleAlarmSwitch.Layer.BorderColor = UIColor.FromRGB((byte)151, (byte)151, (byte)151).CGColor;
            ToggleAlarmSwitch.Layer.BorderWidth = (nfloat)1;


            ToggleAlarmSwitch.ValueChanged -= ToggleAlarmSwitch_ValueChanged;
            ToggleAlarmSwitch.ValueChanged += ToggleAlarmSwitch_ValueChanged;
        }

        private void ToggleAlarmSwitch_ValueChanged(object sender, EventArgs e)
        {
            var impactGenerator = new UIImpactFeedbackGenerator(UIImpactFeedbackStyle.Light);
            impactGenerator.ImpactOccurred();
            //if(ToggleAlarmSwitch.On)
            //{
            //    ToggleAlarmSwitch.ThumbTintColor = Colors.AlarmSwitchOnColor;
            //}
            //else
            //{
            //    ToggleAlarmSwitch.ThumbTintColor = Colors.AlarmSwitchOffColor;
            //}
            ToggleChanged?.Invoke(this, ToggleAlarmSwitch.On);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            ToggleAlarmSwitch.Layer.CornerRadius = ToggleAlarmSwitch.Frame.Height / 2;
        }

    }
}
