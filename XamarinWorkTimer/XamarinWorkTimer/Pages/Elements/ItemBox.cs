using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamarinWorkTimer.DataBase;
using Xamarin.Forms;

namespace XamarinWorkTimer.Pages.Elements
{
    public partial class ItemBox : BoxView
    {
        public ItemBox(Interval interval)
        {
            _Interval = interval;
            Color = Color.Gray;
            
        }

        public Interval _Interval { get; private set; }
    }
}
