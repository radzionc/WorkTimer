using System;
using SQLite;

namespace XamarinWorkTimer
{
    public struct Sum
    {
        [PrimaryKey]
        public string DatePK{ get; set; }
        public int Value { get; set; }
    }
}
