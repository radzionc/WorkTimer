using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace XamarinWorkTimer.Pages.Elements
{
    public class ChooseLine : StackLayout
    {
        public string Name { get; private set; }

        public event EventHandler LineClicked;
        public event EventHandler XClicked;
        public ChooseLine(string name)
        {
            Orientation = StackOrientation.Horizontal;
            Padding = new Thickness(5, 0, 5, 0);
            Name = name;
            Button x = new Button
            {
                Text = '\u2612'.ToString(),
                TextColor = Color.Red,
                FontSize = 36,
                BackgroundColor = Color.Transparent,
                BorderColor = Color.Transparent,
                HorizontalOptions = LayoutOptions.End
            };
            x.Clicked += (object sender, EventArgs args) => { XClicked?.Invoke(this, args); };
            Children.Add(new Label
            {
                Text = Name,
                HorizontalOptions = LayoutOptions.StartAndExpand
            });
            Children.Add(x);
            GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    LineClicked?.Invoke(Name, EventArgs.Empty);
                })
            });
        }
    }
}
