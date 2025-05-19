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
            }
        }
    }
}
