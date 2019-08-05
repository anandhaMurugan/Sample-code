using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Helseboka.Core.Common.DataAccess;
using Helseboka.Core.Common.Model;
using Helseboka.Core.Startup.UpdateVersion.DataAcces;

namespace Helseboka.Core.Startup.UpdateVersion.Models
{
    public class UpdateAppModel
    {

        private UpdateVersionDataHandlers DataHandler = new UpdateVersionDataHandlers();

        public string Platform { get; set; }
        public string RequiredVersion { get; set; }
        public string NewestVersion { get; set; }
        public string Text { get; set; }

        public async Task<Response<List<UpdateAppModel>>> GetVersionUpdate()
        {
            return await DataHandler.GetVersionUpdate();
        }
    }
}