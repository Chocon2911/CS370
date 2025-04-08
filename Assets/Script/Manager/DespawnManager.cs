using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnManager : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    private static DespawnManager instance;
    public static DespawnManager Instance => instance;

    //===========================================Unity============================================
    protected override void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("instance not null (transform)", transform.gameObject);
            Debug.LogError("Instance not null (instance)", instance.gameObject);
            return;
        }

        instance = this;
        base.Awake();
    }

    //===========================================Method===========================================
    public void Despawn(Spawner spawner, Transform despawnObj)
    {
        spawner.Despawn(despawnObj);
    }

    //======================================Despawn By Time=======================================
    public bool DespawnByTime(Cooldown despawnCD)
    {
        despawnCD.CoolingDown();

        if (!despawnCD.IsReady) return false;
        return true;
    }

    public void DespawnByTime(Cooldown despawnCD, Transform despawnObj, Spawner spawner)
    {
        if (this.DespawnByTime(despawnCD)) this.Despawn(spawner, despawnObj);
    }

    //====================================Despawn By Distance=====================================
    public bool DespawnByDistance(float despawnDistance, Vector2 despawnObjPos, Vector2 targetPos)
    {
        float xDistance = Mathf.Abs(despawnObjPos.x - targetPos.x);
        float yDistance = Mathf.Abs(despawnObjPos.y - targetPos.y);
        float currDistance = Mathf.Sqrt(xDistance * xDistance + yDistance * yDistance);

        if (currDistance > despawnDistance) return true;
        return false;
    }

    public void DespawnByDistance(float despawnDistance, Vector2 despawnObjPos, Vector2 targetPos, Transform despawnObj, Spawner spawner)
    {
        if (this.DespawnByDistance(despawnDistance, despawnObjPos, targetPos)) this.Despawn(spawner, despawnObj);
    }
}
