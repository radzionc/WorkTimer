using Android.Content;
using Android.App;
using Xamarin.Forms;
using Android.OS;

[assembly: Dependency(typeof(XamarinWorkTimer.Droid.AndroidReminderService))]

namespace XamarinWorkTimer.Droid
{
    public class AndroidReminderService : IReminderService
    {
        public void Remind(int seconds, string title, string message)
        {

            Intent alarmIntent = new Intent(Forms.Context, typeof(AlarmReceiver));
            alarmIntent.PutExtra("message", message);
            alarmIntent.PutExtra("title", title);

            PendingIntent pendingIntent = PendingIntent.GetBroadcast(Forms.Context, 0, alarmIntent, PendingIntentFlags.UpdateCurrent);
            AlarmManager alarmManager = (AlarmManager)Forms.Context.GetSystemService(Context.AlarmService);

            alarmManager.Set(AlarmType.ElapsedRealtimeWakeup, SystemClock.ElapsedRealtime() + seconds * 1000, pendingIntent);
        }

        public void CancelRemind()
        {
            
        }
    }
}
