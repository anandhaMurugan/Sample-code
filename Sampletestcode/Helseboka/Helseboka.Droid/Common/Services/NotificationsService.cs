using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Preferences;
using Android.Support.V4.App;
using Android.Util;
using Android.Widget;
using Helseboka.Core.Common.CommonImpl;
using Helseboka.Core.MedicineModule.Model;
using Helseboka.Droid.Common.CommonImpl;
using Helseboka.Droid.Common.Receivers;
using System.Linq;
using Java.Util;
using Helseboka.Droid.Common.Constants;

namespace Helseboka.Droid.Common.Services
{
    [Service]
    public class NotificationsService : Service
    {
        public static bool IsForegroundServiceRunning { get; set; } = false;

        public override void OnCreate()
        {
            base.OnCreate();
            Log.Debug("Helseboka", $"Notifications Service - Started");
            RegisterForegroundService();
        }

        public override StartCommandResult OnStartCommand(Android.Content.Intent intent, StartCommandFlags flags, int startId)
        {
            Log.Debug("Helseboka", $"Notifications Service - Started {intent.Action}");

            try
            {
                if (intent.Action == AndroidConstants.RegisterMedicineReminderAlarm)
                {
                    var serializer = new JSONSerializer();
                    var prefs = PreferenceManager.GetDefaultSharedPreferences(this);
                    if (prefs != null)
                    {
                        var jsonString = prefs.GetString(SharedPreferenceKey.MedicineReminderList.ToString(), "");
                        Log.Debug("Helseboka", $"Saved Data {jsonString}");
                        if (!String.IsNullOrEmpty(jsonString))
                        {
                            var response = serializer.Deserialize<List<MedicineReminder>>(jsonString);

                            if (response.isSuccess && response.result != null)
                            {
                                Log.Debug("Helseboka", $"Saved Data Count {response.result.Count}");
                                NotificationHandler.ScheduleNotificationForReminders(this, response.result);
                            }
                            else
                            {
                                Log.Error("Helseboka", $"Parsing Pailed");
                            }
                        }
                        else
                        {
                            Log.Error("Helseboka", $"saved Data is null");
                        }
                    }
                    else
                    {
                        Log.Error("Helseboka", $"Pref is null");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Helseboka", $"Exception is {ex.ToString()}");
            }

            return StartCommandResult.Sticky;
        }

        public override Android.OS.IBinder OnBind(Android.Content.Intent intent)
        {
            return null;
        }


        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        void RegisterForegroundService()
        {
            try
            {
                Log.Debug("Helseboka", $"Notifications Service - Foreground notification fired");
                var title = Resources.GetString(Resource.String.app_name);
                var message = Resources.GetString(Resource.String.general_push_foreground_service_description);
                var notification = new NotificationCompat.Builder(this, AndroidConstants.NotificationChannelId)
                                                         .SetContentTitle(title)
                                                         .SetContentText(message)
                                                         .SetSmallIcon(Resource.Drawable.app_status_icon)
                                                         .SetOngoing(true)
                                                         .Build();

                StartForeground(AndroidConstants.ForegroundServiceNotificationId, notification);
            }
            catch (Exception ex)
            {
                Log.Error("Helseboka", $"Exception is {ex.ToString()}");
            }
        }
    }
}
