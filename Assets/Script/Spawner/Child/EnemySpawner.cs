using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Spawner
{
    //==========================================Variable==========================================
    private static EnemySpawner instance;
    public static EnemySpawner Instance => instance;

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
