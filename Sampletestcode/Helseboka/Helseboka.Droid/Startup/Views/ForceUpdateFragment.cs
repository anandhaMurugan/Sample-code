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
using Helseboka.Core.Common.Constant;
using Helseboka.Core.Startup.UpdateVersion.Interface;
using Helseboka.Droid.Common.Views;

namespace Helseboka.Droid.Startup.Views
{
    
    public class ForceUpdateFragment : BaseFragment
    {
        private Button updateButton;
        private TextView closeOrNotNowButton;
        private TextView descriptionTextView;
        private bool shouldUpdate;
        private string descriptionText;

        private IUpdatePresenter Presenter
        {
            get => presenter as IUpdatePresenter;
        }

        public ForceUpdateFragment(IUpdatePresenter updatePresenter, bool shouldUpdate, string descriptionText)
        {
            this.presenter = updatePresenter;
            this.shouldUpdate = shouldUpdate;
            this.descriptionText = descriptionText;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_updateapp, null);
            updateButton = view.FindViewById<Button>(Resource.Id.updateButton);
            closeOrNotNowButton = view.FindViewById<TextView>(Resource.Id.closeOrNotNowButton);
            descriptionTextView = view.FindViewById<TextView>(Resource.Id.pageContent);

            descriptionTextView.Text = descriptionText;
            if (shouldUpdate)
            {
                closeOrNotNowButton.Visibility = ViewStates.Invisible;
            }
            else
            {
                closeOrNotNowButton.Text = Resources.GetString(Resource.String.update_NotNow_buttonText);
            }

            updateButton.Click += UpdateButton_Click;
            closeOrNotNowButton.Click += CloseOrNotNowButton_Click;
            return view;
        }

        private void CloseOrNotNowButton_Click(object sender, EventArgs e)
        {
            if (!shouldUpdate)
            {
                Presenter.ProceedCloseOrNotNowClicked();
            }
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            Android.Net.Uri authUri = Android.Net.Uri.Parse(APIEndPoints.GetUpdateAndroidUrl);
            var browserIntent = new Intent(Intent.ActionView, authUri);
            StartActivity(browserIntent);
        }
    }
}