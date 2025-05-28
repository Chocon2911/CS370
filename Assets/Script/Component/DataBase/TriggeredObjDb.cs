using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TriggeredObjDb : DatabaseHandler
{
    //==========================================Override==========================================
    public override bool CreateTable()
    {
        using (var connection = GetConnection())
        {
            connection.CreateTable<TriggeredObjDbData>();
            return true;
        }
    }

    public override bool RemoveRow(string id)
    {
        using (var connection = GetConnection())
        {
            return connection.Delete<TriggeredObjDbData>(id) > 0;
        }
    }

    //===========================================Method===========================================
    public bool Insert(TriggeredObjDbData data)
    {
        using (var connection = GetConnection())
        {
            return connection.Insert(data) > 0;
        }
    }

    public bool Update(TriggeredObjDbData data)
    {
        using (var connection = GetConnection())
        {
            return connection.Update(data) > 0;
        }
    }

    public TriggeredObjDbData Query(string id)
    {
        using (var connection = GetConnection())
        {
            return connection.Table<TriggeredObjDbData>().FirstOrDefault(x => x.Id == id);
        }
    }

    public List<TriggeredObjDbData> QueryAll()
    {
        using (var connection = GetConnection())
        {
            return connection.Table<TriggeredObjDbData>().ToList();
        }
    }

    public bool IsItemExist(string id)
    {
        using (var connection = GetConnection())
        {
            return connection.Table<TriggeredObjDbData>().Count(x => x.Id == id) > 0;
        }
    }

    public bool InsertUpdate(TriggeredObjDbData data)
    {
        if (IsItemExist(data.Id))
        {
            return this.Update(data);
        }
        else
        {
            return this.Insert(data);
        }
    }
}
