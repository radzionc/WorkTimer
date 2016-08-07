using System;
using SQLite;

namespace XamarinWorkTimer
{
    public struct Item
    {
        [PrimaryKey]
        public string NamePK { get; set; }
    }
}
