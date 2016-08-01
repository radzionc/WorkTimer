using System;
using SQLite;

namespace XamarinWorkTimer
{
    struct Item
    {
        [PrimaryKey]
        public string Name { get; set; }
        public int Time { get; set; }
    }
}
