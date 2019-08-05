using System;

namespace Helseboka.Droid.Common.Constants
{
    public enum SharedPreferenceKey
    {
        Reminders,
        MedicineReminderList,
        PushDeviceToken
    }

    public static class AndroidConstants
    {
        public const String NotificationChannelId = "no.helseboka";
        public const String NotificationMessage = "message";
        public const String NotificationTitle = "title";
        public const String NotificationId = "notificationID";
        public const int ForegroundServiceNotificationId = 99;
        public const String RegisterMedicineReminderAlarm = "NotificationService.RegisterReminder";
    }
}
