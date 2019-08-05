using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Util;

using Firebase.Messaging;
using Helseboka.Droid.Common.Constants;
using Helseboka.Droid.Startup;

namespace Helseboka.Droid.Common.Services
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class HelsebokaFirebaseMessagingService : FirebaseMessagingService
    {
        public override void OnMessageReceived(RemoteMessage message)
        {
            var body = message.GetNotification().Body;
            var title = message.GetNotification().Title;
            SendNotification(title, body, message.Data);
        }

        void SendNotification(String title, string messageBody, IDictionary<string, string> data)
        {
            var notificationBuilder = new NotificationCompat.Builder(this, AndroidConstants.NotificationChannelId)
                                                            .SetSmallIcon(Resource.Drawable.app_status_icon)
                                                            .SetContentTitle(title)
                                                            .SetContentText(messageBody)
                                                            .SetAutoCancel(true);

            var notificationManager = NotificationManagerCompat.From(this);
            notificationManager.Notify(10, notificationBuilder.Build());
        }
    }
}
