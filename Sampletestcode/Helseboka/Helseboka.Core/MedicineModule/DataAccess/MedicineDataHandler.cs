using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Helseboka.Core.Common.CommonImpl;
using Helseboka.Core.Common.Constant;
using Helseboka.Core.Common.DataAccess;
using Helseboka.Core.Common.Interfaces;
using Helseboka.Core.Common.Model;
using Helseboka.Core.MedicineModule.Interface;
using Helseboka.Core.MedicineModule.Model;

namespace Helseboka.Core.MedicineModule.DataAccess
{
    public class MedicineDataHandler : BaseDataHandler, IMedicineAPI
    {
        public async Task<Response<List<MedicineInfo>>> SearchMedicine(string searchText)
        {
            var url = String.Format(APIEndPoints.SearchMedicine, searchText);
            var apiHandler = GetAPIhandlerForGet<List<MedicineInfo>>(url);
            return await apiHandler.Execute();
        }

        public async Task<Response<List<MedicineReminder>>> GetAllMedicineAndReminders()
        {
            var apiHandler = GetAPIhandlerForGet<List<MedicineReminder>>(APIEndPoints.GetAllMedicineAndReminders);
            return await apiHandler.Execute();
        }

        public async Task<Response> AddMedicineOrReminder(AddMedicineRequest request)
        {
            var apiHandler = GetAPIhandlerForPost<AddMedicineRequest, Empty>(APIEndPoints.AddMedicine, request);
            return await apiHandler.Execute();
        }

        public async Task<Response> DeleteMedicine(string medicineId)
        {
            var url = String.Format(APIEndPoints.DeleteMedicine, medicineId);
            var apiHandler = GetAPIhandlerForDelete<Empty>(url);
            return await apiHandler.Execute();
        }

        public async Task<Response> DeleteReminder(string medicineId)
        {
            var url = String.Format(APIEndPoints.DeleteReminder, medicineId);
            var apiHandler = GetAPIhandlerForDelete<Empty>(url);
            return await apiHandler.Execute();
        }

        public async Task<Response<List<GetAlarmResponse>>> GetAlarms(string date)
        {
            var url = String.Format(APIEndPoints.GetAlarms, date);
            var apiHandler = GetAPIhandlerForGet<List<GetAlarmResponse>>(url);
            return await apiHandler.Execute();
        }

        public async Task<Response> MarkAlarmAsComplete(int alarmId)
        {
            var url = String.Format(APIEndPoints.MarkAlarmAsComplete, alarmId);
            var apiHandler = GetAPIhandlerForPost<Empty, Empty>(url, new Empty());
            return await apiHandler.Execute();
        }

        public async Task<Response> RenewPrescription(RenewPrescriptionRequest request)
        {
            var apiHandler = GetAPIhandlerForPost<RenewPrescriptionRequest, Empty>(APIEndPoints.RenewPrescription, request);
            return await apiHandler.Execute();
        }
    }
}
