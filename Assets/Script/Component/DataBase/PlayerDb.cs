using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerDb : DataBase
{
    //==========================================Override==========================================
    public override bool CreateTable()
    {
        using (var connection = GetConnection())
        {
            connection.CreateTable<PlayerDbData>();
            return true;
        }
    }

    public override bool RemoveRow(string id)
    {
        using (var connection = GetConnection())
        {
            return connection.Delete<PlayerDbData>(id) > 0;
        }
    }

    //===========================================Method===========================================
    public bool Insert(PlayerDbData data)
    {
        using (var connection = GetConnection())
        {
            return connection.Insert(data) > 0;
        }
    }

    public bool Update(PlayerDbData data)
    {
        using (var connection = GetConnection())
        {
            return connection.Update(data) > 0;
        }
    }

    public PlayerDbData Query(string id)
    {
        using (var connection = GetConnection())
        {
            return connection.Find<PlayerDbData>(id);
        }
    }

    public List<PlayerDbData> QueryAll()
    {
        using (var connection = GetConnection())
        {
            return connection.Table<PlayerDbData>().ToList();
        }
    }

    public bool IsPlayerExist()
    {
        using (var connection = GetConnection())
        {
            return connection.Table<PlayerDbData>().Count() > 0;
        }
    }
}
