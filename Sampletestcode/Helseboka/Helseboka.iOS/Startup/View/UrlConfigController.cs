using Foundation;
using Helseboka.Core.Common.EnumDefinitions;
using Helseboka.Core.Startup.Interface;
using Helseboka.iOS.Common.View;
using System;
using System.Collections.Generic;
using System.Linq;
using UIKit;

namespace Helseboka.iOS
{
    public partial class UrlConfigController : BaseView
    {

        private Dictionary<ConfigTypes, bool> Envicheckboxes;
        private Dictionary<BankIdConfigTypes, bool> BankIdcheckboxes;

        public IUrlPresenter Presenter
        {
            get => presenter as IUrlPresenter;
            set => presenter = value;
        }
        public UrlConfigController (IntPtr handle) : base (handle){}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            ErrLabel.Hidden = true;
            DevCheckBox.SelectionChanged += DevCheckBox_SelectionChanged;
            TestCheckBox.SelectionChanged+= TestCheckBox_SelectionChanged;
            StagCheckBox.SelectionChanged+=StagCheckBox_SelectionChanged;
            ProdCheckBox.SelectionChanged+=ProdCheckBox_SelectionChanged;

            PreProdBankidCheckBox.SelectionChanged+=PreProdBankidCheckBox_SelectionChanged;
            ProdBankidCheckBox.SelectionChanged+=ProdBankidCheckBox_SelectionChanged;
        }

        private void DevCheckBox_SelectionChanged(object sender, bool e)
        {           
            TestCheckBox.Selected = false;
            StagCheckBox.Selected = false;
            ProdCheckBox.Selected = false;
            ErrLabel.Hidden = true;
        }

        void TestCheckBox_SelectionChanged(object sender, bool e)
        {
            DevCheckBox.Selected = false;
            StagCheckBox.Selected = false;
            ProdCheckBox.Selected = false;
            ErrLabel.Hidden = true;
        }

        void StagCheckBox_SelectionChanged(object sender, bool e)
        {
            DevCheckBox.Selected = false;
            TestCheckBox.Selected = false;
            ProdCheckBox.Selected = false;
            ErrLabel.Hidden = true;
        }

        void ProdCheckBox_SelectionChanged(object sender, bool e)
        {
            DevCheckBox.Selected = false;
            TestCheckBox.Selected = false;
            StagCheckBox.Selected = false;
            ErrLabel.Hidden = true;
        }

        void PreProdBankidCheckBox_SelectionChanged(object sender, bool e)
        {
            ProdBankidCheckBox.Selected = false;
            ErrLabel.Hidden = true;
        }

        void ProdBankidCheckBox_SelectionChanged(object sender, bool e)
        {
            PreProdBankidCheckBox.Selected = false;
            ErrLabel.Hidden = true;
        }

        partial void ContinueToLogin_Tapped(PrimaryActionButton sender)
        {
            Envicheckboxes = new Dictionary<ConfigTypes, bool>
            {
                {ConfigTypes.Dev, DevCheckBox.Selected },
                {ConfigTypes.Test, TestCheckBox.Selected },
                {ConfigTypes.Staging,StagCheckBox.Selected },
                {ConfigTypes.Prod,ProdCheckBox.Selected }
            };

            BankIdcheckboxes = new Dictionary<BankIdConfigTypes, bool>
            {
                {BankIdConfigTypes.PreProd,  PreProdBankidCheckBox.Selected },
                {BankIdConfigTypes.Prod,  ProdBankidCheckBox.Selected }
            };

            if (Envicheckboxes.Any(x => x.Value) && BankIdcheckboxes.Any(x => x.Value))
            {
                ShowLoader();
                ContinueToLoginBtn.Enabled = false;
                ErrLabel.Hidden = true;
                var configParam = Envicheckboxes.FirstOrDefault(x => x.Value == true).Key;
                var bankIdParam = BankIdcheckboxes.FirstOrDefault(x => x.Value == true).Key;
                Presenter.DevconfigType(configParam, bankIdParam);
            }
            else
            {
                //display error here
                ErrLabel.Hidden = false;
            }
        }

    }
}