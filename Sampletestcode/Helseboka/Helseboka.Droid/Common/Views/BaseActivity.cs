
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Helseboka.Droid.Common.EnumDefinitions;
using Helseboka.Droid.Common.Interfaces;

namespace Helseboka.Droid.Common.Views
{
    [Activity()]
    public abstract class BaseActivity : AppCompatActivity
    {
        public abstract View RootView { get; }

        protected BaseFragment currentFragment;

        public void NavigateTo(int containerId, BaseFragment fragment, TransitionEffect effect = TransitionEffect.None)
        {
            currentFragment = fragment;
            Android.Support.V4.App.FragmentTransaction fragmentTx = SupportFragmentManager.BeginTransaction();
            if (effect == TransitionEffect.Push)
            {
                fragmentTx.SetCustomAnimations(Resource.Animation.slide_in_from_right, Resource.Animation.slide_out_to_left);
            }
            else if (effect == TransitionEffect.Pop)
            {
                fragmentTx.SetCustomAnimations(Resource.Animation.slide_in_from_left, Resource.Animation.slide_out_to_right);
            }
            fragmentTx.Replace(containerId, fragment);
            fragmentTx.Commit();
        }

        public override void OnBackPressed()
        {
            if (currentFragment != null)
            {
                if(!currentFragment.OnBackKeyPressed())
                {
                    base.OnBackPressed();
                }
            }
            else
            {
                base.OnBackPressed();
            }

        }
    }
}
