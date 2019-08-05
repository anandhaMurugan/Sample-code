using System;
using Helseboka.iOS.Common.Constant;
using UIKit;

namespace Helseboka.iOS.Home.View
{
	public partial class HomeTabView : UITabBarController
    {
		public HomeTabView() { }

		public HomeTabView(IntPtr ptr) : base(ptr) { }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.
			TabBar.TintColor = Colors.ThemeTurkise;
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}

