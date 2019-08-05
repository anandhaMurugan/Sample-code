using System;
using System.Linq;
using Helseboka.Core.Common.CommonImpl;
using Helseboka.Core.MedicineModule.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;
using Helseboka.Core.Common.Model;

namespace Helseboka.Core.MedicineModule.Model
{
    public class MedicineManager
    {
        private IMedicineAPI DataHandler
        {
            get => ApplicationCore.Container.Resolve<IMedicineAPI>();
        }

        public async Task<Response<List<MedicineInfo>>> SearchMedicine(String searchText)
        {
            return await DataHandler.SearchMedicine(searchText);
        }

        public async Task<Response> RenewPrescription(List<MedicineInfo> medicines)
        {
            var request = new RenewPrescriptionRequest();
            request.MedicineIds = medicines.Select(x => x.Id).ToList();
            request.DoctorId = Convert.ToInt32(ApplicationCore.Instance.CurrentUser.DoctorId);
            return await DataHandler.RenewPrescription(request);
        }




    }
}
