using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XamarinWorkTimer.DataBase;


namespace XamarinWorkTimer
{
    public static class g
    {
        public static string item = "item";
        public static string interval = "interval";
        public static string sum = "sum";
        public static string stop = "Stop";
        public static string choose = "Choose Job?";
        public static string slider = "slider";
        public static string lastTime = "lastTime";
        public static string dateFormat = "dd:MM:yyyy";
        public static string timeFormat = "HH:mm:ss";

        public static string month = "month";
        public static string week = "week";
        public static string today = "today";
        public static int secondsInDay = 86400;

        public static int period
        {
            get { return (int)App.Current.Properties[slider]; }
            set { App.Current.Properties[slider] = value; }
        }
        public static bool timer = false;

        public static DB<Item> itemDB = new DB<Item>(item);
        public static DB<Interval> intervalDB = new DB<Interval>(interval);
        public static DB<Sum> sumDB = new DB<Sum>(sum);

        public static  int TodaySum()
        {
            int result = 0;
            foreach(Interval interval in intervalDB.GetAll())
                result += interval.Sum;

            return result;
        }
        public static string SecToStr(int seconds)
        {
            TimeSpan sec = TimeSpan.FromSeconds(seconds);

            if (seconds < 3600)
                return sec.ToString(@"mm\:ss");
            else if (seconds == 3600)
                return "60:00";
            else
                return sec.ToString(@"hh\:mm\:ss");
        }
        
        public static int StrToSec(string time)
        {
            string[] times = time.Split(':');
            return int.Parse(times[0]) * 3600 + int.Parse(times[1]) * 60 + int.Parse(times[2]);
        }
    }
}
