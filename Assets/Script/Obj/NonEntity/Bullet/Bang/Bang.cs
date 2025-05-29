using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bang : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    [SerializeField] protected Cooldown despawnCD;

    //===========================================Unity============================================
    protected override void OnEnable()
    {
        base.OnEnable();
        this.despawnCD.ResetStatus();
    }

    protected virtual void Update()
    {
        this.Despawning();
    }

    //======================================Despawn By Time=======================================
    protected virtual void Despawning()
    {
        Util.Instance.DespawnByTime(ref despawnCD, transform, BulletSpawner.Instance);
    }
}
