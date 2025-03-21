using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemManager : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    [Header("System Manager")]
    private static SystemManager instance;

    //==========================================Get Set===========================================
    public static SystemManager Instance => instance;

    //===========================================Unity============================================
    protected override void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("One SystemManager Only", transform.gameObject);
            return;
        }

        instance = this;
    }
}
