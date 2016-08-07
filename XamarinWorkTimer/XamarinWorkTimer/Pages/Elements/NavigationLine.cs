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
            //if (g.sumDB.Count() > 30)
                addLine(g.month);
            //if (g.sumDB.Count() > 7)
                addLine(g.week);
            addLine(g.today);
        }

        public void addLine(string name)
        {
            Label label = new Label
            {
                Text = name,
                
                HorizontalOptions = LayoutOptions.CenterAndExpand
                //HorizontalTextAlignment = TextAlignment.Start
            };
            if (name == g.today)
                label.TextColor = Color.Silver;
            //else
                //label.Text += " | ";

            label.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    label.TextColor = Color.Silver;
                    Clicked?.Invoke(name, EventArgs.Empty);
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
