using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CastEnergyBallData
{
    [SerializeField] public Cooldown restoreCD;
    [SerializeField] public Cooldown chargeCD;
    [SerializeField] public Cooldown endCD;
    [SerializeField] public bool isCharging;
    [SerializeField] public bool isFinishing;
}
