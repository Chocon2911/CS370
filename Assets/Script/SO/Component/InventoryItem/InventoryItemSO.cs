using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItemSO", menuName = "SO/Component/InventoryItem")]
public class InventoryItemSO : ScriptableObject
{
    public Sprite Icon;
    public string ItemName;
    public int MaxAmount;
    public InventoryItemType Type;
    public int SellPrice;
}
