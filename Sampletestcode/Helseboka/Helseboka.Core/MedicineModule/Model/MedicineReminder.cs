using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Helseboka.Core.Common.Extension;
using Helseboka.Core.Common.CommonImpl;
using Helseboka.Core.Common.Model;
using Helseboka.Core.MedicineModule.Interface;
using Helseboka.Core.Common.Interfaces;

namespace Helseboka.Core.MedicineModule.Model
{
    public class MedicineReminder : ICloneable<MedicineReminder>
    {
        public MedicineInfo Medicine { get; set; }
        public ReminderInfo Reminder { get; set; }

        public bool HasReminder
        {
            get => Reminder != null;
        }

        private IMedicineAPI DataHandler
        {
            get => ApplicationCore.Container.Resolve<IMedicineAPI>();
        }

        public MedicineReminder Clone()
        {
            var result = new MedicineReminder();

            if(this.Medicine != null)
            {
                result.Medicine = this.Medicine.Clone();
            }

            if(this.Reminder != null)
            {
                result.Reminder = this.Reminder.Clone();
            }

            return result;
        }

        public async Task<Response> DeleteFromProfile()
        {
            if (Medicine != null)
            {
                var response = await DataHandler.DeleteMedicine(Medicine.Id.ToString());

                if (response.IsSuccess && HasReminder)
                {
                    ApplicationCore.Container.Resolve<INotificationService>().DeleteScheduledNotification(Medicine).Forget();
                }

                return response;
            }
            else
            {
                return Response.GetSuccessResponse();
            }

        }

        public async Task<Response> DeleteReminder()
        {
            if (Medicine != null && HasReminder)
            {
                var response = await DataHandler.DeleteReminder(Medicine.Id.ToString());

                if (response.IsSuccess)
                {
                    Reminder = null;
                    ApplicationCore.Container.Resolve<INotificationService>().DeleteScheduledNotification(Medicine).Forget();
                }

                return response;
            }
            else
            {
                return Response.GetSuccessResponse();
            }
        }
    }
}
