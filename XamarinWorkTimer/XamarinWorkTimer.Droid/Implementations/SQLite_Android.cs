using System;
using XamarinWorkTimer;
using Xamarin.Forms;
using System.IO;
using SQLite;

[assembly: Dependency(typeof(XamarinWorkTimer.Droid.SQLite_Android))]

namespace XamarinWorkTimer.Droid
{
    public class SQLite_Android : ISQLite
    {
        public SQLiteConnection GetConnection(string databaseName)
        {
            var sqliteFilename = databaseName + ".db3";
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder
            var path = Path.Combine(documentsPath, sqliteFilename);
            var conn = new SQLite.SQLiteConnection(path);
            return conn;
        }
        
    }
}