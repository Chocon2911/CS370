using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class GameContentDb <T> : DatabaseHandler<T> where T : GameContentDbData, new()
{
    public T QueryByAccountId(string objId)
    {
        using (var connection = GetConnection())
        {
            return connection.Table<T>().FirstOrDefault(x => x.AccountId == GameManager.Instance.AccountId && x.Id == objId);
        }
    }

    public List<T> QueryAllByAccountId()
    {
        using (var connection = GetConnection())
        {
            return connection.Table<T>().Where(x => x.AccountId == GameManager.Instance.AccountId).ToList();
        }
    }

    public bool InsertUpdate(T data)
    {
        if (IsObjExist(data.Id))
        {
            Debug.Log("Update");
            return this.Update(data);
        }
        else
        {
            Debug.Log("Insert");
            return this.Insert(data);
        }
    }

    public bool IsObjExist(string id)
    {
        using (var connection = GetConnection())
        {
            return connection.Table<T>().Count(x => x.Id == id && x.AccountId == GameManager.Instance.AccountId) > 0;
        }
    }
}
