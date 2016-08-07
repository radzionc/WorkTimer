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
        public StatisticPage()
        {
            InitializeComponent();

            var fs = new FormattedString();
            fs.Spans.Add(new Span { Text = performanceLabel.Text });
            fs.Spans.Add(new Span
            {
                Text = g.SecToStr(g.TodaySum()),
                ForegroundColor = Color.Silver
            });
            performanceLabel.FormattedText = fs;

            Dictionary<string, int> items = new Dictionary<string, int>();

            foreach (Interval interval in g.intervalDB.GetAll())
            {
                IntervalBox box = new IntervalBox(interval);
                intervalLayout.Children.Add(box, box._Rectangle, AbsoluteLayoutFlags.All);

                if (!items.ContainsKey(interval.Name))
                    items.Add(interval.Name, interval.Sum);
                else
                    items[interval.Name] += interval.Sum;
            }

            foreach (var item in items.OrderBy(x => x.Value))
                itemLayout.Children.Add(new StatisticLine(item.Key, item.Value));

            StackLayout sl = new StackLayout();
            sl.Children.Add(new BoxView() { Color = Color.Silver, WidthRequest = 100, HeightRequest = 2 });
            sl.Children.Add(new NavigationLine());
            grid.Children.Add(sl, 0, 3);
        }
    }
}
