using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Foundation;
using Helseboka.Core.Auth.Model;
using Helseboka.Core.Common.Interfaces;
using Helseboka.Core.Common.Model;
using Helseboka.Core.Common.Extension;
using Helseboka.Core.MedicineModule.Model;
using UIKit;
using UserNotifications;
using Helseboka.iOS.Common.Extension;

namespace Helseboka.iOS.Notification
{
    public class NotificationService : INotificationService
    {
        #region Singleton Implementation
        //Lazy singleton initialization
        // http://csharpindepth.com/Articles/General/Singleton.aspx
        // Double-check-Locking will not be required - https://msdn.microsoft.com/en-us/library/ff650316.aspx
        private NotificationService()
        {
        }

        public static NotificationService Instance { get { return Nested.instance; } }

        private class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }

            internal static readonly NotificationService instance = new NotificationService();
        }
        #endregion

        private String notificationToken;

        public void Register()
        {
            if(UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound, (bool arg1, Foundation.NSError arg2) =>
                {
                    UIDevice.CurrentDevice.InvokeOnMainThread(() =>
                    {
                        UIApplication.SharedApplication.RegisterForRemoteNotifications();
                    });
                });
            }
            else
            {
                var notificationSettings = UIUserNotificationSettings.GetSettingsForTypes(
                 UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound, null
                );

                UIApplication.SharedApplication.RegisterUserNotificationSettings(notificationSettings);
                UIApplication.SharedApplication.RegisterForRemoteNotifications();
            }
        }

        public async Task UpdateNotificationToken(NSData deviceToken)
        {
            var pushToken = deviceToken.Description;
            if (!string.IsNullOrWhiteSpace(pushToken))
            {
                pushToken = pushToken.Trim('<').Trim('>').Replace(" ","");
            }

            var oldDeviceToken = NSUserDefaults.StandardUserDefaults.StringForKey("PushDeviceToken");

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
            var response = await ApplicationCore.Instance.CurrentUser.UpdateNotificationToken("IOS", pushToken);
            if (response.IsSuccess)
            {
                NSUserDefaults.StandardUserDefaults.SetString(pushToken, "PushDeviceToken");
            }
        }

        public async Task ScheduleNotification(List<MedicineReminder> reminderList)
        {
            await DeleteAllNotification();
            if(reminderList != null)
            {
                foreach (var medicineDetails in reminderList)
                {
                    if(medicineDetails.HasReminder && medicineDetails.Reminder != null)
                    {
                        foreach (var day in medicineDetails.Reminder.Days)
                        {
                            foreach (var time in medicineDetails.Reminder.FrequencyPerDay)
                            {
                                ScheduleLocalNotification(day, time, medicineDetails.Medicine, medicineDetails.Reminder.StartDate, medicineDetails.Reminder.EndDate).Forget();
                            }
                        }
                    }
                }
            }
        }

        private async Task DeleteAllNotification()
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                var pendingNotificationList = await UNUserNotificationCenter.Current.GetPendingNotificationRequestsAsync();
                if (pendingNotificationList != null && pendingNotificationList.Count() > 0)
                {
                    var notificationToDelete = pendingNotificationList.Select(x => x.Identifier).ToList();
                    UIDevice.CurrentDevice.InvokeOnMainThread(() => UNUserNotificationCenter.Current.RemovePendingNotificationRequests(notificationToDelete.ToArray()));
                }
            }
        }

        public async Task DeleteScheduledNotification(MedicineInfo medicine)
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                var pendingNotificationList = await UNUserNotificationCenter.Current.GetPendingNotificationRequestsAsync();
                if (pendingNotificationList != null && pendingNotificationList.Count() > 0)
                {
                    var notificationToDelete = new List<String>();
                    foreach (var notification in pendingNotificationList)
                    {
                        if (notification.Identifier.StartsWith(medicine.Id.ToString(), StringComparison.OrdinalIgnoreCase))
                        {
                            notificationToDelete.Add(notification.Identifier);
                        }
                    }

                    if (notificationToDelete.Count > 0)
                    {
                        UIDevice.CurrentDevice.InvokeOnMainThread(() => UNUserNotificationCenter.Current.RemovePendingNotificationRequests(notificationToDelete.ToArray()));
                    }
                }
            }
        }

        public async Task ScheduleLocalNotification(DayOfWeek day, String time, MedicineInfo medicine, DateTime startDate, DateTime? endDate = null)
        {
            await Task.FromResult<bool>(ScheduleLocalNotificationInternal(day, time, medicine, startDate, endDate));
        }

        private bool ScheduleLocalNotificationInternal(DayOfWeek day, String time, MedicineInfo medicine, DateTime startDate, DateTime? endDate = null)
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                if (!String.IsNullOrEmpty(time) && Int32.TryParse(time.Split(':').FirstOrDefault(), out var hour) && Int32.TryParse(time.Split(':').LastOrDefault(), out var minutes))
                {
                    var dateComponents = new NSDateComponents();
                    dateComponents.Weekday = ((int)day + 1);
                    dateComponents.Hour = hour;
                    dateComponents.Minute = minutes;

                    var trigger = UNCalendarNotificationTrigger.CreateTrigger(dateComponents, true);

                    var notificationContent = new UNMutableNotificationContent();
                    notificationContent.Title = "General.Push.Reminder.Title".Translate();
                    notificationContent.Body = medicine.NameFormStrength;
                    var sound = UNNotificationSound.GetSound("alarm.caf");
                    notificationContent.Sound = sound;

                    var userInfo = new Dictionary<String, String>();
                    userInfo.Add("day", day.ToString());
                    userInfo.Add("hour", hour.ToString());
                    userInfo.Add("minute", minutes.ToString());
                    userInfo.Add("medicineId", medicine.Id.ToString());
                    userInfo.Add("medicineName", medicine.Name);
                    userInfo.Add("medicineStrength", medicine.Strength);
                    userInfo.Add("startDate", startDate.ToString("dd-MM-yyyy"));
                    userInfo.Add("endDate", endDate.HasValue ? endDate.Value.ToString("dd-MM-yyyy") : String.Empty);

                    notificationContent.UserInfo = NSDictionary.FromObjectsAndKeys(userInfo.Values.ToArray(), userInfo.Keys.ToArray());

                    var notificationRequest = UNNotificationRequest.FromIdentifier($"{medicine.Id}-{day}-{hour}-{minutes}", notificationContent, trigger);

                    UIDevice.CurrentDevice.InvokeOnMainThread(async () => await UNUserNotificationCenter.Current.AddNotificationRequestAsync(notificationRequest));
                    return true;
                }
            }
            else
            {
                throw new NotImplementedException("Local pushnotification for iOS lower than 10 is not implemented");
            }

            return false;
        }
    }
}
