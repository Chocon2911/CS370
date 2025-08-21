using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DashData
{
    [SerializeField] public Cooldown restoreCD;
    [SerializeField] public Cooldown dashCD;
    [SerializeField] public float speed;
    [SerializeField] public int dir;
    [SerializeField] public bool isDashing;
}
