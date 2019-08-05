using System;
using Helseboka.Core.Common.EnumDefinitions;

namespace Helseboka.Core.Common.Extension
{
    public static class DateExtension
    {
        public static Day GetDay(this DateTime date)
        {
            if (date.Day == DateTime.Now.Day)
            {
                return Day.Today;
            }
            else if (date.Day == DateTime.Now.Day - 1)
            {
                return Day.Yesterday;
            }
            else if (date.Day == DateTime.Now.Day + 1)
            {
                return Day.Tomorrow;
            }
            else
            {
                return Day.Others;
            }
        } 

        public static String GetTimeString(this DateTime date)
        {
            return date.ToString(@"HH\:mm");
        }
    }
}
