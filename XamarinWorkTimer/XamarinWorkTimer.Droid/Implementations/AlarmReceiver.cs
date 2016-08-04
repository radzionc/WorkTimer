using Android.Content;
using Android.App;
using Android.Support.V4.App;
using Android.Graphics;
using Android.Support.V4.Content;

namespace XamarinWorkTimer.Droid
{
    [BroadcastReceiver]
    public class AlarmReceiver : WakefulBroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            StartWakefulService(context, intent);

            string message = intent.GetStringExtra("message");
            string title = intent.GetStringExtra("title");

            Intent notIntent = new Intent(context, typeof(MainActivity));
            PendingIntent contentIntent = PendingIntent.GetActivity(context, 0, notIntent, PendingIntentFlags.CancelCurrent);
            NotificationManager manager = NotificationManager.FromContext(context);
            var style = new Notification.BigTextStyle();
            style.BigText(message);

            int resourceId = Resource.Drawable.icon;

            var wearableExtender = new Notification.WearableExtender()
    .SetBackground(BitmapFactory.DecodeResource(context.Resources, resourceId))
                ;

            //Generate a notification with just short text and small icon
            var builder = new Notification.Builder(context)
                .SetPriority((int)NotificationPriority.Max)
                .SetVisibility(NotificationVisibility.Public)
                .SetDefaults(NotificationDefaults.All)
                .SetCategory(Notification.CategoryAlarm)
                .SetContentIntent(contentIntent)
                .SetSmallIcon(Resource.Drawable.icon)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetWhen(Java.Lang.JavaSystem.CurrentTimeMillis())
                .SetAutoCancel(true);

            var notification = builder.Build();
            manager.Notify(0, notification);
        }
    }
}