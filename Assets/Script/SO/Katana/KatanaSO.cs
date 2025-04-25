using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Katana")]
public class KatanaSO : ScriptableObject
{
    [Header("Damage Collission")]
    public int damage;
    public float pushForce;

    [Header("Attack")]
    public List<string> attackableTags;
    public LayerMask attackableLayer;
    public float restoreDelay;
    public float attackDelay;
}
