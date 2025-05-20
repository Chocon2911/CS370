using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : HuyMonoBehaviour
{
    public override void LoadComponents()
    {
        base.LoadComponents();
        foreach (Transform obj in transform)
        {
            HuyMonoBehaviour entity = obj.GetComponent<HuyMonoBehaviour>();
            if (entity != null)
            {
                entity.LoadComponents();
                entity.gameObject.layer = LayerMask.NameToLayer("Monster");
                entity.gameObject.tag = "Monster";
            }
        }
    }

    protected void Reset()
    {
        foreach (Transform obj in transform)
        {
            Entity entity = obj.GetComponent<Entity>();
            if (entity != null)
            {
                entity.RandomId();
            }
        }
    }
}
