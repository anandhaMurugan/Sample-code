using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Helseboka.Core.Common.CommonImpl;
using Helseboka.Core.Common.DataAccess;
using Helseboka.Core.Common.EnumDefinitions;
using Helseboka.Core.Common.Interfaces;
using Helseboka.Core.Common.Model;
using Helseboka.Core.HelpAndFAQ.Interface;

namespace Helseboka.Core.HelpAndFAQ.DataAccess
{
    public class HelpDataHandler : BaseDataHandler, IHelpFAQDataAccess
    {
        private IDeviceHandler DeviceHandler
        {
            get => ApplicationCore.Container.Resolve<IDeviceHandler>();
        }

        public async Task<List<HelpFAQDataModel>> GetHelpFAQList(HelpFAQType helpType)
        {
            String fileName = helpType.ToString() + ".json";
            return await Task.FromResult<List<HelpFAQDataModel>>(GetFAQDataList(fileName));
        }

        private List<HelpFAQDataModel> GetFAQDataList(String fileName)
        {
            var lang = DeviceHandler.GetLanguageCode();
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(HelpDataHandler)).Assembly;
            Stream stream = assembly.GetManifestResourceStream($"Helseboka.Core.HelpAndFAQ.Resource.{lang}.{fileName}");

            using (var reader = new System.IO.StreamReader(stream))
            {
                var jsonString = reader.ReadToEnd();
                var response = Serializer.Deserialize<List<HelpFAQDataModel>>(jsonString);
                return response.result;
            }
        }
    }
}
