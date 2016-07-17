using System;
using XamarinWorkTimer;
using Xamarin.Forms;
using XamarinWorkTimer.Droid;
using System.IO;

[assembly: Dependency(typeof(Notification))]

namespace XamarinWorkTimer.Droid
{
    class Notification : INotification
    {
        public void NotifaicationTime(long time)
        {
            
        }
    }
}