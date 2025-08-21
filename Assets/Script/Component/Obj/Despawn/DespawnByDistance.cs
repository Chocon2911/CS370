using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDespawnByDistance
{
    float GetDespawnDistance();
    Vector2 GetDespawnObjPos();
    Vector2 GetTargetPos();
    ref bool GetCanDespawn();
}

public class DespawnByDistance
{
    public void Despawn(IDespawnByDistance user)
    {
        float xDistance = Mathf.Abs(user.GetDespawnObjPos().x - user.GetTargetPos().x);
        float yDistance = Mathf.Abs(user.GetDespawnObjPos().y - user.GetTargetPos().y);

        if (xDistance > user.GetDespawnDistance() || yDistance > user.GetDespawnDistance()) user.GetCanDespawn() = true;
    }
}
