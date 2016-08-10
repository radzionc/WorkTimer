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
        DateTime last = DateTime.Today;
        string type;
        int position = 0;

        string Uname; int Usum; int Udays; 

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

        public void SetOnPosition()
        {
            scroll.ScrollToAsync(scroll.ContentSize.Width, 0, false);
        }

        void sizeCreation()
        {
            if (type == g.day)
                barSize = 30;
            else if (type == g.week)
                barSize = 55;
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
                    if (date == last)
                    {
                        Uname = name;
                        Usum = sum - g.TodaySum();
                        Udays = 1;
                    }
                    AddBar(name, sum, 1);
                }
                else if(type == g.week)
                {
                    for(DateTime d = date; d <= last; d = d.AddDays(1.0))
                    {
                        bool sameWeek = g.GetIso8601WeekOfYear(d) == g.GetIso8601WeekOfYear(date);
                        if (sameWeek && sums.ContainsKey(d))
                            sum += sums[d];

                        if (!sameWeek || d == last)
                        {
                            string name = $"{g.GetIso8601WeekOfYear(date).ToString()}'s week";
                            if (d == last)
                            {
                                Uname = name;
                                Usum = sum - g.TodaySum();
                                Udays = 7;
                            }
                            AddBar(name, sum, 7);
                            date = d;
                            break;
                        }
                    }
                }

                else if (type == g.month)
                {
                    for (DateTime d = date; d <= last; d = d.AddDays(1.0))
                    {
                        bool sameMonth = d.Month == date.Month;
                        if (sameMonth && sums.ContainsKey(d))
                            sum += sums[d];

                        if (!sameMonth || d == last)
                        {
                            string name = date.ToString("MMMM");
                            if (d == last)
                            {
                                Uname = name;
                                Usum = sum - g.TodaySum();
                                Udays = DateTime.DaysInMonth(date.Year, date.Month);
                            }
                            AddBar(name, sum, DateTime.DaysInMonth(date.Year, date.Month));
                            date = d;
                            break;
                        }
                    }
                }
            }
        }

        void AddBar(string name, int sum = 0, int days = 1) 
        {
            double height = (double)sum / days / g.secondsInDay * g.barMaxHeight;
            int average = (days == 1) ? 0 : (sum / days);

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

            if(Uname == null)
                position++;
        }

        public void Update()
        {
            if(Uname != null)
            {
                chart.ColumnDefinitions.RemoveAt(position);
                chart.Children.RemoveAt(position);
                AddBar(Uname, Usum + g.TodaySum(), Udays);
            }
        }
    }
}
