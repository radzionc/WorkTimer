using SQLite;

namespace XamarinWorkTimer
{
    public interface ISQLite
    {
        SQLiteConnection GetConnection(string databaseName);
    }
}
