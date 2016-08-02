using System;
using SQLite;

namespace XamarinWorkTimer
{
    struct Item
    {
        [PrimaryKey]
        public string NamePK { get; set; }
        public int Time { get; set; }
    }
}
