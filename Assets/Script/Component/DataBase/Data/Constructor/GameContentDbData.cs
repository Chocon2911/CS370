using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameContentDbData : DbData
{
    public string AccountId;
    
    public GameContentDbData(string id, string accountId) : base(id)
    {
        AccountId = accountId;
    }

    public GameContentDbData() : base() { }
}
