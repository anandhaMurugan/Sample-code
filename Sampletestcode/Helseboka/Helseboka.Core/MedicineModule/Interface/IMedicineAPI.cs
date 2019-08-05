using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Helseboka.Core.Common.Model;
using Helseboka.Core.MedicineModule.Model;

namespace Helseboka.Core.MedicineModule.Interface
{
    public interface IMedicineAPI
    {
        Task<Response<List<MedicineInfo>>> SearchMedicine(String searchText);

        Task<Response> AddMedicineOrReminder(AddMedicineRequest request);

        Task<Response<List<MedicineReminder>>> GetAllMedicineAndReminders();

        Task<Response> DeleteMedicine(string medicineId);

        Task<Response> DeleteReminder(string medicineId);

        Task<Response<List<GetAlarmResponse>>> GetAlarms(string date);

        Task<Response> MarkAlarmAsComplete(int alarmId);

        Task<Response> RenewPrescription(RenewPrescriptionRequest request);
    }
}
