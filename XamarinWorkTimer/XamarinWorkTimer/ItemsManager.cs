using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinWorkTimer.Pages;


namespace XamarinWorkTimer
{
    public class ItemsManager
    {
        List<Item> Items;
        StartPage startPage;
        ChoosePage choosePage;
        public ItemsManager(ref StartPage startPage, ref ChoosePage choosePage)
        {
            this.startPage = startPage;
            this.choosePage = choosePage;
            Items = (List<Item>)Application.Current.Properties["items"];
            
        }
    }

   
}
