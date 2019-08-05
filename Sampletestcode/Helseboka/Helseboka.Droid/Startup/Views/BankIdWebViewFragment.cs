
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using Helseboka.Core.Common.Constant;
using Helseboka.Core.Login.Interface;
using Helseboka.Droid.Common.Views;

namespace Helseboka.Droid.Startup.Views
{
    public class BankIdWebViewFragment : BaseFragment
    {
        WebView bankidWebView;

        private ILoginPresenter Presenter
        {
            get => presenter as ILoginPresenter;
        }

        public BankIdWebViewFragment(ILoginPresenter presenter)
        {
            this.presenter = presenter;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_bankid_webview, null);

            view.KeyPress -= View_KeyPress;
            view.KeyPress += View_KeyPress;

            bankidWebView = view.FindViewById<WebView>(Resource.Id.webview);
            bankidWebView.Settings.JavaScriptEnabled = true;
            bankidWebView.SetWebViewClient(new CustomWebViewClient(Presenter));

            return view;
        }

        void View_KeyPress(object sender, View.KeyEventArgs e)
        {
            if (e.KeyCode == Keycode.Back)
            {
                Presenter.GoBackToBankIdOption();
            }
        }


        public override void OnResume()
        {
            base.OnResume();
            var url = Presenter.GetAuthURL();
            bankidWebView.LoadUrl(url);
        }
    }

    public class CustomWebViewClient : WebViewClient
    {
        ILoginPresenter presenter;
        public CustomWebViewClient(ILoginPresenter presenter)
        {
            this.presenter = presenter;
        }

        public override void OnPageStarted(WebView view, string url, Bitmap favicon)
        {
            base.OnPageStarted(view, url, favicon);
        }

        public override bool ShouldOverrideUrlLoading(WebView view, string url)
        {
            if (url.StartsWith(AppConstant.AppScheme, StringComparison.OrdinalIgnoreCase))
            {
                presenter.CheckAuthResponse(url);
                return true;
            }
            else
            {
                return base.ShouldOverrideUrlLoading(view, url);
            }
        }

        public override bool ShouldOverrideUrlLoading(WebView view, IWebResourceRequest request)
        {
            var url = request.Url.ToString();

            if (url.StartsWith(AppConstant.AppScheme, StringComparison.OrdinalIgnoreCase))
            {
                presenter.CheckAuthResponse(url);
                return true;
            }
            else
            {
                return base.ShouldOverrideUrlLoading(view, request);
            }
        }
    }
}
