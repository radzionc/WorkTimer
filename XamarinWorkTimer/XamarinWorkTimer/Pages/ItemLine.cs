using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinWorkTimer.Pages
{
    public class ItemLine : StackLayout
    {
        Switch switchItem = new Switch();
        Label itemName = new Label();
        public ItemLine(Item item)
        {
            Orientation = StackOrientation.Horizontal;
            switchItem.IsEnabled = item.seconds == 0;
            itemName.Text = item.key;

            Children.Add(switchItem);
            Children.Add(itemName);

            switchItem.Toggled += (object sender, ToggledEventArgs args) =>
            {
                if (switchItem.IsToggled)
                    itemName.IsEnabled = true;
                else
                    itemName.IsEnabled = false;
            };
        }

    }
}
