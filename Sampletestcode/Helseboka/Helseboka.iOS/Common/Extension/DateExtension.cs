using System;
using Helseboka.Core.Common.EnumDefinitions;
using Helseboka.Core.Common.Extension;

namespace Helseboka.iOS.Common.Extension
{
    public static class DateExtension
    {
        public static String GetDayString(this DateTime date)
        {
            var day = date.GetDay();
            switch (day)
            {
                case Day.Today:
                case Day.Yesterday:
                case Day.Tomorrow: return day.ToString().Translate();
                default: return date.ToString("dd.MM.yyyy");
            }
        }
    }
}
