using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamarinWorkTimer
{
    public static class g
    {
        public static string item = "item";
        public static string interval = "interval";
        public static string sum = "sum";
        public static string slider = "slider";
        public static string lastTime = "lastTime";
        public static string dateFormat = "dd:MM:yyyy";
        public static string timeFormat = "HH:mm:ss";
        public static int secondsInDay = 86400;

        public static string strToSec(int seconds)
        {
            TimeSpan sec = TimeSpan.FromSeconds(seconds);

            if (seconds < 3600)
                return sec.ToString(@"mm\:ss");
            else if (seconds == 3600)
                return "60:00";
            else
                return sec.ToString(@"hh\:mm\:ss");
        }
        
        public static int secToStr(string time)
        {
            string[] times = time.Split(':');
            return int.Parse(times[0]) * 3600 + int.Parse(times[1]) * 60 + int.Parse(times[2]);
        }
    }
}
