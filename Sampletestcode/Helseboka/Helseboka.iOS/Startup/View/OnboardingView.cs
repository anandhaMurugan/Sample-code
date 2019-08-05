using System;
using System.Collections.Generic;
using CoreGraphics;
using Helseboka.iOS.Common.View;
using Helseboka.iOS.Common.Extension;
using UIKit;
using Helseboka.Core.Startup.Interface;
using Helseboka.Core.Resources.StringResources;
using Helseboka.iOS.Common.Constant;

namespace Helseboka.iOS.Startup.View
{
    public partial class OnboardingView : BaseView
    {
        private UIPageViewController pageViewController;
        public static readonly String Identifier = "OnboardingView";

        public List<OnboardingContentView> pages = new List<OnboardingContentView>();

        public ISplashPresenter Presenter
        {
            get => presenter as ISplashPresenter;
            set => presenter = value;
        }

        public OnboardingView() { }

        public OnboardingView(IntPtr handler) : base(handler) { }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            pages.Add(new OnboardingContentView(0, AppResources.OnBoardingFirstTitle, "Onboard-image-1"));
            pages.Add(new OnboardingContentView(1, AppResources.OnBoardingSecondTitle, "Onboard-image-2"));
            pages.Add(new OnboardingContentView(2, AppResources.OnBoardingThirdTitle, "Onboard-image-3"));

            pageViewController = new UIPageViewController(UIPageViewControllerTransitionStyle.Scroll, UIPageViewControllerNavigationOrientation.Horizontal);
            pageViewController.SetViewControllers(new UIViewController[] { pages[0] as UIViewController }, UIPageViewControllerNavigationDirection.Forward, false, null);
            pageViewController.DataSource = new PageDataSource(pages);
            EmbedView(ContainerView, this, pageViewController);
            pageViewController.DidFinishAnimating += PageViewController_DidFinishAnimating;

            PageControl.Pages = pages.Count;
            PageControl.Transform = CGAffineTransform.MakeScale(2, 2);

            BackwardButton.Hidden = true;
            ForwardButton.Hidden = true;
            HoppOverButton.SetTitle(AppResources.OnBoardingNextButton, UIControlState.Normal);
            HoppOverButton.SetBackgroundImage(UIImage.FromBundle("Standard-button-disabled-background"), UIControlState.Normal);
            HoppOverButton.SetTitleColor(Colors.OnboardingButtonTitleColor, UIControlState.Normal);
        }

        partial void HoppOver_Tapped(UIButton sender)
        {
            Presenter.StartRegistrationFlow();

            //var contr = pageViewController.ViewControllers[0] as OnboardingContentView;
            //if(contr.Index == 2)
            //{
            //    Presenter.StartRegistrationFlow();
            //}
            //else
            //{
            //    pageViewController.SetViewControllers(new UIViewController[] { pages[contr.Index + 1] }, UIPageViewControllerNavigationDirection.Forward, true, (finished) => SetPageStyleAccordingToIndex());
            //}
        }

        partial void BackwardButton_Tapped(UIButton sender)
        {
            var contr = pageViewController.ViewControllers[0] as OnboardingContentView;
            if(contr.Index > 0)
            {
                pageViewController.SetViewControllers(new UIViewController[] { pages[contr.Index - 1] }, UIPageViewControllerNavigationDirection.Reverse, true, (finished) => SetPageStyleAccordingToIndex());
            }
        }

        partial void ForwardButton_Tapped(UIButton sender)
        {
            var contr = pageViewController.ViewControllers[0] as OnboardingContentView;
            if (contr.Index < pages.Count - 1)
            {
                pageViewController.SetViewControllers(new UIViewController[] { pages[contr.Index + 1] }, UIPageViewControllerNavigationDirection.Forward, true,(finished) => SetPageStyleAccordingToIndex());
            }
        }

        private void PageViewController_DidFinishAnimating(object sender, UIPageViewFinishedAnimationEventArgs e)
        {
            SetPageStyleAccordingToIndex();
        }

        private void SetPageStyleAccordingToIndex()
        {
            var contr = pageViewController.ViewControllers[0] as OnboardingContentView;
            PageControl.CurrentPage = contr.Index;
            BackwardButton.Hidden = false;
            ForwardButton.Hidden = false;
            if (contr.Index == 2)
            {
                ForwardButton.Hidden = true;
                HoppOverButton.SetTitle(AppResources.OnBoardingGoToButton, UIControlState.Normal);
                HoppOverButton.SetTitleColor(UIColor.White, UIControlState.Normal);
                HoppOverButton.SetBackgroundImage(UIImage.FromBundle("Standard-button-background"), UIControlState.Normal);

            }
            else
            {
                HoppOverButton.SetTitle(AppResources.OnBoardingNextButton, UIControlState.Normal);
                HoppOverButton.SetTitleColor(Colors.OnboardingButtonTitleColor, UIControlState.Normal);
                HoppOverButton.SetBackgroundImage(UIImage.FromBundle("Standard-button-disabled-background"), UIControlState.Normal);

                if (contr.Index == 0)
                {
                    BackwardButton.Hidden = true;
                }
            }

            BackwardButtonCenterYConstraint.Constant = contr.PageTitleLabel.Frame.Height / 2;
            this.View.UpdateConstraints();
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            var contr = pageViewController.ViewControllers[0] as OnboardingContentView;
            BackwardButtonCenterYConstraint.Constant = contr.PageTitleLabel.Frame.Height / 2;
            this.View.UpdateConstraints();
            ForwardButton.Hidden = false;
        }
    }

    public class PageDataSource : UIPageViewControllerDataSource
    {
        List<OnboardingContentView> pages;

        public PageDataSource(List<OnboardingContentView> pages)
        {
            this.pages = pages;
        }

        override public UIViewController GetPreviousViewController(UIPageViewController pageViewController, UIViewController referenceViewController)
        {
            var currentPage = referenceViewController as OnboardingContentView;
            OnboardingContentView pageToReturn = null;

            if (currentPage.Index == 0)
            {
                return null;
                //pageToReturn = pages[pages.Count - 1];
            }
            else
            {
                pageToReturn = pages[currentPage.Index - 1];
            }

            // NOTE: If the same view controller is returned, UIPageViewController will break and show black screen
            return pageToReturn != currentPage ? pageToReturn : null;
        }

        override public UIViewController GetNextViewController(UIPageViewController pageViewController, UIViewController referenceViewController)
        {
            var currentPage = referenceViewController as OnboardingContentView;
            OnboardingContentView pageToReturn = pages[(currentPage.Index + 1) % pages.Count];

            if (currentPage.Index == pages.Count - 1)
            {
                return null;
            }

            return pageToReturn != currentPage ? pageToReturn : null;
        }


    }
}

