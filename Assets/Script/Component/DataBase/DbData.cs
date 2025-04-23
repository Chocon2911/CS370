using System;
using System.Collections;
using SQLite4Unity3d;
using UnityEngine;

[Serializable]
public abstract class DbData
{
    //==========================================Variable==========================================
    [PrimaryKey] public string Id { get; set; }

    //========================================Constructor=========================================
    public DbData(string id)
    {
        this.Id = id;
    }

    public DbData() { }
}