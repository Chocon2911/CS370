using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawner : Spawner
{
    //==========================================Variable==========================================
    private static WeaponSpawner instance;
    public static WeaponSpawner Instance => instance;

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
