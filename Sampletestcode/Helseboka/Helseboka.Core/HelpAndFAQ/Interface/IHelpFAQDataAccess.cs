using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Helseboka.Core.Common.EnumDefinitions;
using Helseboka.Core.Common.Model;

namespace Helseboka.Core.HelpAndFAQ.Interface
{
    public interface IHelpFAQDataAccess
    {
        Task<List<HelpFAQDataModel>> GetHelpFAQList(HelpFAQType helpType);
    }
}
