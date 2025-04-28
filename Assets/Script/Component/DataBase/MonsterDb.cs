using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterDb : DataBase
{
    //==========================================Override==========================================
    public override bool CreateTable()
    {
        using (var connection = GetConnection())
        {
            connection.CreateTable<MonsterDbData>();
            return true;
        }
    }

    public override bool RemoveRow(string id)
    {
        using (var connection = GetConnection())
        {
            return connection.Delete<MonsterDbData>(id) > 0;
        }
    }

    //===========================================Method===========================================
    public bool Insert(MonsterDbData data)
    {
        using (var connection = GetConnection())
        {
            return connection.Insert(data) > 0;
        }
    }

    public bool Update(MonsterDbData data)
    {
        using (var connection = GetConnection())
        {
            return connection.Update(data) > 0;
        }
    }

    public MonsterDbData Query(string id)
    {
        using (var connection = GetConnection())
        {
            return connection.Find<MonsterDbData>(id);
        }
    }

    public List<MonsterDbData> QueryAll()
    {
        using (var connection = GetConnection())
        {
            return connection.Table<MonsterDbData>().ToList();
        }
    }

    public bool IsPlayerExist()
    {
        using (var connection = GetConnection())
        {
            return connection.Table<MonsterDbData>().Count() > 0;
        }
    }
}
