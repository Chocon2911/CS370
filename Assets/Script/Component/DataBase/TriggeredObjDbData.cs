using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggeredObjDbData : DbData
{
    public bool IsTriggered { get; set; }

    public TriggeredObjDbData(string id, bool isTriggered) : base(id)
    {
        this.IsTriggered = isTriggered;
    }

    public TriggeredObjDbData() : base() { }
}
