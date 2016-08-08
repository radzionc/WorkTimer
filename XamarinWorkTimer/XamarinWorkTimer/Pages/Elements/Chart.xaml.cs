using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using XamarinWorkTimer.Pages.Elements;

namespace XamarinWorkTimer.Pages
{
    public partial class Chart : Grid
    {
        int barsOnOnePage;
        int bars;
        int barSize;
        Dictionary<DateTime, int> sums = new Dictionary<DateTime, int>();
        DateTime first = g.StrToDate(g.sumDB.GetAll().First().DatePK);
        DateTime last = DateTime.Today;
        string type; 

        public Chart(string type)
        {
            this.type = type;
            InitializeComponent();

            foreach (Sum sum in g.sumDB.GetAll())
                sums.Add(g.StrToDate(sum.DatePK), sum.Value);
            sums.Add(DateTime.Today, g.TodaySum());

            sizeCreation();
            BarsCreation();
        }

        void sizeCreation()
        {
            if (type == g.day)
            {
                barsOnOnePage = 11;
                bars = (int)(last - first).TotalDays;
            }
            else if (type == g.week)
            {
                barsOnOnePage = 6;
                bars = Math.Abs((last.Month - first.Month) + 12 * (last.Year - first.Year));
            }
            else if (type == g.month)
            {
                barsOnOnePage = 5;
                bars = (int)((last - first).TotalDays / 7);
            }
            barSize = 1000 / barsOnOnePage;
        }
        void BarsCreation()
        {
            List<View> views = new List<View>();

            for (DateTime date = first; date <= last; date = date.AddDays(1.0))
            {
                if(type == g.day)
                {
                    int sum = 0;
                    if (sums.ContainsKey(date))
                        sum = sums[date];
                    BoxView boxView = new BoxView
                    {
                        Color = Color.Teal,
                        HeightRequest = sum/g.secondsInDay * 1000,
                        VerticalOptions = LayoutOptions.End
                    };
                    views.Add(boxView);
                }
                else if(type == g.week)
                {
                    int sum = 0;
                    for(DateTime d = date; d <= last; date = date.AddDays(1.0))
                    {
                        if (g.GetIso8601WeekOfYear(d) != g.GetIso8601WeekOfYear(date) || d == last)
                        {
                            date = d;
                            BoxView boxView = new BoxView
                            {
                                Color = Color.Teal,
                                HeightRequest = sum / 7 / g.secondsInDay * 1000,
                                VerticalOptions = LayoutOptions.End
                            };
                            views.Add(boxView);
                            break;
                        }

                        if (sums.ContainsKey(d))
                            sum += sums[d];
                    }
                }

                else if (type == g.month)
                {
                    int sum = 0;
                    for (DateTime d = date; d <= last; date = date.AddDays(1.0))
                    {
                        if (d.Month == date.Month && sums.ContainsKey(d))
                            sum += sums[d];

                        if (d.Month != date.Month || d == last)
                        {
                            date = d;
                            BoxView boxView = new BoxView
                            {
                                Color = Color.Teal,
                                HeightRequest = sum / DateTime.DaysInMonth(date.Year, date.Month) / g.secondsInDay * 1000,
                                VerticalOptions = LayoutOptions.End
                            };
                            views.Add(boxView);
                            break;
                        }
                    }
                }
            }

            for (int i = 0; i < views.Count(); i++)
            {
                chart.RowDefinitions.Add(new RowDefinition { Height = new GridLength(barSize) });
                chart.Children.Add(views[i], 0, i);
            }
        }
    }
}
