using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Helseboka.Core.Common.Interfaces;
using Helseboka.Core.Common.Model;
using Helseboka.Core.MedicineModule.Model;

namespace Helseboka.Core.MedicineModule.Interface
{
    public interface IMedicineAlarmPresenter : IBasePresenter
    {
        void GoBack();

        void GoBackToHome();

        List<MedicineInfo> GetNewMedicines(List<MedicineInfo> existingMedicineList);

        Task<Response> AddReminder(MedicineReminder medicine, List<DayOfWeek> days, List<DateTime> frequencies, DateTime startDate, DateTime? endDate = null, List<MedicineInfo> medicineList = null);

        Task<Response> RemoveReminder(MedicineReminder medicine);

        List<MedicineReminder> GetConflictingReminders(List<MedicineInfo> selectedMedicine);
    }
}
