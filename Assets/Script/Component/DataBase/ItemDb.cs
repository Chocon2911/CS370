using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemDb : DataBase
{
    //==========================================Override==========================================
    public override bool CreateTable()
    {
        using (var connection = GetConnection())
        {
            connection.CreateTable<ItemDbData>();
            return true;
        }
    }

    public override bool RemoveRow(string id)
    {
        using (var connection = GetConnection())
        {
            return connection.Delete<ItemDbData>(id) > 0;
        }
    }

    //===========================================Method===========================================
    public bool Insert(ItemDbData data)
    {
        using (var connection = GetConnection())
        {
            return connection.Insert(data) > 0;
        }
    }

    public bool Update(ItemDbData data)
    {
        using (var connection = GetConnection())
        {
            return connection.Update(data) > 0;
        }
    }

    public ItemDbData Query(string id)
    {
        using (var connection = GetConnection())
        {
            return connection.Table<ItemDbData>().FirstOrDefault(x => x.Id == id);
        }
    }

    public List<ItemDbData> QueryAll()
    {
        using (var connection = GetConnection())
        {
            return connection.Table<ItemDbData>().ToList();
        }
    }

    public bool IsPlayerExist()
    {
        using (var connection = GetConnection())
        {
            return connection.Table<ItemDbData>().Count() > 0;
        }
    }
}
