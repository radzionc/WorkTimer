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
        public static string items = "items";
        public static string intervals = "intervals";
        public static string sums = "sums";
        public static string slider = "slider";
        public static string lastTime = "lastTime";
        public static string dateFormat = "dd/MM/yyyy";
        public static string timeFormat = "hh/mm/ss";
        public static int secInDay = 86400;

        public static ItemDB itemDB = new ItemDB(items);
        public static DB<Interval> intervalDB = new DB<Interval>(intervals);
        public static DB<Summary> summaryDB = new DB<Summary>(sums);
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
