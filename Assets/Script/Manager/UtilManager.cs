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
        if (instance != null)
        {
            Debug.LogError("instance not null (transform)", transform.gameObject);
            Debug.LogError("Instance not null (instance)", instance.gameObject);
            return;
        }

        instance = this;
        base.Awake();
    }

    //===========================================Rotate===========================================
    public void RotateFaceDir(int dir, Transform obj)
    {
        if (dir >= 0) obj.rotation = Quaternion.Euler(0, 0, 0);
        else obj.rotation = Quaternion.Euler(0, 180, 0);
    }
}
