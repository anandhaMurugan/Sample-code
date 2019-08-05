using System;
using Android.App;
using Android.Util;

using Firebase.Iid;
using Helseboka.Droid.Common.CommonImpl;

namespace Helseboka.Droid.Common.Services
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class HelsebokaFirebaseIIDService : FirebaseInstanceIdService
    {
        public override void OnTokenRefresh()
        {
            var refreshedToken = FirebaseInstanceId.Instance.Token;
            SendRegistrationToServer(refreshedToken);
        }

        void SendRegistrationToServer(string token)
        {
            NotificationHandler.Instance.UpdateNotificationToken(token).Wait();
        }
    }
}
