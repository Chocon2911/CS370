using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDbData : DbData
{
    //==========================================Variable==========================================
    public bool IsTaken { get; set; }
    public bool IsRestorable { get; set; }

    //========================================Constructor=========================================
    public ItemDbData(string id, bool isTaken, bool isRestorable) : base(id)
    {
        this.IsTaken = IsTaken;
        this.IsRestorable = IsRestorable;
    }

    public ItemDbData() : base() { }
}
