using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamarinWorkTimer
{
    public class Test
    {
        DateTime last = DateTime.Today;
        readonly DateTime first = new DateTime(2015, 9, 27);
        public Test()
        {
            g.sumDB.DeleteAll();
            DBTest();
        }

        public void DBTest()
        {
            for (DateTime date = first; date < last; date = date.AddDays(1.0))
            {
                Random r = new Random();
                int sum = r.Next(0, 4);
                if (sum != 0)
                    sum = r.Next(1, g.secondsInDay / 2);

                g.sumDB.Add(new Sum { DatePK = date.ToString(g.dateFormat), Value = sum });
            }
        }
    }
}
