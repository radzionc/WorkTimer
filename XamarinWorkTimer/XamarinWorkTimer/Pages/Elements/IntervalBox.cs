using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using XamarinWorkTimer.DataBase;

namespace XamarinWorkTimer.Pages.Elements
{
    public class IntervalBox : BoxView
    {
        public IntervalBox(Interval interval)
        {
            _Interval = interval;
            Color = Color.Red;
            int startTime = g.strToSec(interval.StartPK);
            _Rectangle = new Rectangle((double)startTime/ g.secondsInHour, 0, (double)interval.Sum/ g.secondsInHour, 1);
            Log = $"{interval.Name} : {interval.StartPK} - {g.strToSec((g.strToSec(interval.StartPK) + interval.Sum))} : {g.strToSec(interval.Sum)}";
        }

        public Rectangle _Rectangle { get; private set; }
        public Interval _Interval { get; private set; }
        public string Log { get; private set; }
    }
}
