using System;
using SQLite;

namespace XamarinWorkTimer
{
    struct Summary
    {
        [PrimaryKey]
        public string Date{ get; set; }
        public int Sum { get; set; }
    }
}
