using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamarinWorkTimer
{
    public class Item
    {
        public string key { private set; get; }
        public bool choosen { get; set; }
        public int seconds { get; set; }

        public Item(string key)
        {
            this.key = key;
            choosen = false;
            seconds = 0;
        }
    }
}
