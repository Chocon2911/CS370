using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    [Header("Component")]
    private Dictionary<int, InventoryItem> items = new();

    //==========================================Get Set===========================================
    public Dictionary<int, InventoryItem> Items => items;
}
