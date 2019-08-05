using System;
using Helseboka.Core.Startup.Interface;
using Helseboka.iOS.Common.View;
using UIKit;
using Helseboka.Shared;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace Helseboka.iOS.Startup.View
{
    public partial class Splash : BaseView
    {
        public ISplashPresenter Presenter
        {
            get => presenter as ISplashPresenter;
            set => presenter = value;
        }

        public Splash(IntPtr ptr) : base(ptr)
        {
        }

        public Splash()
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            HideLoader();
            Presenter.DecideAndNavigateAppFlow();
#if TEST
            //var vc = new MainPage().CreateViewController();
            //NavigationController.PresentViewController(vc, true, null);
#else
            //Presenter.DecideAndNavigateAppFlow();
#endif
        }
    }
}

