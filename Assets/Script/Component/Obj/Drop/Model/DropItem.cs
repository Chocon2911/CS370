using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : HuyMonoBehaviour, DropItemComponent
{
    //==========================================Variable==========================================
    [SerializeField] private DropItemSO so;

    //====================================Drop Item Component=====================================
    void DropItemComponent.Drop()
    {
        foreach (ItemDropRateData item in this.so.ItemDropRates)
        {
            if (Random.Range(0, 100000) > item.DropRate) continue;
            int random = Random.Range(0, item.AmountRates.Count);
            foreach (AmountDropRateData amount in item.AmountRates)
            {
                if (random >= amount.To || random < amount.From) continue;
                for (int i = 0; i < amount.Amount; i++)
                {
                    Transform newItem = ItemSpawner.Instance.SpawnByName(item.ItemName, transform.position, transform.rotation);
                    newItem.gameObject.SetActive(true);
                }
            }
        }
    }
}
