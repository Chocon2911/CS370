using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopItem", menuName = "SO/Component/ShopItem", order = 1)]
public class ShopItemSO : ScriptableObject
{
    public InventoryItem Item;
    public int Cost;
}
