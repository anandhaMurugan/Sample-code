using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Helseboka.Core.Common.Interfaces;
using Helseboka.Core.Common.Model;
using Helseboka.Core.MedicineModule.Model;

namespace Helseboka.Core.MedicineModule.Interface
{
    public interface IMedicineRouter : IBaseRouter
    {
        void NavigateToSetMedicineAlarm(MedicineReminder medicine);

        void NavigateToMedicineOverview(MedicineReminder medicine, Boolean isFromSearch);

        void GoBackToHome();

        void NavigateBackToOverviewFromAlarm();
    }
}
