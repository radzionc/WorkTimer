using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V4.Content;

namespace XamarinWorkTimer.Droid.Implementations
{
    [Service]
    class NotificationService : IntentService
    {
        protected override void OnHandleIntent(Intent intent)
        {
            var manager = PowerManager.FromContext(this);
            var wakeLock = manager.NewWakeLock(WakeLockFlags.ScreenBright | WakeLockFlags.Full | WakeLockFlags.AcquireCausesWakeup, "WakeLockTag");
            wakeLock.Acquire(60000);
        }
    }
}