using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggeredObjDbData : GameContentDbData
{
    public bool IsTriggered { get; set; }

    public TriggeredObjDbData(string id, string accountId, bool isTriggered) : base(id, accountId)
    {
        this.IsTriggered = isTriggered;
    }

    public TriggeredObjDbData(string dbId, string id, string accountId, bool isTriggered) : base(dbId, id, accountId) 
    {
        this.IsTriggered = isTriggered;
    }

    public TriggeredObjDbData() : base() { }
}
