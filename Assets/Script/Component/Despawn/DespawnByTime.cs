using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDespawnByTime
{
    // Property
    Cooldown GetDespawnCD();
    Transform GetDespawnObj();
    ref bool GetCanDespawn();
}

public class DespawnByTime
{
    //===========================================Method===========================================
    public void Despawn(IDespawnByTime user)
    {
        user.GetDespawnCD().CoolingDown();

        if (user.GetDespawnCD().IsReady)
        user.GetCanDespawn() = true;
    }
}
