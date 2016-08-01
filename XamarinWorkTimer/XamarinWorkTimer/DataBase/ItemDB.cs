using System;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace XamarinWorkTimer.DataBase
{
    class ItemDB : DB<Item> 
    {
        public ItemDB(string name) : base(name) { }
        public int Sum()
        {
            int result = 0;
            foreach (Item item in GetAll())
                result += item.Time;

            return result;
        }

        public void Clean()
        {
            database.Query<Item>("UPDATE [Item] SET Time = 0");
        }
    }
}
