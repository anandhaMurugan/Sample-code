using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Helseboka.Core.Common.Interfaces;
using Helseboka.Core.Common.Model;
using Helseboka.Core.MedicineModule.Model;

namespace Helseboka.Core.MedicineModule.Interface
{
    public interface IMedicineHomePresenter : IBasePresenter
    {
        Task<Response<List<MedicineInfo>>> SearchMedicine(String searchText);

        Task<Response<List<MedicineReminder>>> GetAllMedicineAndReminders();

        bool IsMedicineExistInProfile(MedicineInfo medicine);

        void NavigateToMedicineAlarmView(MedicineReminder medicine);

        void NavigateToMedicineOverview(MedicineReminder medicine);

        void SelectMedicineFromSearchResult(MedicineInfo medicine);

        Task DeleteMedicine(MedicineReminder medicine);

        Task<Response> RenewPrescription(List<MedicineInfo> medicines);

        void GoBack();

        Task AddMedicineToProfile(MedicineReminder medicineDetails);
    }
}
