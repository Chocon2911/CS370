using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemDb : DatabaseHandler
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
            return connection.Find<ItemDbData>(id);
        }
    }

    public List<ItemDbData> QueryAll()
    {
        using (var connection = GetConnection())
        {
            return connection.Table<ItemDbData>().ToList();
        }
    }

    public bool IsItemExist(string id)
    {
        using (var connection = GetConnection())
        {
            return connection.Table<ItemDbData>().Count(x => x.Id == id) > 0;
        }
    }

    public bool InsertUpdate(ItemDbData data)
    {
        if (IsItemExist(data.Id))
        {
            Debug.Log("Wtf");
            return this.Update(data);
        }
        else
        {
            return this.Insert(data);
        }
    }

    public void RestoreAll()
    {
        using (var connection = GetConnection())
        {
            var takenItems = connection.Table<ItemDbData>().Where(x => x.IsTaken == true && x.AccountId == GameManager.Instance.AccountId).ToList();
            foreach (var takenItem in takenItems)
            {
                if (!takenItem.IsRestorable) continue;
                takenItem.IsTaken = false;
                connection.Update(takenItem);
            }
        }
    }
}
