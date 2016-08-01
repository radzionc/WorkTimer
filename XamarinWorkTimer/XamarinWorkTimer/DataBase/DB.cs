using System;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace XamarinWorkTimer.DataBase
{
    public class DB<T> where T : struct
    {
        protected SQLiteConnection database;
        public DB(string name)
        {
            database = DependencyService.Get<ISQLite>().GetConnection(name);
            database.CreateTable<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return (from i in database.Table<T>() select i).ToList();
        }

        public T Get(string pk)
        {
            return database.Find<T>(pk);
        }

        public void DeleteAll()
        {
            database.DeleteAll<T>();
        }

        public void Delete(string pk)
        {
            database.Delete<T>(pk);
        }

        public bool Contain(string pk)
        {
            T e = Get(pk);
            return Get(pk).Equals(null)? false : true;
        }

        public void Add(T instance)
        {
            database.Insert(instance);
        }
        
        public void Update(T instance)
        {
            database.Update(instance);
        }
    }
}
