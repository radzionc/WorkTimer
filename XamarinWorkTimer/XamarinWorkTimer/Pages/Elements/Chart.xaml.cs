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
        double barSize;
        BoxView previousBox;
        Dictionary<DateTime, int> sums = new Dictionary<DateTime, int>();
        DateTime first = g.StrToDate(g.sumDB.GetAll().First().DatePK);
        DateTime last = g.StrToDate(g.sumDB.GetAll().Last().DatePK);
        string type;
        int position = 0;

        public Chart(string type)
        {
            this.type = type;
            InitializeComponent();

            foreach (Sum sum in g.sumDB.GetAll())
                sums.Add(g.StrToDate(sum.DatePK), sum.Value);
            //sums.Add(DateTime.Today, g.TodaySum());

            sizeCreation();
            BarsCreation();
        }

        void sizeCreation()
        {
            if (type == g.day)
                barSize = 40;
            else if (type == g.week)
                barSize = 60;
            else if (type == g.month)
                barSize = 80;
        }
        void BarsCreation()
        {
            for (DateTime date = first; date <= last; date = date.AddDays(1.0))
            {
                int sum = 0;

                if(type == g.day)
                {
                    if (sums.ContainsKey(date))
                        sum = sums[date];

                    string name;
                    if (date.Date == DateTime.Today)
                        name = "Today";
                    else if (date.Date.AddDays(1.0) == DateTime.Today)
                        name = "Yesterday";
                    else
                        name = date.ToString(g.dateFormat);
                    AddBar(name, (double)sum / g.secondsInDay * 600, sum);
                }
                else if(type == g.week)
                {
                    for(DateTime d = date; d <= last; date = date.AddDays(1.0))
                    {
                        if (g.GetIso8601WeekOfYear(d) == g.GetIso8601WeekOfYear(date) && sums.ContainsKey(d))
                            sum += sums[d];

                        if (g.GetIso8601WeekOfYear(d) != g.GetIso8601WeekOfYear(date) || d == last)
                        {
                            date = d;
                            string name = $"{g.GetIso8601WeekOfYear(date).ToString()}'s week";
                            AddBar(name, (double)sum / 7 / g.secondsInDay * 600, sum, sum / 7);
                            break;
                        }
                    }
                }

                else if (type == g.month)
                {
                    for (DateTime d = date; d <= last; date = date.AddDays(1.0))
                    {
                        if (d.Month == date.Month && sums.ContainsKey(d))
                            sum += sums[d];

                        if (d.Month != date.Month || d == last)
                        {
                            date = d;
                            string log = date.Month.ToString("MMM");
                            AddBar(log, (double)sum / DateTime.DaysInMonth(date.Year, date.Month) / g.secondsInDay * 600, sum, sum / DateTime.DaysInMonth(date.Year, date.Month));
                            break;
                        }
                    }
                }
            }
        }

        void AddBar(string name, double height, int sum = 0, int average = 0) 
        {
            BoxView boxView = new BoxView
            {
                Color = Color.Teal,
                HeightRequest = height,
                VerticalOptions = LayoutOptions.End
            };

            ContentView bar = new ContentView();
            bar.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    if (boxView != previousBox)
                    {
                        if (previousBox != null)
                            previousBox.Color = Color.Teal;
                        previousBox = boxView;
                        boxView.Color = Color.Silver;

                        this.name.Text = name;
                        summaryLayout.IsVisible = true;
                        summary.Text = g.SecToStr(sum);

                        if (type != g.day)
                        {
                            averageLayout.IsVisible = true;
                            this.average.Text = g.SecToStr(average);
                        }
                        else
                            averageLayout.IsVisible = false;
                    }
                })
            });
            bar.Content = boxView;

            chart.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(barSize) });
            chart.Children.Add(bar, position, 0);

            position++;
        }
    }
}
