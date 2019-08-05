
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Helseboka.Droid.Common.Views;
using Helseboka.Core.Common.Extension;
using Helseboka.Droid.Common.Utils;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Graphics;
using Helseboka.Core.Startup.Interface;
using Me.Relex;
using Helseboka.Core.Resources.StringResources;
using static Android.Views.View;
using static Android.Views.ViewGroup;

namespace Helseboka.Droid.Startup.Views
{
    public class OnboardingFragment : BaseFragment
    {
        TextView continueText;
        LinearLayout tabDots;
        ViewPager pager;
        ImageView leftArrow;
        ImageView rightArrow;
        OnboardingPagerAdapter Adapter;

        private ISplashPresenter Presenter
        {
            get => presenter as ISplashPresenter;
        }

        public OnboardingFragment(ISplashPresenter presenter)
        {
            this.presenter = presenter;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_onboarding, null);
            continueText = view.FindViewById<TextView>(Resource.Id.continueText);
            tabDots = view.FindViewById<LinearLayout>(Resource.Id.tabDots);
            pager = view.FindViewById<ViewPager>(Resource.Id.pager);
            leftArrow = view.FindViewById<ImageView>(Resource.Id.leftArrow);
            rightArrow = view.FindViewById<ImageView>(Resource.Id.rightArrow);

            tabDots.GetChildAt(0).Selected = true;

            continueText.Click += ContinueText_Click;
            leftArrow.Click += LeftArrow_Click;
            rightArrow.Click += RightArrow_Click;

            pager.PageSelected += Pager_PageSelected;
            Adapter = new OnboardingPagerAdapter(Activity);
            pager.Adapter = Adapter;

            leftArrow.Visibility = ViewStates.Invisible;
            continueText.SetBackgroundResource(Resource.Drawable.large_button_disabled_background);
            continueText.SetTextColor(Color.ParseColor("#6b6b6b"));
            continueText.Text = AppResources.OnBoardingNextButton;

            return view;
        }

        void LeftArrow_Click(object sender, EventArgs e)
        {
            if (pager.CurrentItem > 0)
            {
                pager.CurrentItem -= 1;
            }
        }

        void RightArrow_Click(object sender, EventArgs e)
        {
            if(pager.CurrentItem < 2)
            {
                pager.CurrentItem += 1;
            }
        }


        void Pager_PageSelected(object sender, ViewPager.PageSelectedEventArgs e)
        {
            if(rightArrow.LayoutParameters is MarginLayoutParams layoutParams)
            {
                layoutParams.BottomMargin = (int)Adapter.TitleHeights[e.Position] + 30.ConvertToPixel(Activity);
            }

            leftArrow.Visibility = ViewStates.Visible;
            rightArrow.Visibility = ViewStates.Visible;
            if(e.Position == 2)
            {
                continueText.SetBackgroundResource(Resource.Drawable.large_button_enabled_background);
                continueText.SetTextColor(Color.White);
                continueText.Text = AppResources.OnBoardingGoToButton;

                rightArrow.Visibility = ViewStates.Invisible;
            }
            else
            {
                continueText.SetBackgroundResource(Resource.Drawable.large_button_disabled_background);
                continueText.SetTextColor(Color.ParseColor("#6b6b6b"));
                continueText.Text = AppResources.OnBoardingNextButton;
                rightArrow.Visibility = ViewStates.Visible;
                if(e.Position == 0)
                {
                    leftArrow.Visibility = ViewStates.Invisible;
                }
            }

            for (int index = 0; index < tabDots.ChildCount; index++)
            {
                if(index == e.Position)
                {
                    tabDots.GetChildAt(index).Selected = true;
                }
                else
                {
                    tabDots.GetChildAt(index).Selected = false;
                }
            }
        }

        void ContinueText_Click(object sender, EventArgs e)
        {
            Presenter.StartRegistrationFlow();

            //if(pager.CurrentItem == 2)
            //{
            //    Presenter.StartRegistrationFlow();
            //}
            //else
            //{
            //    pager.CurrentItem += 1;
            //}
        }
    }

    public class OnboardingPagerAdapter : PagerAdapter
    {
        Context context;
        public List<Double> TitleHeights = new List<double> { 0, 0, 0 };

        public OnboardingPagerAdapter(Context context)
        {
            this.context = context;
        }

        public override int Count
        {
            get => 3;
        }

        public override bool IsViewFromObject(View view, Java.Lang.Object obj)
        {
            return view == obj;
        }

        public override Java.Lang.Object InstantiateItem(ViewGroup container, int position)
        {
            var inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
            var view = inflater.Inflate(Resource.Layout.template_onboarding_item, null);
            String titleText;
            int imageResourceId;
            switch(position)
            {
                case 0: titleText = AppResources.OnBoardingFirstTitle;
                    imageResourceId = Resource.Drawable.onboarding_illustration_1;
                    break;
                case 1:
                    titleText = AppResources.OnBoardingSecondTitle;
                    imageResourceId = Resource.Drawable.onboarding_illustration_2;
                    break;
                default:
                    titleText = AppResources.OnBoardingThirdTitle;
                    imageResourceId = Resource.Drawable.onboarding_illustration_3;
                    break;
            }
            var titleTextView = view.FindViewById<TextView>(Resource.Id.pageTitle);
            titleTextView.Text = titleText;

            titleTextView.Measure(0, 0);
            TitleHeights[position] = titleTextView.MeasuredHeight;


            view.FindViewById<ImageView>(Resource.Id.illustrationImage).SetImageResource(imageResourceId);

            container.AddView(view);
            return view;
        }

        public override void DestroyItem(ViewGroup container, int position, Java.Lang.Object obj)
        {
            container.RemoveView(obj as View);
        }

    }
}
