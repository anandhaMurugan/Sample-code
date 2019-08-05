using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helseboka.Core.Common.Presenter;
using Helseboka.Core.Models;
using Helseboka.Core.Startup.Interface;
using Helseboka.Core.Startup.UpdateVersion.Interface;
using Helseboka.Core.Startup.UpdateVersion.Models;
using Helseboka.Core.Common.Extension;
using Helseboka.Core.Common.Model;
using Xamarin.Forms;
using Helseboka.Core.Common.CommonImpl;
using Helseboka.Core.Common.EnumDefinitions;

namespace Helseboka.Core.Startup.UpdateVersion.Presenter
{
    public class UpdatePresenter : BasePresenter, IUpdatePresenter
    {
        List<UpdateAppModel> updateModelList;
        int indexOfAndroid;
        int indexOfIos;
        PlatformType currentDevicePlatform;

        private UpdateAppModel updateModel = new UpdateAppModel();
        private IStartupRouter Router { get => router as IStartupRouter; }

        public UpdatePresenter(IStartupRouter startupRouter)
        {
            router = startupRouter;
        }

        public async Task UpdateVersionCheckDetails()
        {
            var response = await updateModel.GetVersionUpdate();
            if (!response.IsSuccess)
            {
                RaiseError(response.ResponseInfo);
                return;
            }
            updateModelList = response.Result;

            for (int i = 0; i < updateModelList.Count; i++)
            {
                if (Enum.TryParse(updateModelList[i].Platform, out PlatformType result))
                {
                    if (result == PlatformType.ANDROID)
                    {
                        indexOfAndroid = i;
                    }
                    else if (result == PlatformType.IOS)
                    {
                        indexOfIos = i;
                    }
                }
            }

            if (indexOfAndroid != indexOfIos)
            {
                DecideUpdateFlow();
            } 
        }

        private void DecideUpdateFlow()
        {
            currentDevicePlatform = DeviceDetails.Instance.GetDevicePlatform();
           
            if (currentDevicePlatform == PlatformType.ANDROID)
            {
                RelateVersionForUpdate(indexOfAndroid);
            }
            else if(currentDevicePlatform == PlatformType.IOS)
            {
                RelateVersionForUpdate(indexOfIos);
            }
        }

        private void RelateVersionForUpdate(int index)
        {
            string appVersion = Router.GetAppVersion();
       
            var currentAppVersion = new Version(appVersion);
            var requiredVersion = new Version(updateModelList[index].RequiredVersion);
            var newestVersion = new Version(updateModelList[index].NewestVersion);
            string descriptionText = updateModelList[index].Text;
           
            if (currentAppVersion >= requiredVersion)
            {
                if(currentAppVersion < newestVersion)
                {
                    Router.StartAfterUpdateSettings(false, true, descriptionText);
                    return;
                }
                Router.StartAfterUpdateSettings(false, false);
            }
            else
            {
                Router.StartAfterUpdateSettings(true, true, descriptionText);
            }
        }

        public void ProceedCloseOrNotNowClicked()
        {
            Router.StartAfterUpdateSettings(false, false);
        }
    }
}