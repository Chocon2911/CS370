using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AccountDb : DatabaseHandler
{
    //==========================================Override==========================================
    public override bool CreateTable()
    {
        using (var connection = GetConnection())
        {
            connection.CreateTable<AccountDbData>();
            return true;
        }
    }

    public override bool RemoveRow(string id)
    {
        using (var connection = GetConnection())
        {
            return connection.Delete<AccountDbData>(id) > 0;
        }
    }

    //===========================================Method===========================================
    public bool Insert(AccountDbData data)
    {
        using (var connection = GetConnection())
        {
            return connection.Insert(data) > 0;
        }
    }

    public bool Update(AccountDbData data)
    {
        using (var connection = GetConnection())
        {
            return connection.Update(data) > 0;
        }
    }

    public AccountDbData Query(string id)
    {
        using (var connection = GetConnection())
        {
            return connection.Table<AccountDbData>().FirstOrDefault(x => x.Id == id);
        }
    }

    public List<AccountDbData> QueryAll()
    {
        using (var connection = GetConnection())
        {
            return connection.Table<AccountDbData>().ToList();
        }
    }
}
