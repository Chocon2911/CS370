using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Entity/Goblin")]
public class GoblinSO : GroundMonsterSO
{
    [Header("===Goblin===")]
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
