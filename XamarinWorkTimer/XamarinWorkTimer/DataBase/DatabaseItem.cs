using System;
using SQLite;

namespace XamarinWorkTimer
{
    public class DatabaseItem
    {
        public DatabaseItem()
        {
            Time = 0;
        }

        [PrimaryKey]
        public string Name { get; set; }
        public int Time { get; set; }
    }
}
