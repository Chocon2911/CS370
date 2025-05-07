using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Entity/Bat")]
public class BatSO : MonsterSO
{
    [Space(25)]
    [Header("===Bat===")]
    [Header("Target Detection")]
    public float DetectionRad;

    [Space(10)]

    [Header("Gore")]
    public float GoreSpeed;
    public float GoreDistance;
    public float GoreRestoreDelay;
    public float GoreChargeDelay;
    public float GoreAttackDelay;
}
