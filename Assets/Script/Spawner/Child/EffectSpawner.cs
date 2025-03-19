using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSpawner : Spawner
{
    public enum EffectType
    {
        DASH_EFFECT_1 = 0,
    }

    //==========================================Variable==========================================
    private static EffectSpawner instance;
    public static EffectSpawner Instance => instance;

    //===========================================Unity============================================
    protected override void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("instance not null (transform)", transform.gameObject);
            Debug.LogError("Instance not null (instance)", instance.gameObject);
            return;
        }

        instance = this;
        base.Awake();
    }

    //===========================================Method===========================================
    public Transform SpawnByType(EffectType type, Vector3 position, Quaternion rotation)
    {
        return this.Spawn(this.prefabs[(int)type], position, rotation);
    }
}
