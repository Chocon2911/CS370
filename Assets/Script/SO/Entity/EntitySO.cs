using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySO : ScriptableObject
{
    [Header("===Entity===")]
    public int maxHealth;
    public float HurtDelay;
    public float DespawnDelay;
}
