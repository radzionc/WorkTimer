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
        Color True = Color.Purple;
        Color False = Color.Navy;
        public bool Choosen { set { Color = value ? True : False; } }
        public IntervalBox(Interval interval)
        {
            _Interval = interval;
            Color = False;
            int startTime = g.secToStr(interval.StartPK);
            _Rectangle = new Rectangle((double)startTime/ g.secondsInDay, 0, (double)interval.Sum/ g.secondsInDay, 1);
            Log = $"{interval.Name} : {interval.StartPK} - {g.strToSec((g.secToStr(interval.StartPK) + interval.Sum))} : {g.strToSec(interval.Sum)}";
        }
        public Rectangle _Rectangle { get; private set; }
        public Interval _Interval { get; private set; }
        public string Log { get; private set; }
    }
}
