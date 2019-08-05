using System;
using System.ComponentModel;
using CoreGraphics;
using Foundation;
using Helseboka.iOS.Common.Constant;
using UIKit;
using Helseboka.iOS.Common.Extension;
using Xamarin.TTTAttributedLabel;

namespace Helseboka.iOS.Common.View
{
    [Register("BaseUILabel"), DesignTimeVisible(true)]
    public class BaseUILabel : UILabel
    {
        private UIEdgeInsets padding = new UIEdgeInsets(0, 0, 0, 0);
        public UIEdgeInsets Padding
        {
            get => padding;
            set
            {
                padding = value;
                LayoutSubviews();
            }
        }
        
        public BaseUILabel()
        {
            CommontInitialization();
        }

        public BaseUILabel(IntPtr ptr) : base(ptr)
        {
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            CommontInitialization();
        }

        private void CommontInitialization()
        {
        }

        public override void DrawText(CGRect rect)
        {
            base.DrawText(padding.InsetRect(rect));
        }

        public override CGSize IntrinsicContentSize
        {
            get
            {
                var originalSize = base.IntrinsicContentSize;

                originalSize.Width += (padding.Left + padding.Right);
                originalSize.Height += (padding.Top + padding.Bottom);

                return originalSize;
            }
        }
    }

	// Page title label style
	[Register("PageTitleLabel"), DesignTimeVisible(true)]
    public class PageTitleLabel : BaseUILabel
    {
		public PageTitleLabel()
        {
			CommontInitialization();
        }

		public PageTitleLabel(IntPtr ptr) : base(ptr) 
		{
		}

		public override void AwakeFromNib()
		{
			base.AwakeFromNib();
			CommontInitialization();
		}

		private void CommontInitialization()
		{
            this.Font = Fonts.GetBoldFont(24);
            this.TextColor = Colors.TitleTextColor;
		}
	}

    // Page title label style
    [Register("LinkableLabel"), DesignTimeVisible(true)]
    public class LinkableLabel : TTTAttributedLabel
    {
        public LinkableLabel()
        {
            CommontInitialization();
        }

        public LinkableLabel(IntPtr ptr) : base(ptr)
        {
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            CommontInitialization();
        }

        private void CommontInitialization()
        {
            this.EnabledTextCheckingTypes = NSTextCheckingType.Link;
        }
    }

    public class LinkDelegate : Xamarin.TTTAttributedLabel.TTTAttributedLabelDelegate
    {
        private Action<NSUrl> OnLinkTap;

        public LinkDelegate(Action<NSUrl> onLinkTap)
        {
            this.OnLinkTap = onLinkTap;
        }

        public override void DidSelectLinkWithURL(TTTAttributedLabel label, NSUrl url)
        {
            OnLinkTap?.Invoke(url);
        }
    }

    // Title text in page
	[Register("TitleLabel"), DesignTimeVisible(true)]
    public class TitleLabel : BaseUILabel
    {
        public TitleLabel()
        {
            CommontInitialization();
        }

        public TitleLabel(IntPtr ptr) : base(ptr)
        {
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            CommontInitialization();
        }

        private void CommontInitialization()
        {
            this.Font = Fonts.GetBoldFont(16);
            this.TextColor = Colors.TitleTextColor;
        }
    }

	// Title text in page
	[Register("DescriptionLabel"), DesignTimeVisible(true)]
    public class DescriptionLabel : BaseUILabel
    {
		public DescriptionLabel()
        {
            CommontInitialization();
        }

		public DescriptionLabel(IntPtr ptr) : base(ptr)
        {
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            CommontInitialization();
        }

        private void CommontInitialization()
        {
            this.Font = Fonts.GetNormalFont(14);
			this.TextColor = Colors.DescriptionLabelTextColor;
        }
    }

    // Title text in page
    [Register("HelpDescriptionLabel"), DesignTimeVisible(true)]
    public class HelpDescriptionLabel : BaseUILabel
    {
        public HelpDescriptionLabel()
        {
            CommontInitialization();
        }

        public HelpDescriptionLabel(IntPtr ptr) : base(ptr)
        {
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            CommontInitialization();
        }

        private void CommontInitialization()
        {
            this.Font = Fonts.GetMediumFont(14);
            this.TextColor = Colors.DescriptionLabelTextColor;
        }
    }

	// Help Text in BankId page
	[Register("HelpTextLabel"), DesignTimeVisible(true)]
    public class HelpTextLabel : BaseUILabel
    {
		public HelpTextLabel()
        {
            CommontInitialization();
        }

