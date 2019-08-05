using System;

using UIKit;

namespace Helseboka.iOS.Startup.View
{
    public partial class OnboardingContentView : UIViewController
    {
        private String pageTitle;
        private String imageName;
        private int index = -1;
        public int Index
        {
            get
            {
                return index;
            }
        }

        public UIImageView IllustrationImage
        {
            get => ImageView;
        }

        public UILabel PageTitleLabel
        {
            get => TitleLabel;
        }

        public OnboardingContentView(int pageIndex, String title, String imageName) : base("OnboardingContentView", null)
        {
            this.index = pageIndex;
            this.pageTitle = title;
            this.imageName = imageName;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            TitleLabel.Text = pageTitle;
            ImageView.Image = UIImage.FromBundle(imageName);
            ImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
        }
    }
}

