using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XamarinWorkTimer.DataBase;


namespace XamarinWorkTimer
{
    public static class g
    {
        public const string item = "item";
        public const string interval = "interval";
        public const string sum = "sum";
        public const string stop = "Stop";
        public const string choose = "Choose Job?";
        public const string slider = "slider";
        public const string lastTime = "lastTime";
        public const string dateFormat = "dd:MM:yyyy";
        public const string timeFormat = "HH:mm:ss";
              
        public const string month = "months";
        public const string week = "weeks";
        public const string day = "days";
        public const string today = "today";
        public const int secondsInDay = 86400;

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

        public static DateTime StrToDate(string date)
        {
            return DateTime.ParseExact(date, g.dateFormat, CultureInfo.InvariantCulture);
        }
        public static string SecToStr(int seconds)
        {
            TimeSpan sec = TimeSpan.FromSeconds(seconds);
            if (seconds > secondsInDay)
                return sec.ToString(@"0:dd\.hh\:mm\:ss") + "days";
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

        public static int GetIso8601WeekOfYear(DateTime time)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }
    }
}
