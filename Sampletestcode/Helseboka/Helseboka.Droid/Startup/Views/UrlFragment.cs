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
using Helseboka.Core.Common.CommonImpl;
using Helseboka.Core.Common.EnumDefinitions;
using Helseboka.Core.Startup.Interface;
using Helseboka.Droid.Common.EnumDefinitions;
using Helseboka.Droid.Common.Views;

namespace Helseboka.Droid.Startup.Views
{
    public class UrlFragment : BaseFragment
    {
       
        private ImageView devUrlcheckbox;
        private ImageView testUrlcheckbox;
        private ImageView stagingUrlcheckbox;
        private ImageView prodUrlcheckbox;
        
        private ImageView preProdBankCheckbox;
        private ImageView prodBankCheckbox;

        private TextView errorlabel;
        private Button continueBtn;
        private Dictionary<ConfigTypes,bool> Envicheckboxes;
        private Dictionary<BankIdConfigTypes, bool> BankIdcheckboxes;
        private IUrlPresenter Presenter
        {
            get => presenter as IUrlPresenter;
        }

        public UrlFragment(IUrlPresenter urlPresenter)
        {
            this.presenter = urlPresenter;
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
            //  return base.OnCreateView(inflater, container, savedInstanceState);
            var view= inflater.Inflate(Resource.Layout.fragment_testurl_selection, null);
            devUrlcheckbox = view.FindViewById<ImageView>(Resource.Id.devUrlSelection);
            testUrlcheckbox=view.FindViewById<ImageView>(Resource.Id.testUrlSelection);
            stagingUrlcheckbox= view.FindViewById<ImageView>(Resource.Id.stagingUrlSelection);
            prodUrlcheckbox = view.FindViewById<ImageView>(Resource.Id.prodUrlSelection);

            preProdBankCheckbox= view.FindViewById<ImageView>(Resource.Id.preProdBankUrlSelection);
            prodBankCheckbox = view.FindViewById<ImageView>(Resource.Id.prodBankUrlSelection);

            errorlabel = view.FindViewById<TextView>(Resource.Id.errorlabeldevsettings);
            continueBtn = view.FindViewById<Button>(Resource.Id.Continuebtn);

            devUrlcheckbox.Click += Devurlcheckbox_Click;
            testUrlcheckbox.Click += TestUrlcheckbox_Click;
            stagingUrlcheckbox.Click += StagingUrlcheckbox_Click;
            prodUrlcheckbox.Click += ProdUrlcheckbox_clicked;

            preProdBankCheckbox.Click += PreProdBankCheckbox_Click;
            prodBankCheckbox.Click += ProdBankCheckbox_Click;

            continueBtn.Click += ContinueBtn_Click;
            return view;
        }

       
        private void Devurlcheckbox_Click(object sender, EventArgs e)
        {
            devUrlcheckbox.Selected = !devUrlcheckbox.Selected;
            testUrlcheckbox.Selected = false;
            stagingUrlcheckbox.Selected = false;
            prodUrlcheckbox.Selected = false;
            errorlabel.Visibility = ViewStates.Invisible;
        }

        private void TestUrlcheckbox_Click(object sender, EventArgs e)
        {
            testUrlcheckbox.Selected = !testUrlcheckbox.Selected;
            devUrlcheckbox.Selected = false;
            stagingUrlcheckbox.Selected = false;
            prodUrlcheckbox.Selected = false;
            errorlabel.Visibility = ViewStates.Invisible;

        }

        private void StagingUrlcheckbox_Click(object sender, EventArgs e)
        {
            stagingUrlcheckbox.Selected = !stagingUrlcheckbox.Selected;
            devUrlcheckbox.Selected = false;
            testUrlcheckbox.Selected = false;
            prodUrlcheckbox.Selected = false;
            errorlabel.Visibility = ViewStates.Invisible;
        }

        private void ProdUrlcheckbox_clicked(object sender, EventArgs e)
        {
            prodUrlcheckbox.Selected = !prodUrlcheckbox.Selected;
            devUrlcheckbox.Selected = false;
            testUrlcheckbox.Selected = false;
            stagingUrlcheckbox.Selected = false;

        }

        private void PreProdBankCheckbox_Click(object sender, EventArgs e)
        {
            preProdBankCheckbox.Selected = !preProdBankCheckbox.Selected;
            prodBankCheckbox.Selected = false;
            errorlabel.Visibility = ViewStates.Invisible;
        }

        private void ProdBankCheckbox_Click(object sender, EventArgs e)
        {
            prodBankCheckbox.Selected = !prodBankCheckbox.Selected;
            preProdBankCheckbox.Selected = false;
            errorlabel.Visibility = ViewStates.Invisible;
        }

        private void ContinueBtn_Click(object sender, EventArgs e)
        {
            Envicheckboxes = new Dictionary<ConfigTypes, bool>
            {
                {ConfigTypes.Dev, devUrlcheckbox.Selected },
                {ConfigTypes.Test, testUrlcheckbox.Selected },
                {ConfigTypes.Staging,stagingUrlcheckbox.Selected },
                {ConfigTypes.Prod,prodUrlcheckbox.Selected }
            };

            BankIdcheckboxes = new Dictionary<BankIdConfigTypes, bool>
            {
                {BankIdConfigTypes.PreProd,  preProdBankCheckbox.Selected },
                {BankIdConfigTypes.Prod,  prodBankCheckbox.Selected }
            };


            if (Envicheckboxes.Any(x => x.Value) && BankIdcheckboxes.Any(x => x.Value))
            {
                continueBtn.Enabled = false;
                errorlabel.Visibility = ViewStates.Invisible;
                var configParam = Envicheckboxes.FirstOrDefault(x => x.Value == true).Key;
                var bankIdParam = BankIdcheckboxes.FirstOrDefault(x => x.Value == true).Key;
                Presenter.DevconfigType(configParam, bankIdParam);
            }
            else
            {
                //display error here
                errorlabel.Visibility = ViewStates.Visible;
            }
          
        }

    }
}