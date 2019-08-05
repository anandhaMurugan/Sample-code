using System;
using System.ComponentModel;
using Foundation;
using Helseboka.iOS.Common.Constant;
using UIKit;

namespace Helseboka.iOS.Common.View
{
	[Register("BasePrimaryActionButton"), DesignTimeVisible(true)]
    public class BasePrimaryActionButton : UIButton
    {
		public BasePrimaryActionButton()
        {
            CommontInitialization();
        }

		public BasePrimaryActionButton(IntPtr ptr) : base(ptr)
        {
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            CommontInitialization();
        }

		protected void CommontInitialization()
        {
            this.Font = Fonts.GetMediumFont(18);
            this.SetTitleColor(UIColor.White, UIControlState.Normal);
        }
    }

	[Register("PrimaryActionButton"), DesignTimeVisible(true)]
	public class PrimaryActionButton : BasePrimaryActionButton
    {
		public PrimaryActionButton() : base() 
		{
			Initialization();
		}

		public override void AwakeFromNib()
		{
			base.AwakeFromNib();
			Initialization();
		}

		public PrimaryActionButton(IntPtr ptr) : base (ptr) { }

		protected void Initialization()
		{
            this.SetTitleColor(UIColor.White, UIControlState.Normal);
            this.SetTitleColor(Colors.DisabledButtonTextColor, UIControlState.Disabled);
			this.SetBackgroundImage(UIImage.FromBundle("Standard-button-background"), UIControlState.Normal);
			this.SetBackgroundImage(UIImage.FromBundle("Standard-button-disabled-background"), UIControlState.Disabled);
		}
	}

	[Register("SignUpSecondaryActionButton"), DesignTimeVisible(true)]
    public class SignUpSecondaryActionButton : UIButton
    {
		public SignUpSecondaryActionButton()
        {
            CommontInitialization();
        }

		public SignUpSecondaryActionButton(IntPtr ptr) : base(ptr)
        {
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            CommontInitialization();
        }

        protected void CommontInitialization()
        {
            this.Font = Fonts.GetMediumFont(16);
			this.SetTitleColor(Colors.CancelButtonTextColor, UIControlState.Normal);
        }
    }

    [Register("SmallActionButton"), DesignTimeVisible(true)]
    public class SmallActionButton : UIButton
    {
        public SmallActionButton()
        {
            CommontInitialization();
        }

        public SmallActionButton(IntPtr ptr) : base(ptr)
        {
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            CommontInitialization();
        }

        protected void CommontInitialization()
        {
            this.Font = Fonts.GetMediumFont(16);
            this.SetTitleColor(UIColor.White, UIControlState.Normal);
            this.SetTitleColor(Colors.DisabledButtonTextColor, UIControlState.Disabled);
            this.SetBackgroundImage(UIImage.FromBundle("Small-action-button-enabled-background"), UIControlState.Normal);
            this.SetBackgroundImage(UIImage.FromBundle("Small-action-button-disabled-background"), UIControlState.Disabled);
        }
    }

    [Register("MediumActionButton"), DesignTimeVisible(true)]
    public class MediumActionButton : UIButton
    {
        public MediumActionButton()
        {
            CommontInitialization();
        }

        public MediumActionButton(IntPtr ptr) : base(ptr)
        {
            
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            CommontInitialization();
        }

        protected void CommontInitialization()
        {
            this.Font = Fonts.GetMediumFont(17);
            this.SetTitleColor(UIColor.White, UIControlState.Normal);
            this.SetTitleColor(Colors.DisabledButtonTextColor, UIControlState.Disabled);
            this.SetBackgroundImage(UIImage.FromBundle("Medium-action-button-enabled-background"), UIControlState.Normal);
            this.SetBackgroundImage(UIImage.FromBundle("Medium-action-button-disabled-background"), UIControlState.Disabled);
        }
    }

    [Register("CheckBox"), DesignTimeVisible(true)]
    public class CheckBox : UIButton
    {
        public event EventHandler<Boolean> SelectionChanged;
        public CheckBox()
        {
            CommontInitialization();
        }

        public CheckBox(IntPtr ptr) : base(ptr)
        {
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            CommontInitialization();
        }

        protected void CommontInitialization()
        {
            this.Font = Fonts.GetMediumFont(15);
            this.SetTitleColor(Colors.LoginHelpTextColor, UIControlState.Normal);
            this.SetImage(UIImage.FromBundle("Checkbox-checked"), UIControlState.Selected);
            this.SetImage(UIImage.FromBundle("Checkbox-unchecked"), UIControlState.Normal);
            this.TouchUpInside += (sender, e) => {
                this.Selected = !this.Selected;
                SelectionChanged?.Invoke(this, this.Selected);
            };
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            var imageSize = ImageView.Frame.Size;
            var titleSize = TitleLabel.Frame.Size;

            var padding = 0;

            ImageEdgeInsets = new UIEdgeInsets( - (titleSize.Height + padding) , 0, 0, - titleSize.Width);
            TitleEdgeInsets = new UIEdgeInsets( 0, - imageSize.Width, - 35, 0);
        }
    }
}
