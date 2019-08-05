using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Helseboka.Core.Common.Interfaces;
using Helseboka.Core.MedicineModule.Model;
using Java.Util;
using Android.OS;
using Android.Support.V4.App;
using Android.Media;
using Helseboka.Core.Common.Extension;
using Android.Preferences;
using Helseboka.Droid.Common.Receivers;
using Helseboka.Core.Common.CommonImpl;
using Android.Util;
using Helseboka.Droid.Common.Constants;
using Helseboka.Droid.Common.Services;
using Helseboka.Core.Common.Model;
using Helseboka.Core.Auth.Model;

namespace Helseboka.Droid.Common.CommonImpl
{
    public class NotificationHandler : INotificationService
    {
        #region Singleton Implementation
        //Lazy singleton initialization
        // http://csharpindepth.com/Articles/General/Singleton.aspx
        // Double-check-Locking will not be required - https://msdn.microsoft.com/en-us/library/ff650316.aspx
        private NotificationHandler()
        {
        }

        public static NotificationHandler Instance { get { return Nested.instance; } }

        private class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }

            internal static readonly NotificationHandler instance = new NotificationHandler();
        }
        #endregion

        private Context context;
        private String notificationToken;

        public void SetContext(Context context)
        {
            this.context = context;
        }

        void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification
                // channel on older versions of Android.
                return;
            }

            var channelName = context.Resources.GetString(Resource.String.channel_name);
            var channelDescription = context.GetString(Resource.String.channel_description);
            var channel = new NotificationChannel(AndroidConstants.NotificationChannelId, channelName, NotificationImportance.High)
            {
                Description = channelDescription
            };

            var notificationManager = (NotificationManager)context.GetSystemService(Context.NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }

        public void Register()
        {
            CreateNotificationChannel();
        }

        public async Task UpdateNotificationToken(String pushToken)
        {
            var prefs = PreferenceManager.GetDefaultSharedPreferences(context);
            var oldDeviceToken = prefs.GetString(SharedPreferenceKey.PushDeviceToken.ToString(), "");

            if (string.IsNullOrEmpty(oldDeviceToken) || !oldDeviceToken.Equals(pushToken))
            {
                if (!String.IsNullOrEmpty(AuthService.Instance.APIKey) && ApplicationCore.Instance.CurrentUser != null)
                {
                    await UpdateNotificationTokenToServer(pushToken);
                }
                else
                {
                    this.notificationToken = pushToken;
                }
            }
        }

        public async Task CompletePendingTask()
        {
            if (!String.IsNullOrEmpty(notificationToken))
            {
                await UpdateNotificationTokenToServer(notificationToken);
                notificationToken = null;
            }
        }

        private async Task UpdateNotificationTokenToServer(string pushToken)
        {
            var response = await ApplicationCore.Instance.CurrentUser.UpdateNotificationToken("ANDROID", pushToken);
            if (response.IsSuccess)
            {
                var prefs = PreferenceManager.GetDefaultSharedPreferences(context);
                var editor = prefs.Edit();
                editor.PutString(SharedPreferenceKey.PushDeviceToken.ToString(), pushToken);
                editor.Apply();
            }
        }

        private void DeleteReminder(List<int> tags)
        {
            var alarmManager = (AlarmManager)context.GetSystemService(Context.AlarmService);

            foreach (var tag in tags)
            {
                var intent = new Intent(context, typeof(AlarmReceiver));
                var pendingIntent = PendingIntent.GetBroadcast(context, tag, intent, PendingIntentFlags.CancelCurrent);

                alarmManager.Cancel(pendingIntent);

                Log.Debug("Helseboka", $"Deleted alarm for {tag}");
            }
        }

        public async Task DeleteScheduledNotification(MedicineInfo medicine)
        {
            var task = Task.Run(() =>
            {
                var prefs = PreferenceManager.GetDefaultSharedPreferences(context);
                var existingReminderRecord = prefs.GetStringSet(medicine.Id.ToString(), new List<String>());
                var existingMedicalRecord = prefs.GetStringSet(SharedPreferenceKey.Reminders.ToString(), new List<String>());
                var editor = prefs.Edit();

                var reminderTags = existingReminderRecord.Select(x => Convert.ToInt32(x)).ToList();
                DeleteReminder(reminderTags);

                existingReminderRecord.Clear();
                existingMedicalRecord.Remove(medicine.Id.ToString());
                editor.PutStringSet(medicine.Id.ToString(), null);
                editor.PutStringSet(SharedPreferenceKey.Reminders.ToString(), existingMedicalRecord);
                editor.Apply();
            });

            await task;
        }

        private void DeleteAllPendingAlarms()
        {
            Log.Debug("Helseboka", $"Deleting all notification");

            var tagsToBeDeleted = new List<int>();

            var prefs = PreferenceManager.GetDefaultSharedPreferences(context);
            var editor = prefs.Edit();
            var existingMedicalRecord = prefs.GetStringSet(SharedPreferenceKey.Reminders.ToString(), new List<String>());

            foreach (var medicine in existingMedicalRecord)
            {
                var existingReminderRecord = prefs.GetStringSet(medicine, new List<String>());
                tagsToBeDeleted.AddRange(existingReminderRecord.Select(x => Convert.ToInt32(x)).ToList());

                editor.PutStringSet(medicine, null);
            }

            DeleteReminder(tagsToBeDeleted);

            editor.PutStringSet(SharedPreferenceKey.Reminders.ToString(), null);
            editor.Apply();
        }

        public async Task ScheduleLocalNotification(DayOfWeek day, string time, MedicineInfo medicine, DateTime startDate, DateTime? endDate = null)
        {
            var task = Task.Run(() =>
            {
                var result = ScheduleAlarmForReminder(context, day, time, medicine, startDate, endDate);
                if(result.HasValue)
                {
                    StartForegroundService();

                    var prefs = PreferenceManager.GetDefaultSharedPreferences(context);
                    var existingReminderRecord = prefs.GetStringSet(medicine.Id.ToString(), new List<String>());
                    var existingMedicalRecord = prefs.GetStringSet(SharedPreferenceKey.Reminders.ToString(), new List<String>());
                    var editor = prefs.Edit();
                    existingReminderRecord.Add(result.Value.ToString());
                    existingMedicalRecord.Add(medicine.Id.ToString());
                    editor.PutStringSet(medicine.Id.ToString(), existingReminderRecord);
                    editor.PutStringSet(SharedPreferenceKey.Reminders.ToString(), existingMedicalRecord);
                    editor.Apply();
                }
            });

            await task;
        }

        public async Task ScheduleNotification(List<MedicineReminder> reminderList)
        {
            await Task.Run(() =>
            {
                try
                {
                    DeleteAllPendingAlarms();
                    if (reminderList!= null && reminderList.Count > 0)
                    {
                        Log.Debug("Helseboka", $"Scheduling all notification");
                        ScheduleNotificationForReminders(context, reminderList);
                        StartForegroundService();
                        SaveToPermanentStorage(reminderList);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("Helseboka", ex.ToString());
                }

            });
        }

        public static int? ScheduleAlarmForReminder(Context context, DayOfWeek day, string time, MedicineInfo medicine, DateTime startDate, DateTime? endDate = null)
        {
            try
            {
                Log.Debug("Helseboka", $"Scheduling alarm for {day} and time {time}");
                if (!String.IsNullOrEmpty(time) && Int32.TryParse(time.Split(':').FirstOrDefault(), out var hour) && Int32.TryParse(time.Split(':').LastOrDefault(), out var minutes))
                {
                    var id = Convert.ToInt32(string.Format("{0}{1}{2}", medicine.Id, hour, minutes));
                    var intent = new Intent(context, typeof(AlarmReceiver));
                    intent.PutExtra(AndroidConstants.NotificationId, id);
                    intent.PutExtra(AndroidConstants.NotificationMessage, medicine.NameFormStrength);
                    intent.PutExtra(AndroidConstants.NotificationTitle, context.Resources.GetString(Resource.String.general_push_reminder_title));

                    var pendingIntent = PendingIntent.GetBroadcast(context, id, intent, PendingIntentFlags.UpdateCurrent);

                    /* Calculating time difference based on C# Datetime
                    // Calculate which will be the desired date time in which the alarm will go off for the first time
                    var differenceInNumberOfDaysBetweenDayOfWeek = day - DateTime.Today.DayOfWeek;
                    var nextDayOfWeek = differenceInNumberOfDaysBetweenDayOfWeek >= 0
                                            ? DateTime.Today.AddDays(differenceInNumberOfDaysBetweenDayOfWeek)
                                            : DateTime.Today.AddDays(differenceInNumberOfDaysBetweenDayOfWeek + 7);
                    var desiredDateTime = new DateTime(nextDayOfWeek.Year, nextDayOfWeek.Month, nextDayOfWeek.Day, hour, minutes, 0);

                    // this are the milliseconds left to arrive to the desired date time for the alarm to go off
                    var desiredMillisToArriveToDateTime = (desiredDateTime - DateTime.Now).Milliseconds;

                    var alarmManager = (AlarmManager)context.GetSystemService(Context.AlarmService);
                    alarmManager.SetRepeating(AlarmType.RtcWakeup, calendar.TimeInMillis, 7 * AlarmManager.IntervalDay, pendingIntent);
                    ******************/

                    Calendar calendar = Calendar.Instance;
                    // In Java Calendar.DayofWeek is from sunday(value 1) to Saturday(value 7). Where Dotnet DayOfWeek is Sunday (value 0) and Saturday(value 6)
                    // How stupid is that.
                    calendar.Set(CalendarField.DayOfWeek, (int)day + 1); 
                    calendar.Set(CalendarField.HourOfDay, hour);
                    calendar.Set(CalendarField.Minute, minutes);
                    calendar.Set(CalendarField.Second, 0);

                    if (calendar.TimeInMillis < Java.Lang.JavaSystem.CurrentTimeMillis())
                    {
                        Log.Debug("Helseboka", $"Adding 7 days as scheduled time is past");
                        calendar.Add(CalendarField.DayOfYear, 7);
                    }

                    var alarmManager = (AlarmManager)context.GetSystemService(Context.AlarmService);
                    alarmManager.SetRepeating(AlarmType.RtcWakeup, calendar.TimeInMillis, 7 * AlarmManager.IntervalDay, pendingIntent);

                    Log.Debug("Helseboka", $"Alarm scheduled for id {id}");
                    Log.Debug("Helseboka", $"Current date is   : {Calendar.Instance.Time.ToString()}");
                    Log.Debug("Helseboka", $"Alarm will fire at {calendar.Time.ToString()}");

                    return id;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Helseboka", ex.ToString());
            }

            return null;
        }

        public static void ScheduleNotificationForReminders(Context context, List<MedicineReminder> reminderList)
        {
            try
            {
                Log.Debug("Helseboka", $"Scheduling all notification - started");

                if (reminderList != null)
                {
                    foreach (var medicineDetails in reminderList)
                    {
                        if (medicineDetails.HasReminder && medicineDetails.Reminder != null)
                        {
                            foreach (var day in medicineDetails.Reminder.Days)
                            {
                                foreach (var time in medicineDetails.Reminder.FrequencyPerDay)
                                {
                                    ScheduleAlarmForReminder(context, day, time, medicineDetails.Medicine, medicineDetails.Reminder.StartDate, medicineDetails.Reminder.EndDate);
                                }
                            }
                        }
                    }
                }

                Log.Debug("Helseboka", $"Scheduling all notification - completed");
            }
            catch (Exception ex)
            {
                Log.Error("Helseboka", ex.ToString());
            }
        }

        public static void ShowNotification(Context context, int id, String title, String message)
        {
            try
            {
                NotificationCompat.Builder builder = new NotificationCompat.Builder(context, AndroidConstants.NotificationChannelId)
                                                                            .SetContentTitle(title)
                                                                            .SetContentText(message)
                                                                            .SetSmallIcon(Resource.Drawable.app_status_icon);

                builder.SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Alarm));
                builder.SetVibrate(new long[] { 1000, 1000, 1000, 1000, 1000 });
                builder.SetLights(Android.Graphics.Color.Red, 1, 1);

                Notification notification = builder.Build();


                NotificationManager notificationManager = context.GetSystemService(Context.NotificationService) as NotificationManager;

                notificationManager.Notify(id, notification);
                Log.Debug("Helseboka", $"Notification fired for id {id}");
            }
            catch (Exception ex)
            {
                Log.Error("Helseboka", ex.ToString());
            }
        }

        private void SaveToPermanentStorage(List<MedicineReminder> reminderList)
        {
            var serializer = ApplicationCore.Container.Resolve<ISerializer>();
            var response = serializer.Serialize<List<MedicineReminder>>(reminderList);
            if (response.isSuccess)
            {
                var jsonString = response.jsonString;

                var prefs = PreferenceManager.GetDefaultSharedPreferences(context);
                var editor = prefs.Edit();
                editor.PutString(SharedPreferenceKey.MedicineReminderList.ToString(), jsonString);
                editor.Apply();
            }
        }

        private void StartForegroundService()
        {
            Log.Debug("Helseboka", $"Request to Start Foreground service {NotificationsService.IsForegroundServiceRunning}");
            if (!NotificationsService.IsForegroundServiceRunning && context != null)
            {
                try
                {
                    var intent = new Intent(context, typeof(NotificationsService));
                    if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
                    {
                        context.StartForegroundService(intent);
                    }
                    else
                    {
                        context.StartService(intent);
                    }
                    NotificationsService.IsForegroundServiceRunning = true;
                    Log.Debug("Helseboka", $"Foreground Service started");
                }
                catch (Exception ex)
                {
                    Log.Error("Helseboka", ex.ToString());
                }
            }
                
        }
    }
}
