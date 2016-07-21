using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamarinWorkTimer
{
    public static class gf
    {
        public static string database = "database";
        public static string intervalsDatabase = "intervalsDatabase";
        public static string summaryDatabase = "summaryDatabase";
        public static string slider = "slider";
        public static string lastTime = "lastTime";
        public static string stopTimer = "stopTimer";
        public static string chooseLine = "chooseLine";
        public static string pause = "pause";
        public static string resume = "resume";
        public static string dateFormat = "dd/MM/yyyy";
        public static string timeFormat = "hh/mm/ss";

        public static void PropertiesCheck()
        {
            if (!App.Current.Properties.ContainsKey(slider))
                App.Current.Properties[slider] = 45;
            if (!App.Current.Properties.ContainsKey(lastTime))
                App.Current.Properties[lastTime] = DateTime.Now;
            if (!App.Current.Properties.ContainsKey(stopTimer))
                App.Current.Properties[stopTimer] = true;
            if (!App.Current.Properties.ContainsKey(chooseLine))
                App.Current.Properties[chooseLine] = null;
        }
        public static string FromSecondsToString(int seconds)
        {
            TimeSpan sec = TimeSpan.FromSeconds(seconds);

            if (seconds < 3600)
                return sec.ToString(@"mm\:ss");
            else if (seconds == 3600)
                return "60:00";
            else
                return sec.ToString(@"hh\:mm\:ss");
        }
    }
}
