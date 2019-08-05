using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Helseboka.Droid.Common.Services;

namespace Helseboka.Droid.Common
{
    //[Application]
    //public class HelsebokaApplication : Android.App.Application
    //{
    //    public static Context AppContext;

    //    public HelsebokaApplication(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
    //    {

    //    }

    //    public override void OnCreate()
    //    {
    //        base.OnCreate();

    //        AppContext = this.ApplicationContext;

    //        StartPushService();
    //    }

    //    public static void StartPushService()
    //    {
    //        AppContext.StartService(new Intent(AppContext, typeof(NotificationsService)));

    //        //if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Kitkat)
    //        //{
    //        //    PendingIntent pintent = PendingIntent.GetService(AppContext, 0, new Intent(AppContext, typeof(NotificationsService)), 0);
    //        //    AlarmManager alarm = (AlarmManager)AppContext.GetSystemService(Context.AlarmService);
    //        //    alarm.Cancel(pintent);
    //        //}
    //    }

    //    public static void StopPushService()
    //    {
    //        AppContext.StopService(new Intent(AppContext, typeof(NotificationsService)));

    //        //PendingIntent pintent = PendingIntent.GetService(AppContext, 0, new Intent(AppContext, typeof(NotificationsService)), 0);
    //        //AlarmManager alarm = (AlarmManager)AppContext.GetSystemService(Context.AlarmService);
    //        //alarm.Cancel(pintent);
    //    }
    //}
}
