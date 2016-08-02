using System;
using SQLite;

namespace XamarinWorkTimer
{
    struct Sum
    {
        [PrimaryKey]
        public string DatePK{ get; set; }
        public int Value { get; set; }
    }
}
