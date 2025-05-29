using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClaspedMagicHand : HuyMonoBehaviour
{
    [Header("===Clasped Magic Hand===")]
    [SerializeField] protected Cooldown despawnCD;

    protected virtual void Update()
    {
        Util.Instance.DespawnByTime(ref this.despawnCD, transform, BulletSpawner.Instance);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        this.despawnCD.ResetStatus();
    }
}
