using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using XamarinWorkTimer;
using Android.Content;

namespace XamarinWorkTimer.Droid
{
    [Activity(Label = "XamarinWorkTimer", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
    ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        App app;
        public static bool IsActive { get; set; }
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            this.Window.SetFlags(WindowManagerFlags.KeepScreenOn, WindowManagerFlags.KeepScreenOn);
            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(app = new App());
        }

        public override void OnBackPressed()
        {
            if (!app.InStartPage)
                app.OnStartPage();
            else
                MoveTaskToBack(true);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            AndroidReminderService.pendingIntent?.Cancel();
        }

        protected override void OnResume()
        {
            base.OnResume();
            IsActive = true;
        }
        protected override void OnPause()
        {
            base.OnPause();
            IsActive = false;
        }
    }

}

