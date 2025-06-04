using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemDb : GameContentDb<ItemDbData>
{
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
