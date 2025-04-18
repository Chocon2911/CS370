using System;
using System.Collections.Generic;
using System.IO;
using SQLite4Unity3d;

[Serializable]
public abstract class DataBase
{
    //========================================Constructor=========================================
    public DataBase() { }

    //===========================================Method===========================================
    public SQLiteConnection GetConnection()
    {
        return new SQLiteConnection(DataBaseManager.Instance.DbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
    }

    public abstract bool CreateTable();
    public abstract bool RemoveRow(string id);
}
