using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Helseboka.Core.Common.Interfaces;
using Helseboka.Core.Common.Model;
using Helseboka.Core.Common.Presenter;
using Helseboka.Core.Profile.Model;
using Helseboka.Core.Startup.Interface;
using System.Linq;

namespace Helseboka.Core.Startup.Presenter
{
    public class DoctorSelectionPresenter : BasePresenter, IDoctorSelectionPresenter
    {
        private const int pageSize = 10;
        private int currentPage = 0;
        private String currentSearch;

        public bool HasMoreData { get; private set; }

        private IStartupRouter Router { get => router as IStartupRouter; }

        public DoctorSelectionPresenter(IStartupRouter baseRouter)
        {
            router = baseRouter;
        }

        public async Task<List<Doctor>> SearchDoctor(string searchText)
        {
            currentSearch = searchText;
            currentPage = 0;
            var response = await ApplicationCore.Instance.SearchDoctorOrOffice(searchText, currentPage, pageSize);
            if (response.IsSuccess && currentSearch == searchText)
            {
                HasMoreData = currentPage < response.Result.TotalPages - 1;
                var doctorList = response.Result.Content == null ? new List<Doctor>() : response.Result.Content.SelectMany(data => data.Doctors).ToList();
                return doctorList;
            }
            else
            {
                RaiseError(response.ResponseInfo);
                return new List<Doctor>();
            }
        }

        public async Task<List<Doctor>> LoadMore()
        {
            if (HasMoreData)
            {
                var response = await ApplicationCore.Instance.SearchDoctorOrOffice(currentSearch, ++currentPage, pageSize);
                if (response.IsSuccess)
                {
                    HasMoreData = currentPage < response.Result.TotalPages - 1;
                    var doctorList = response.Result.Content == null ? new List<Doctor>() : response.Result.Content.SelectMany(data => data.Doctors).ToList();
                    return doctorList;
                }
                else
                {
                    RaiseError(response.ResponseInfo);
                    return new List<Doctor>();
                }
            }
            else
            {
                return new List<Doctor>();
            }
        }

        public async Task SelectDoctor(Doctor doctor)
        {
            Loading();
            var response = await ApplicationCore.Instance.CurrentUser.UpdateDoctor(doctor);
            HideLoading();
            if (response.IsSuccess)
            {
                Router.DoctorSelectionCompleted();
            }
            else
            {
                RaiseError(response.ResponseInfo);
            }
        }

        public async Task<Response<User>> GetCurrentUser()
        {
            if (ApplicationCore.Instance.CurrentUser != null)
            {
                return Response<User>.GetSuccessResponse(ApplicationCore.Instance.CurrentUser);
            }
            else
            {
                return await ApplicationCore.Instance.GetUserDetails();
            }
        }

        public void Cancel()
        {
            Router.DoctorSelectionCompleted();
        }
    }
}
