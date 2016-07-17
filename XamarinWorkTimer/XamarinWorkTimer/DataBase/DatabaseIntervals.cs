using System;
using SQLite;

namespace XamarinWorkTimer
{
    public class DatabaseIntervals
    {
        public DatabaseIntervals()
        {
            Start = DateTime.Now.ToString(gf.dateFormat);
        }

        [PrimaryKey]
        public string Start { get; private set; }
        public string Stop { get; set; }
    }
}
