using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Damagable
{
    void TakeDamage(int damage, Transform attacker);
    void Push(Vector2 force);
}
