using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SlashData
{
    public Cooldown restoreCD;
    public Cooldown attackCD;
    public int damage;
    public CircleCollider2D slashCol;
    public bool isAttacking;
}
