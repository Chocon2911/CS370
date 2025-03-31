using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : Spawner
{
    //==========================================Variable==========================================
    private static BulletSpawner instance;
    public static BulletSpawner Instance => instance;

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

    public Transform Spawn(BulletType type, Vector2 spawnPos, Quaternion spawnRot)
    {
        return this.Spawn(this.prefabs[(int)type], spawnPos, spawnRot);
    }

}
