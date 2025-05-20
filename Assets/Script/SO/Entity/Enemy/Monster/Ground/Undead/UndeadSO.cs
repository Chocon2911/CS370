using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Entity/Undead")]
public class UndeadSO : GroundMonsterSO
{
    [Space(25)]
    [Header("===Undead===")]
    [Header("Chase Target")]
    public float StopChaseDistance;
    public float ChaseSpeed;
    
    [Space(10)]
    
    [Header("Cut")]
    public float CutRadius;
    public int CutDamage;
    public float CutPushForce;
    public float CutRange;
    public float CutRestoreCD;
    public float CutChargeCD;
    public float CutAttackCD;
    public float CutFinishCD;
    public LayerMask AttackableLayer;
    public List<string> AttackableTags;
}
