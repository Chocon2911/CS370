using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Entity/MushroomMan")]
public class MushroomManSO : GroundMonsterSO
{
    [Space(25)]
    [Header("===Mushroom Man===")]
    [Header("Chase Target")]
    public float StopChaseDistance;
    public float ChaseSpeed;

    [Space(10)]

    [Header("Bite")]
    public int BiteDamage;
    public float BitePushForce;
    public float BiteRange;
    public float BiteRestoreDelay;
    public float BiteChargeDelay;
    public float BiteAttackDelay;
}
