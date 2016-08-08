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
        Today today = new Today();
        Grid main;
        public StatisticPage()
        {
            InitializeComponent();
            GridMain(today);

            navigationLine.Clicked += (object sender, EventArgs args) =>
            {
                if ((string)sender == g.today)
                    GridMain(today);
                else
                    GridMain(new Chart((string)sender));
            };
        }

        public void GridMain(Grid newGrid)
        {
            if (main != null)
                grid.Children.Remove(main);
            main = newGrid;

            grid.Children.Add(newGrid, 0, 0);
        }
    }
}
