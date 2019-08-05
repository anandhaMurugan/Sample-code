using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Helseboka.Core.Common.Model;
using Helseboka.Core.Common.Extension;
using Helseboka.Core.Common.Presenter;
using Helseboka.Core.MedicineModule.Interface;
using Helseboka.Core.MedicineModule.Model;
using System.Linq;

namespace Helseboka.Core.MedicineModule.Presenter
{
    public class MedicineHomePresenter : BasePresenter, IMedicineHomePresenter, IMedicineAlarmPresenter
    {
        private MedicineManager manager = new MedicineManager();
        private IMedicineRouter Router
        {
            get => router as IMedicineRouter;
        }

        public MedicineHomePresenter(IMedicineRouter medicineRouter)
        {
            router = medicineRouter;
        }

        public async Task<Response<List<MedicineInfo>>> SearchMedicine(string searchText)
        {
            return await manager.SearchMedicine(searchText);
        }

        public void NavigateToMedicineAlarmView(MedicineReminder medicine)
        {
            Router.NavigateToSetMedicineAlarm(medicine);
        }

        public void NavigateToMedicineOverview(MedicineReminder medicine)
        {
            Router.NavigateToMedicineOverview(medicine, false);
        }

        void IMedicineAlarmPresenter.GoBack()
        {
            Router.NavigateBackToOverviewFromAlarm();
        }

        void IMedicineHomePresenter.GoBack()
        {
            Router.GoBackToHome();
        }

        public void GoBackToHome()
        {
            Router.GoBackToHome();
        }

        public bool IsMedicineExistInProfile(MedicineInfo medicine)
        {
            if (ApplicationCore.Instance.CurrentUser.MedicineReminders != null)
            {
                return ApplicationCore.Instance.CurrentUser.MedicineReminders.Exists((obj) => obj.Medicine.Id == medicine.Id);
            }
            else
            {
                return false;
            }
        }

        public void SelectMedicineFromSearchResult(MedicineInfo medicine)
        {
            //Loading();
            //var response = await medicine.AddToProfile();
            //HideLoading();
            //if(response.IsSuccess)
            //{
            //    var medicineReminder = new MedicineReminder();
            //    medicineReminder.Medicine = medicine;
            //    NavigateToMedicineOverview(medicineReminder);
            //}
            //else
            //{
            //    RaiseError(response.ResponseInfo);
            //}

            var medicineReminder = new MedicineReminder();
            medicineReminder.Medicine = medicine;
            Router.NavigateToMedicineOverview(medicineReminder, true);
        }

        public async Task AddMedicineToProfile(MedicineReminder medicineDetails)
        {
            Loading();
            var response = await medicineDetails.Medicine.AddToProfile();
            HideLoading();
            if(response.IsSuccess)
            {
                Router.GoBackToHome();
            }
            else
            {
                RaiseError(response.ResponseInfo);
            }
        }

        public async Task<Response<List<MedicineReminder>>> GetAllMedicineAndReminders()
        {
            return await ApplicationCore.Instance.CurrentUser.GetAllMedicineAndReminders();
        }

        public async Task<Response> AddReminder(MedicineReminder medicine, List<DayOfWeek> days, List<DateTime> frequencies, DateTime startDate, DateTime? endDate = null, List<MedicineInfo> medicineList = null)
        {
            frequencies.Sort();
            var stringFrequencies = frequencies.Select(x => x.GetTimeString()).ToList();
            return await medicine.Medicine.AddReminder(days, stringFrequencies, startDate, endDate, medicineList);
        }

        public List<MedicineInfo> GetNewMedicines(List<MedicineInfo> existingMedicineList)
        {
            return ApplicationCore.Instance.CurrentUser.MedicineReminders.Select(x => x.Medicine).ToList().Except(existingMedicineList).ToList();
        }

        public async Task<Response> RemoveReminder(MedicineReminder medicine)
        {
            return await medicine.DeleteReminder();
        }

        public async Task DeleteMedicine(MedicineReminder medicine)
        {
            Loading();
            var response = await medicine.DeleteFromProfile();
            HideLoading();
            if (response.IsSuccess)
            {
                Router.GoBackToHome();
            }
            else
            {
                RaiseError(response.ResponseInfo);
            }
        }

        public async Task<Response> RenewPrescription(List<MedicineInfo> medicines)
        {
            if (medicines != null && medicines.Count > 0)
            {
                Loading();
                var response = await manager.RenewPrescription(medicines);
                HideLoading();
                if (!response.IsSuccess)
                {
                    RaiseError(response.ResponseInfo);
                }
                return response;
            }
            else
            {
                return Response.GetSuccessResponse();
            }
        }

        public List<MedicineReminder> GetConflictingReminders(List<MedicineInfo> selectedMedicine)
        {
            if (selectedMedicine != null && selectedMedicine.Count > 0 && ApplicationCore.Instance.CurrentUser.MedicineReminders != null)
            {
                return ApplicationCore.Instance.CurrentUser.MedicineReminders.Where(x => x.HasReminder && selectedMedicine.Contains(x.Medicine)).ToList();
            }
            else
            {
                return null;
            }
        }
    }
}
