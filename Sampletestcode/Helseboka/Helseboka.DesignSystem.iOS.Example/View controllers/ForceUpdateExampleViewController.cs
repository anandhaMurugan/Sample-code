using System;
using Helseboka.DesignSystem.iOS.Constants;
using UIKit;

namespace Helseboka.DesignSystem.iOS.Example.Viewcontrollers
{
    public partial class ForceUpdateExampleViewController : UIViewController
    {
        public ForceUpdateExampleViewController(IntPtr handle) : base(handle)
        {

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            TitleLabel.Font = Fonts.Bold(24);
            DescriptionLabel.Font = Fonts.Medium(16);
        }
    }
}