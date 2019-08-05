
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
using Helseboka.Core.Startup.Interface;
using Helseboka.Droid.Common.Views;

namespace Helseboka.Droid.Startup.Views
{
    public class SplashFragment : BaseFragment
    {
        private ISplashPresenter Presenter
        {
            get => presenter as ISplashPresenter;
        }

        public SplashFragment(ISplashPresenter splashPresenter)
        {
            this.presenter = splashPresenter;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            return inflater.Inflate(Resource.Layout.fragment_startup_splash, null);
        }

        public override void OnResume()
        {
            base.OnResume();
            Presenter.DecideAndNavigateAppFlow();
        }
    }
}
