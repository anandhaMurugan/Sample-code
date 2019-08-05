using System;
using System.Collections.Generic;
using Helseboka.Core.Common.EnumDefinitions;

namespace Helseboka.Core.AppointmentModule.Model
{
    public class AppointmentCollection
    {
        public List<AppointmentDetails> UpcommingAppointments { get; protected set; } = new List<AppointmentDetails>();
        public List<AppointmentDetails> OtherAppointments { get; protected set; } = new List<AppointmentDetails>();

        public int Count
        {
            get
            {
                int count = 0;

                if (UpcommingAppointments != null)
                {
                    count += UpcommingAppointments.Count;
                }

                if (OtherAppointments != null)
                {
                    count += OtherAppointments.Count;
                }

                return count;
            }
        }

        public AppointmentCollection(List<AppointmentDetails> appointments)
        {
            Update(appointments);
        }

        public void Update(List<AppointmentDetails> appointments)
        {
            if (appointments != null)
            {
                foreach (var item in appointments)
                {
                    if (item.Status == AppointmentStatus.Cancelled)
                    {
                        OtherAppointments.Add(item);
                    }
                    else
                    {
                        if (item.Status == AppointmentStatus.Confirmed && item.AppointmentTime.HasValue && item.AppointmentTime.Value < DateTime.Now)
                        {
                            OtherAppointments.Add(item);
                        }
                        else
                        {
                            UpcommingAppointments.Add(item);
                        }
                    }
                }
            }
        }
    }
}
