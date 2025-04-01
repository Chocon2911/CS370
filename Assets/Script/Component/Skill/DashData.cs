using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashData
{
    [SerializeField] protected Cooldown restoreCD;
    [SerializeField] protected Cooldown dashCD;
    [SerializeField] protected float speed;
    [SerializeField] protected int dir;
    [SerializeField] protected bool isDashing;
}
