using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DbObj : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    [Header("===Db Obj===")]
    [SerializeField] protected string dbId;
    [SerializeField] protected string id;

    //==========================================Get Set===========================================
    public string Id => id;
    public string DbId { get => dbId; set => dbId = value; }

    //===========================================Method===========================================
    public void RandomId()
    {
        this.id = System.Guid.NewGuid().ToString();
    }
}
