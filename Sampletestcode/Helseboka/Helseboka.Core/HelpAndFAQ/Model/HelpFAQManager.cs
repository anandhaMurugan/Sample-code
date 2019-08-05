using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Helseboka.Core.Common.CommonImpl;
using Helseboka.Core.Common.EnumDefinitions;
using Helseboka.Core.Common.Model;
using Helseboka.Core.Common.Interface;
using Helseboka.Core.HelpAndFAQ.Interface;

namespace Helseboka.Core.HelpAndFAQ.Model
{
    public class HelpFAQManager
    {
        public async Task<List<HelpFAQDataModel>> GetHelpFAQList(HelpFAQType helpType)
        {
            var dataHandler = ApplicationCore.Container.Resolve<IHelpFAQDataAccess>();
            return await dataHandler.GetHelpFAQList(helpType);
        }
    }
}
