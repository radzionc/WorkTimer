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
            Color = Color.Silver;
            int startTime = g.StrToSec(interval.StartPK);
            _Rectangle = new Rectangle((double)startTime/ g.secondsInDay, 0, (double)interval.Sum/ g.secondsInDay, 1);
        }
        public Rectangle _Rectangle { get; private set; }
    }
}
