using System;
using Android.App;
using Android.Content;
using Android.Media;
using Android.Support.V4.App;
using Android.Util;
using Android.Widget;
using Helseboka.Droid.Common.CommonImpl;
using Helseboka.Droid.Common.Constants;

namespace Helseboka.Droid.Common.Receivers
{
    [BroadcastReceiver(Enabled = true)]
    public class AlarmReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            try
            {
                Log.Debug("Helseboka", $"Alarm manager received");
                var title = intent.GetStringExtra(AndroidConstants.NotificationTitle);
                var message = intent.GetStringExtra(AndroidConstants.NotificationMessage);
                var id = intent.GetIntExtra(AndroidConstants.NotificationId, 0);
                Log.Debug("Helseboka", $"Showing Notification with id {id} {title} and {message}");
                NotificationHandler.ShowNotification(context, id, title, message);
            }
            catch (Exception ex)
            {
                Log.Error("Helseboka", ex.ToString());
            }
        }


    }
}
