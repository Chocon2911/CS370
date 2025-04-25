using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDbData : DbData
{
    //==========================================Variable==========================================
    public bool IsTaken { get; set; }

    //========================================Constructor=========================================
    public ItemDbData(string id, bool isTaken) : base(id)
    {
        this.IsTaken = IsTaken;
    }

    public ItemDbData() : base() { }
}
