using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Damagable
{
    void TakeDamage(int damage);
    void Push(Vector2 force);
}
