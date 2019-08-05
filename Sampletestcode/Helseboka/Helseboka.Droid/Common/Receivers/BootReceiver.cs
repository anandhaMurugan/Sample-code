using System;
using Android.App;
using Android.Content;
using Android.Preferences;
using Android.Util;
using Android.Widget;
using Helseboka.Droid.Common.Constants;
using Helseboka.Droid.Common.Services;

namespace Helseboka.Droid.Common.Receivers
{
    [BroadcastReceiver(Enabled = true, Exported = true, DirectBootAware = true)]
    [IntentFilter(new string[] { Intent.ActionBootCompleted, Intent.ActionLockedBootCompleted, "android.intent.action.QUICKBOOT_POWERON", "com.htc.intent.action.QUICKBOOT_POWERON" })]
    public class BootReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            try
            {
                Log.Debug("Helseboka", $"BootReceiver Received intent!");
                
                Intent i = new Intent(context, typeof(NotificationsService));
                i.SetAction(AndroidConstants.RegisterMedicineReminderAlarm);
                context.StartService(i);

                NotificationsService.IsForegroundServiceRunning = true;

                Log.Debug("Helseboka", $"NotificationsService Started");
            }
            catch (Exception ex)
            {
                Log.Debug("Helseboka", $"Exception is {ex.ToString()}");
            }
        }
    }
}
