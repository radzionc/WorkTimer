using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamarinWorkTimer.DataBase
{
    public interface IDB<T, U> 
        where T: class //SQL element
        where U: class //Primary Key
    {
        IEnumerable<T> GetAll();
        void DeleteAll();
        T Get(U pk);
        void Delete(U pk);
        void Add(T instance);
        bool Contain(U pk);
    }
}
