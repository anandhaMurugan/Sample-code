using Helseboka.Core.Common.CommonImpl;
using Helseboka.Core.Common.EnumDefinitions;
using Helseboka.Core.Common.Interfaces;
using Helseboka.Core.Common.Model;
using Helseboka.Core.Common.Presenter;
using Helseboka.Core.Startup.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helseboka.Core.Startup.Presenter
{
    public class UrlPresenter : BasePresenter,IUrlPresenter
    {

        private IStartupRouter Router { get => router as IStartupRouter; }

        public UrlPresenter(IStartupRouter startupRouter)
        {
            router = startupRouter;
        }

        public  void DevconfigType(ConfigTypes configs, BankIdConfigTypes bankIdConfigs)
        {

            switch (configs)
            {
                case ConfigTypes.Dev:
                ApplicationCore.Container.RegisterType<IConfig, DevDevConfig>();
                break;

                case ConfigTypes.Test:
                ApplicationCore.Container.RegisterType<IConfig, TestDevConfig>();
                break;

                case ConfigTypes.Staging:
                ApplicationCore.Container.RegisterType<IConfig, StagDevConfig>();
                break;

                case ConfigTypes.Prod:
                ApplicationCore.Container.RegisterType<IConfig, DevConfig>();
                break;

                default:
                break;

            }

            switch(bankIdConfigs)
            {
                case BankIdConfigTypes.PreProd:
                ApplicationCore.Container.RegisterType<IBankIdConfig, PreProdBankIdConfig>();
                break;

                case BankIdConfigTypes.Prod:
                ApplicationCore.Container.RegisterType<IBankIdConfig, ProdBankIdConfig>();
                break;

                default:
                break;
            }

            Router.StartAfterDevSettings();
        }
    }
}
