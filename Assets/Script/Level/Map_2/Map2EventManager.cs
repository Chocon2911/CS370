using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map2EventManager : EventManager
{
    //==========================================Variable==========================================
    private static Map2EventManager instance1;
    public static Map2EventManager Instance1 => instance1;

    //===========================================Unity============================================
    protected override void Awake()
    {
        if (instance1 != null)
        {
            Debug.LogError("instance not null (transform)", transform.gameObject);
            Debug.LogError("Instance not null (instance)", instance1.gameObject);
            return;
        }

        instance1 = this;
        base.Awake();
    }
}
