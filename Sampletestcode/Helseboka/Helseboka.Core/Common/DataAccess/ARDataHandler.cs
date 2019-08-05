using System;
using System.Threading.Tasks;
using Helseboka.Core.Common.Constant;
using Helseboka.Core.Common.DataAccess;
using Helseboka.Core.Common.Interface;
using Helseboka.Core.Common.Model;
namespace Helseboka.Core.Common.DataAccess
{
    public class ARDataHandler : BaseDataHandler, IARService
    {
        public async Task<Response<PaginationResponse<MedicalCenter>>> SearchDoctorOrOffice(string searchText, int pageNum, int pageSize)
        {
            var url = String.Format(APIEndPoints.SearchDoctorAndOffice, searchText, pageNum, pageSize);
            var apiHandler = GetAPIhandlerForGet<PaginationResponse<MedicalCenter>>(url);
            return await apiHandler.Execute();
        }

        public async Task<Response<Doctor>> GetDoctorDetails(String doctorId)
        {
            var url = String.Format(APIEndPoints.GetDoctorDetails, doctorId);
            var apiHandler = GetAPIhandlerForGet<Doctor>(url);
            return await apiHandler.Execute();
        }
    }
}
