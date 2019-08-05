using Helseboka.Core.Common.EnumDefinitions;
using Helseboka.Core.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helseboka.Core.Startup.Interface
{
    public interface IUrlPresenter : IBasePresenter
    {
        void DevconfigType(ConfigTypes configs, BankIdConfigTypes bankIdConfigs);
    }
}
