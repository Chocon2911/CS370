using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SQLite4Unity3d;

[Serializable]
public class DatabaseHandler <T> where T : DbData, new()
{
    //========================================Constructor=========================================
    public DatabaseHandler() { }

    //===========================================Method===========================================
    public SQLiteConnection GetConnection()
    {
        return new SQLiteConnection(DataBaseManager.Instance.DbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
    }

    public virtual bool CreateTable()
    {
        using (var connection = GetConnection())
        {
            connection.CreateTable<T>();
            return true;
        }
    }

    public virtual bool RemoveRow(string id)
    {
        using (var connection = GetConnection())
        {
            return connection.Delete<T>(id) > 0;
        }
    }

    public virtual bool Insert(T data)
    {
        using (var connection = GetConnection())
        {
            return connection.Insert(data) > 0;
        }
    }

    public bool Update(T data)
    {
        using (var connection = GetConnection())
        {
            return connection.Update(data) > 0;
        }
    }

    public virtual T Query(string id)
    {
        using (var connection = GetConnection())
        {
            return connection.Table<T>().FirstOrDefault(x => x.DbId == id);
        }
    }

    public virtual List<T> QueryAll()
    {
        using (var connection = GetConnection())
        {
            return connection.Table<T>().ToList();
        }
    }
}
