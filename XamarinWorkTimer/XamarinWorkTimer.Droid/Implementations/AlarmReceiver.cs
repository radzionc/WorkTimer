using Android.Content;
using Android.App;
using Android.Support.V4.Content;
using XamarinWorkTimer.Droid.Implementations;

namespace XamarinWorkTimer.Droid
{
    [BroadcastReceiver]
    public class AlarmReceiver : WakefulBroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            string message = intent.GetStringExtra("message");
            string title = intent.GetStringExtra("title");

            Intent notIntent = new Intent(context, typeof(NotificationService));
                
            PendingIntent contentIntent = PendingIntent.GetActivity(context, 0, notIntent, PendingIntentFlags.CancelCurrent);
            NotificationManager manager = NotificationManager.FromContext(context);

            var bigTextStyle = new Notification.BigTextStyle()
                        .SetBigContentTitle(title)
                        .BigText(message);

            Notification.Builder builder = new Notification.Builder(context)
                .SetContentIntent(contentIntent)
                .SetSmallIcon(Resource.Drawable.icon)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetStyle(bigTextStyle)
                .SetWhen(Java.Lang.JavaSystem.CurrentTimeMillis())
                .SetAutoCancel(true)
                .SetPriority((int)NotificationPriority.High)
                .SetVisibility(NotificationVisibility.Public)
                .SetDefaults(NotificationDefaults.Vibrate)
                .SetCategory(Notification.CategoryAlarm);

            if (!MainActivity.IsActive)
            {
                manager.Notify(0, builder.Build());
                StartWakefulService(context, notIntent);
            }
        }
    }
}