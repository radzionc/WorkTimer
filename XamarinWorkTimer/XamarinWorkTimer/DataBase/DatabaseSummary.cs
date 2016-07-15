using System;
using SQLite;

namespace XamarinWorkTimer
{
    public class DatabaseSummary
    {
        public DatabaseSummary()
        {
            
        }

        [PrimaryKey]
        public string Date{ get; set; }
        public int Summary { get; set; }
    }
}
