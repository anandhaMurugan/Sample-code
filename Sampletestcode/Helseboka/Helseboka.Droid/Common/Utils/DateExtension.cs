﻿using System;
using Android.Content;
using Helseboka.Core.Common.EnumDefinitions;
using Helseboka.Core.Common.Extension;
using Java.Util;

namespace Helseboka.Droid.Common.Utils
{
    public static class DateExtension
    {
        public static String GetDayString(this DateTime date, Context context)
        {
            var day = date.GetDay();
            switch (day)
            {
                case Day.Today:
                case Day.Yesterday:
                case Day.Tomorrow: return day.ToString().ToLower().Translate(context);
                default: return date.ToString("dd.MM.yyyy");
            }
        }
    }
}
