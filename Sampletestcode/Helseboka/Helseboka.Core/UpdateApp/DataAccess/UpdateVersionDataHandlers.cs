using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helseboka.Core.Common.Constant;
using Helseboka.Core.Common.DataAccess;
using Helseboka.Core.Common.Model;
using Helseboka.Core.Models;
using Helseboka.Core.Startup.UpdateVersion.Models;

namespace Helseboka.Core.Startup.UpdateVersion.DataAcces
{
    public class UpdateVersionDataHandlers : BaseDataHandler
    {
        public async Task<Response<List<UpdateAppModel>>> GetVersionUpdate()
        {
            var url = String.Format(APIEndPoints.GetUpdateInfo);
            var apiHandler = GetAPIhandlerForGet<List<UpdateAppModel>>(url);
            return await apiHandler.Execute();
        }
    }
}