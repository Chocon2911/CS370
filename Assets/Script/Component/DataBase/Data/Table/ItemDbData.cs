using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDbData : GameContentDbData
{
    //==========================================Variable==========================================
    public bool IsTaken { get; set; }
    public bool IsRestorable { get; set; }

    //========================================Constructor=========================================
    public ItemDbData(string accountId, string id, bool isTaken, bool isRestorable) : base(id, accountId)
    {
        this.IsTaken = isTaken;
        this.IsRestorable = isRestorable;
    }

    public ItemDbData(string dbId, string accountId, string id, bool isTaken, bool isRestorable) : base(dbId, id, accountId)
    {
        this.IsTaken = isTaken;
        this.IsRestorable = isRestorable;
    }

    public ItemDbData() : base() { }
}
