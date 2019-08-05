using Helseboka.Core.Common.Constant;
using Helseboka.Core.Common.DataAccess;
using Helseboka.Core.Common.Model;
using Helseboka.Core.Legedialog.Interface;
using Helseboka.Core.Terms.Interface;
using Helseboka.Core.Terms.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helseboka.Core.Terms.DataAccess
{
    public class TermsDataHandler : BaseDataHandler, ITermsAPI
    {
        public async Task<Response<TermsListModel>> GetAllTerms()
        {
            var apiHandler = GetAPIhandlerForGet<TermsListModel>(APIEndPoints.GetUserTerms);        
            return await apiHandler.Execute();
        }

        public async Task<Response> UpdateAllTerms(List<int> acceptedIds)
        {
            var apiHandler = GetAPIhandlerForPut<List<int>, Empty>(APIEndPoints.GetUserTerms, acceptedIds);
            return await apiHandler.Execute();
        }
    }
}
