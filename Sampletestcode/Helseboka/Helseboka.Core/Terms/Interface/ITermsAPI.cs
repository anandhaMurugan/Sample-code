using Helseboka.Core.Common.Model;
using Helseboka.Core.Terms.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helseboka.Core.Terms.Interface
{
    public interface ITermsAPI
    {
        Task<Response<TermsListModel>> GetAllTerms();

        Task<Response> UpdateAllTerms(List<int> acceptedIds);
    }
}
