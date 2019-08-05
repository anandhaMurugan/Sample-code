using System;
using System.Linq;
using Foundation;
using Helseboka.Core.Common.Constant;
using Helseboka.Core.Login.Interface;
using Helseboka.iOS.Common.View;
using UIKit;

namespace Helseboka.iOS.Login.View
{
	public partial class BankIdWebView : BaseView, IUIWebViewDelegate
    {
		public ILoginPresenter Presenter
        {
            get => presenter as ILoginPresenter;
            set => presenter = value;
        }

		public BankIdWebView() { }

		public BankIdWebView(IntPtr handler) : base(handler) { }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.

			LoadWebView();
			WebView.Delegate = this;
        }

        [Export("webView:shouldStartLoadWithRequest:navigationType:")]
		public bool ShouldStartLoad(UIWebView webView, NSUrlRequest request, UIWebViewNavigationType navigationType)
		{
			return Presenter.CheckAuthResponse(request.Url.AbsoluteString);
		}

		private void LoadWebView()
		{
			var request = new NSMutableUrlRequest(new NSUrl(Presenter.GetAuthURL()));

			var headers = Presenter.GetSecurityHeaders();
			request.Headers = NSDictionary.FromObjectsAndKeys(headers.Values.ToArray(), headers.Keys.ToArray());

			WebView.LoadRequest(request);
		}
    }
}

