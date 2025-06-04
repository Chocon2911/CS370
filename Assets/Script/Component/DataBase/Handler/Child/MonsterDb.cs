using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterDb : GameContentDb<MonsterDbData>
{
    public List<MonsterDbData> QueryBySceneIndex(int sceneIndex)
    {
        using (var connection = GetConnection())
        {
            return connection.Table<MonsterDbData>().Where(x => x.SceneIndex == sceneIndex && x.AccountId == GameManager.Instance.AccountId).ToList();
        }
    }

    public virtual void ReviveAll()
    {
        using (var connection = GetConnection())
        {
            var minions = connection.Table<MonsterDbData>().Where(x => x.Type == (int)MonsterType.MINION && x.AccountId == GameManager.Instance.AccountId).ToList();
            foreach (var minion in minions)
            {
                minion.Health = minion.MaxHealth;
                connection.Update(minion);
            }
        }
    }
}
