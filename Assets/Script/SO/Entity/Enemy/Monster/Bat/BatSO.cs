using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Entity/Bat")]
public class BatSO : MonsterSO
{
    [Space(25)]
    [Header("===Bat===")]

    [Space(10)]

    [Header("Chase Target")]
    public float StopChaseDistance;
    public float ChaseSpeed;

    [Space(10)]
    [Header("Fly Up")]
    public float FlyUpSpeed;
    public float FlyUpDelay;

    [Space(10)]

    [Header("Gore")]
    public float GoreSpeed;
    public float GoreDistance;
    public float GoreRestoreDelay;
    public float GoreChargeDelay;
    public float GoreAttackDelay;
}
