using System;
using SQLite;

namespace XamarinWorkTimer
{
    struct Interval
    {
        [PrimaryKey]
        public string Start { get; set; }
        public int Sum { get; set; }
        public string Name { get; set; }
    }
}
