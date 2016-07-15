using System;
using SQLite;

namespace XamarinWorkTimer
{
    public class DatabaseTimeInterval
    {
        public DatabaseTimeInterval()
        {
            Start = DateTime.Now.ToString(gf.dateFormat);
        }

        public string Start { get; private set; }
        public string Stop { get; set; }
    }
}
