using System;
using System.Threading.Tasks;
using Helseboka.Core.Common.Model;

namespace Helseboka.Core.Common.Interface
{
    public interface IARService
    {
        Task<Response<PaginationResponse<MedicalCenter>>> SearchDoctorOrOffice(String searchText, int pageNum, int pageSize);

        Task<Response<Doctor>> GetDoctorDetails(String doctorId);
    }
}