		public HelpTextLabel(IntPtr ptr) : base(ptr)
        {
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            CommontInitialization();
        }

        private void CommontInitialization()
        {
            this.Font = Fonts.GetMediumFont(16);
			this.TextColor = Colors.LoginHelpTextColor;
        }
    }

    // Title for chat page
    [Register("ChatTitleLabel"), DesignTimeVisible(true)]
    public class ChatTitleLabel : BaseUILabel
    {
        public ChatTitleLabel()
        {
            CommontInitialization();
        }

        public ChatTitleLabel(IntPtr ptr) : base(ptr)
        {
            CommontInitialization();
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            CommontInitialization();
        }

        private void CommontInitialization()
        {
            this.Font = Fonts.GetMediumFont(14);
            this.TextColor = Colors.TitleTextColor;
        }
    }

    // message for chat page
    [Register("ChatMessageLabel"), DesignTimeVisible(true)]
    public class ChatMessageLabel : BaseUILabel
    {
        public ChatMessageLabel()
        {
            CommontInitialization();
        }

        public ChatMessageLabel(IntPtr ptr) : base(ptr)
        {
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            CommontInitialization();
        }

        private void CommontInitialization()
        {
            this.Font = Fonts.GetMediumFont(15);
            this.TextColor = Colors.SentMessageTextColor;
        }
    }

    // Date for chat page
    [Register("ChatDateLabel"), DesignTimeVisible(true)]
    public class ChatDateLabel : BaseUILabel
    {
        public ChatDateLabel()
        {
            CommontInitialization();
        }

        public ChatDateLabel(IntPtr ptr) : base(ptr)
        {
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            CommontInitialization();
        }

        private void CommontInitialization()
        {
            this.Font = Fonts.GetMediumFont(12);
            this.TextColor = Colors.SentMessageTextColor;
        }
    }

    // Selectable field e.g. Doctor Selection
    [Register("SelectableLable"), DesignTimeVisible(true)]
    public class SelectableLable : BaseUILabel
    {
        private bool isEnabled = false;
        [Export("IsEnabled"), Browsable(true)]
        public bool IsEnabled
        {
            get => isEnabled;
            set
            {
                isEnabled = value;
                CommontInitialization();
            }
        }

        public SelectableLable()
        {
            CommontInitialization();
        }

        public SelectableLable(IntPtr ptr) : base(ptr)
        {
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            CommontInitialization();
        }

        private void CommontInitialization()
        {
            this.BackgroundColor = Colors.FillColor;
            this.Lines = 0;
            this.LineBreakMode = UILineBreakMode.WordWrap;
            if (isEnabled)
            {
                this.Font = Fonts.GetBoldFont(16);
                this.TextColor = Colors.TitleTextColor;
                //this.SetBackgroundImage(UIImage.FromBundle("Textbox-active-background"));
                this.Layer.BorderColor = Colors.SelectedLabelBorderColor.CGColor;
                this.Layer.BorderWidth = 2;
            }
            else
            {
                this.Font = Fonts.GetMediumFont(16);
                this.TextColor = Colors.LoginHelpTextColor;
                //this.SetBackgroundImage(UIImage.FromBundle("Textbox-inactive-background"));
                this.Layer.BorderColor = Colors.UnselectedLabelBorderColor.CGColor;
                this.Layer.BorderWidth = 1;
            }
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            this.Layer.CornerRadius = this.Frame.Height / 2;
        }
    }


    [Register("AlarmLabel"), DesignTimeVisible(true)]
    public class AlarmLabel : BaseUILabel
    {
        public AlarmLabel()
        {
            CommontInitialization();
        }

        public AlarmLabel(IntPtr ptr) : base(ptr)
        {
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            CommontInitialization();
        }

        private void CommontInitialization()
        {
            this.Font = Fonts.GetMediumFont(15);
            this.TextColor = Colors.TitleTextColor;
        }
    }

    [Register("AddMoreMedicineLabel"), DesignTimeVisible(true)]
    public class AddMoreMedicineLabel : BaseUILabel
    {
        public AddMoreMedicineLabel()
        {
            CommontInitialization();
        }

        public AddMoreMedicineLabel(IntPtr ptr) : base(ptr)
        {
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            CommontInitialization();
        }

        private void CommontInitialization()
        {
            this.Font = Fonts.GetBoldFont(18);
            this.TextColor = Colors.AddMoreMedicineLabelColor;
        }
    }

}
