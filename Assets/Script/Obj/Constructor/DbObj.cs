using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DbObj : HuyMonoBehaviour
{
    [Header("===Db Obj===")]
    [SerializeField] protected string id;
    public string Id => id;

    public void RandomId()
    {
        this.id = System.Guid.NewGuid().ToString();
    }
}
