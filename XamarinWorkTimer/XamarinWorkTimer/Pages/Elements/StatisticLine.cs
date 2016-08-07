using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace XamarinWorkTimer.Pages.Elements
{
    public class StatisticLine : StackLayout
    {
        public StatisticLine(string name, int time)
        {
            Orientation = StackOrientation.Horizontal;
            Padding = 5;
            
            Children.Add(new Label
            {
                Text = name + " : ",
                HorizontalOptions = LayoutOptions.StartAndExpand
            });

            Children.Add(new Label
            {
                Text = g.SecToStr(time),
                HorizontalOptions = LayoutOptions.End
            });
        }
    }
}
