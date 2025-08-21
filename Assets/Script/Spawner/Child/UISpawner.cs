using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISpawner : Spawner
{
    //==========================================Variable==========================================
    private static UISpawner instance;
    public static UISpawner Instance => instance;

    //===========================================Unity============================================
    protected override void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("One UISpawner only (transform)", transform.gameObject);
            Debug.LogError("One UISpawner only (instance)", instance.gameObject);
            return;
        }

        instance = this;
        base.Awake();
    }

    //===========================================Method===========================================
    public Transform SpawnByCode(UICode code, Vector2 spawnPos, Quaternion spawnRot)
    {
        return this.SpawnByName(code.GetStr(), spawnPos, spawnRot);
    }
}
