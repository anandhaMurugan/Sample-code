using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using Helseboka.Core.Common.Extension;
using Helseboka.Core.Common.Interfaces;

namespace Helseboka.Core.MedicineModule.Model
{
    public class ReminderInfo : ICloneable<ReminderInfo>
    {
        public int Id { get; set; }
        //public String AlarmSound { get; set; }
        public int ServingsPerIntakeInterval { get; set; }
        public String Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        //public bool ShowMedicineNameInNotification { get; set; }
        public List<String> FrequencyPerDay { get; set; }
        public List<DayOfWeek> Days { get; set; }

        public DayOfWeek NextReminderDay
        {
            get
            {
                var today = (int)DateTime.Now.DayOfWeek;
                if (Days != null && Days.Count > 0)
                {
                    int result = -1;
                    Days.Sort();

                    // If there is any reminder set today then check if any reminder is there for today
                    if (Days.Contains((DayOfWeek)today))
                    {
                        var nextFreq = GetNextFrequencies();

                        // There is some reminder pending for today
                        if (nextFreq != null && nextFreq.Count > 0)
                        {
                            result = today;
                        }
                        else
                        {
                            // today there is no pending reminder. So if today is last day in list then next reminder has to be  the first day on list
                            // otherwise it's just the next item after today. Note that the days are sorted here.
                            // TODO: Need to handle the case when there is only one day in list.
                            result = today == (int)Days.Last() ? (int)Days.First() : (int)Days[Days.IndexOf((DayOfWeek)today) + 1];
                        }
                    }
                    else
                    {
                        // Find next day after today which is in the list.
                        var target = DateTime.Now;
                        do
                        {
                            target = target.AddDays(1);
                        } while (!Days.Contains(target.DayOfWeek));

                        result = (int)target.DayOfWeek;
                    }

                    return (DayOfWeek)result;
                }
                else
                {
                    return (DayOfWeek)today;
                }
            }
        }

        public ReminderInfo Clone()
        {
            var result = new ReminderInfo();

            result.Id = this.Id;
            result.ServingsPerIntakeInterval = this.ServingsPerIntakeInterval;
            result.Status = this.Status;
            result.StartDate = this.StartDate;
            result.EndDate = this.EndDate;
            if(this.FrequencyPerDay != null && this.FrequencyPerDay.Count > 0)
            {
                result.FrequencyPerDay = this.FrequencyPerDay.Select(x => x).ToList();
            }
            if(this.Days != null && this.Days.Count > 0)
            {
                result.Days = this.Days.Select(x => x).ToList();
            }

            return result;
        }

        /// <summary>
        /// Gets the next frequencies after current time. This method doesn't check the day.
        /// </summary>
        /// <returns>The next frequencies.</returns>
        public List<String> GetNextFrequencies()
        {
            var result = new List<DateTime>();

            foreach (var time in FrequencyPerDay)
            {
                var frequencyTime = time.ConvertTimeToDateTime();
                if(frequencyTime.HasValue)
                {
                    if (frequencyTime.Value > DateTime.Now)
                    {
                        result.Add(frequencyTime.Value);
                    }
                }
            }

            result.Sort();

            return result.Select(x=>x.GetTimeString()).ToList();
        }
    }
}
