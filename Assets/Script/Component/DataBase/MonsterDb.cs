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
            return connection.Table<MonsterDbData>().FirstOrDefault(x => x.Id == id);
        }
    }

    public List<MonsterDbData> QueryAll()
    {
        using (var connection = GetConnection())
        {
            return connection.Table<MonsterDbData>().ToList();
        }
    }

    public List<MonsterDbData> QueryBySceneIndex(int sceneIndex)
    {
        using (var connection = GetConnection())
        {
            return connection.Table<MonsterDbData>().Where(x => x.SceneIndex == sceneIndex).ToList();
        }
    }

    //===========================================Other============================================
    public bool IsMonsterExist(string id)
    {
        using (var connection = GetConnection())
        {
            return connection.Table<MonsterDbData>().Count(x => x.Id == id) > 0;
        }
    }

    public void ReviveAll()
    {
        using (var connection = GetConnection())
        {
            var minions = connection.Table<MonsterDbData>().Where(x => x.Type == (int)MonsterType.MINION).ToList();
            foreach (var minion in minions)
            {
                minion.Health = minion.MaxHealth;
                connection.Update(minion);
            }
        }
    }

    public void OnPlayerRest()
    {
        this.ReviveAll();
    }
}
