using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DropItem", menuName = "SO/Component/DropItem")]
public class DropItemSO : ScriptableObject
{
    public List<ItemDropRateData> ItemDropRates = new();
}
