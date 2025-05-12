using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundWave : MagicBall
{
    [Header("===Variables")]
    
    [SerializeField] protected string splashName;

    protected override void CollideWithTarget(Collider2D collision)
    {
        base.CollideWithTarget(collision);
        Transform newBang = BulletSpawner.Instance.SpawnByName(this.splashName, transform.position , transform.rotation);
        newBang.gameObject.SetActive(true);
    }

}
