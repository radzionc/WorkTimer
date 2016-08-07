using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using XamarinWorkTimer.DataBase;
using XamarinWorkTimer.Pages.Elements;

namespace XamarinWorkTimer.Pages
{
    public partial class StatisticPage : ContentPage
    {
        IntervalBox previous;
        
        public StatisticPage(List<Interval> intervals)
        {
            InitializeComponent();

            performanceLabel.Text += g.SecToStr(g.TodaySum());
            Dictionary<string, int> items = new Dictionary<string, int>();

            foreach (Interval interval in intervals)
            {
                IntervalBox box = new IntervalBox(interval);

                if(!items.ContainsKey(interval.Name))
                    items.Add(interval.Name, interval.Sum);
                        else
                items[interval.Name] += interval.Sum;

                box.GestureRecognizers.Add(new TapGestureRecognizer()
                {
                    Command = new Command(() =>
                    {
                        if (previous != null)
                            previous.Choosen = false;
                        box.Choosen = true;
                        intervalLabel.Text = box.Log;
                        previous = box;
                    })
                });
                intervalLayout.Children.Add(box, box._Rectangle, AbsoluteLayoutFlags.All);
            }

            foreach (var item in items)
                itemLayout.Children.Add(new StatisticLine(item.Key, item.Value));
        }
    }
}
