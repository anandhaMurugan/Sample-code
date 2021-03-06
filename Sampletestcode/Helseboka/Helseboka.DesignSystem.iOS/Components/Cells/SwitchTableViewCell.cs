﻿using System;

using Foundation;
using Helseboka.DesignSystem.iOS.Constants;
using UIKit;

namespace Helseboka.DesignSystem.iOS.Components.Cells
{
    public interface ISwitchTableViewCellDelegate
    {
        void ToggleSwitchState(SwitchTableViewCell cell);
    }
    public partial class SwitchTableViewCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("SwitchTableViewCell");
        public static readonly UINib Nib;

        public ISwitchTableViewCellDelegate Delegate;

        public override UILabel TextLabel => Label;

        static SwitchTableViewCell()
        {
            Nib = UINib.FromName("SwitchTableViewCell", NSBundle.MainBundle);
        }

        protected SwitchTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            TextLabel.Font = Fonts.Medium(14);
            TextLabel.TextColor = Colors.Grey600;
            Switch.TouchUpInside += Switch_TouchUpInside;
            Switch.TintColor = Colors.Grey350;
            Switch.OnTintColor = Colors.Purple;
        }

        private void Switch_TouchUpInside(object sender, EventArgs e)
        {
            ToggleState();
        }

        public void SetTextAndSwitch(string text, bool switchSelected)
        {
            TextLabel.Text = text;
            Switch.On = switchSelected;
        }

        private void ToggleState()
        {
            if (Delegate != null)
            {
                Delegate.ToggleSwitchState(this);
            }
        }
    }
}
