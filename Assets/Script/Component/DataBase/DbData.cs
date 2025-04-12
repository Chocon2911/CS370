using System;
using System.Collections;
using SQLite4Unity3d;
using UnityEngine;

[Serializable]
public abstract class DbData
{
    //==========================================Variable==========================================
    [PrimaryKey] 
    public string id { get; set; }

    //========================================Constructor=========================================
    public DbData(string id)
    {
        this.id = id;
    }

    public DbData() { }
}