using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using Helseboka.iOS.Common.Utilities;
using UIKit;

namespace Helseboka.iOS
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, "HelsebokaApplication", "AppDelegate");
        }
    }

    [Register("HelsebokaApplication")]
    public class HelsebokaApplication : UIApplication
    {
        public HelsebokaApplication() { }

        public HelsebokaApplication(IntPtr handler) : base(handler) { }

        public override void SendEvent(UIEvent uievent)
        {
            base.SendEvent(uievent);
            InActivityHandler.Instance.Reset();
        }
    }
}
