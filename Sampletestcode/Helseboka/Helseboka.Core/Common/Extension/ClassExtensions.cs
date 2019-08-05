using System;
using Helseboka.Core.Common.EnumDefinitions;
using Helseboka.Core.Resources.StringResources;

namespace Helseboka.Core.Common.Extension
{
    public static class ClassExtensions
    {
        public static Day GetDay(this DayOfWeek dayOfWeek)
        {
            if (dayOfWeek == DateTime.Now.DayOfWeek)
            {
                return Day.Today;
            }
            else if (dayOfWeek == DateTime.Now.AddDays(1).DayOfWeek)
            {
                return Day.Tomorrow;
            }
            else if (dayOfWeek == DateTime.Now.AddDays(-1).DayOfWeek)
            {
                return Day.Yesterday;
            }
            else
            {
                return Day.Others;
            }
        }

        public static String GetChatStatusText(this ChatStatus status)
        {
            switch(status)
            {
                case ChatStatus.Sent : return AppResources.ChatStatusSent;
                case ChatStatus.Delivered: return AppResources.ChatStatusDelivered;
                case ChatStatus.Received: return AppResources.ChatStatusReceived;
                case ChatStatus.Error:
                default: return AppResources.ChatStatusError;
            }
        }
    }
}
