using System;
using System.Collections;
using SQLite4Unity3d;
using UnityEngine;

[Serializable]
public abstract class DbData
{
    //==========================================Variable==========================================
    [PrimaryKey] public string DbId { get; set; }
    public string Id { get; set; }

    //========================================Constructor=========================================
    public DbData(string id)
    {
        this.DbId = Util.Instance.RandomGUID();
        this.Id = id;
    }

    public DbData(string id, string dbId)
    {
        this.DbId = dbId;
        this.Id = id;
    }

    public DbData() { }
}