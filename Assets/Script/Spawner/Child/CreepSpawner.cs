using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepSpawner : Spawner
{
    //==========================================Variable==========================================
    private static CreepSpawner instance;
    public static CreepSpawner Instance => instance;

    //===========================================Unity============================================
    protected override void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("One Instance only", transform.gameObject);
            return;
        }

        instance = this;
        base.Awake();
    }
}
