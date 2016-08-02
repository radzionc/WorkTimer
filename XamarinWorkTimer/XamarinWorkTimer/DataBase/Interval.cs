using System;
using SQLite;

namespace XamarinWorkTimer
{
    struct Interval
    {
        [PrimaryKey]
        public string StartPK { get; set; }
        public int Sum { get; set; }
        public string Name { get; set; }
    }
}
