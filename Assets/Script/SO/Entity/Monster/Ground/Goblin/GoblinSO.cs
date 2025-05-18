using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Entity/Goblin")]
public class GoblinSO : GroundMonsterSO
{
    [Space(25)]
    [Header("===Goblin===")]
    [Header("Chase Target")]
    public float StopChaseDistance;
    public float ChaseSpeed;

    [Space(10)]

    [Header("Hit")]
    public float HitRadius;
    public int HitDamage;
    public float HitPushForce;
    public float HitRange;
    public float HitRestoreCD;
    public float HitChargeCD;
    public float HitAttackCD;
    public float HitFinishCD;
    public LayerMask AttackableLayer;
    public List<string> AttackableTags;
}
