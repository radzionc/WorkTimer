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
            foreach (Interval interval in intervals)
            {
                IntervalBox box = new IntervalBox(interval);

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
        }
    }
}
