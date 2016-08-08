using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace XamarinWorkTimer.Pages.Elements
{
    public partial class Today : Grid
    {
        public Today()
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

            foreach (var item in items.OrderBy(x => -x.Value))
                itemLayout.Children.Add(new StatisticLine(item.Key, item.Value));
        }
    }
}
