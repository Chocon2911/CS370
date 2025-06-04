using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameContentDbData : DbData
{
    public string AccountId { get; set; }
    
    public GameContentDbData(string id, string accountId) : base(id)
    {
        AccountId = accountId;
    }

    public GameContentDbData(string dbId, string id, string accountId) : base(id, dbId) 
    {
        AccountId = accountId;
    }

    public GameContentDbData() : base() { }
}
