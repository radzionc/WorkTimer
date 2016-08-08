using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace XamarinWorkTimer.Pages.Elements
{
    public class NavigationLine : StackLayout
    {
        public event EventHandler Clicked;
        public NavigationLine()
        {
            Orientation = StackOrientation.Horizontal;
            Padding = 5;
            if (g.sumDB.Count() > 30)
                addLine(g.month);
            if (g.sumDB.Count() > 14)
                addLine(g.week);
            if (g.sumDB.Count() > 2)
                addLine(g.day);
            addLine(g.today);
        }

        public void addLine(string name)
        {
            Label label = new Label
            {
                Text = name,
                
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
            if (name == g.today)
                label.TextColor = Color.Silver;

            label.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    if (label.TextColor != Color.Silver)
                    {
                        label.TextColor = Color.Silver;
                        Clicked?.Invoke(name, EventArgs.Empty);
                    }
                })
            });
            Clicked += (object sender, EventArgs args) =>
            {
                if ((string)sender != name)
                    label.TextColor = Color.Teal;
            };
            Children.Add(label);
        }
    }
}
