using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Entity/Player")]
public class PlayerSO : EntitySO
{
    [Space(25)]
    [Header("===Player===")]
    [Header("Interact Check")]
    public float interactDetectLength;

    [Space(10)]

    [Header("Move")]
    public float moveSpeed;
    public float moveSpeedUpTime;
    public float moveSlowDownTime;

    [Space(10)]

    [Header("Jump")]
    public float jumpStartDuration;
    public float jumpSpeed;

    [Space(10)]

    [Header("Dash")]
    public float dashRestoreDuration;
    public float dashDuration;
    public float dashSpeed;

    [Space(10)]

    [Header("Air Jump")]
    public float airJumpSpeed;
    public float airJumpStartDuration;

    [Space(10)]

    [Header("Cast Energy Ball")]
    public float cebRestoreDuration;
    public float cebChargeDuration;
    public float cebEndDuration;
}
