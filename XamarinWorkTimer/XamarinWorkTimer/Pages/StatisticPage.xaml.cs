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
        public StatisticPage(List<Interval> intervals)
        {
            InitializeComponent();
            foreach (Interval interval in intervals)
            {
                IntervalBox box = new IntervalBox(interval);
                box.GestureRecognizers.Add(new TapGestureRecognizer()
                {
                    Command = new Command(() =>
                    {
                        box.Color = Color.Green;
                        intervalLabel.Text = box.Log;
                    })
                });
                intervalLayout.Children.Add(box, box._Rectangle, AbsoluteLayoutFlags.All);
            }
            //AbsoluteLayout a = new AbsoluteLayout();
        }
    }
}
