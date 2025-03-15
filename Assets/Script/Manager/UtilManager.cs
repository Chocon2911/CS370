using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilManager : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    private static UtilManager instance;
    public static UtilManager Instance => instance;

    //===========================================Unity============================================
    protected override void Awake()
    {
        base.Awake();
        if (instance != null)
        {
            Debug.LogError("One Instance only", transform.gameObject);
            return;
        }

        instance = this;
    }

    //===========================================Rotate===========================================
    public void RotateFaceDir(int dir, Transform obj)
    {
        if (dir >= 0) obj.rotation = Quaternion.Euler(0, 0, 0);
        else obj.rotation = Quaternion.Euler(0, 180, 0);
    }
}
