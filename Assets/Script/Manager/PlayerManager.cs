using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    private static PlayerManager instance;
    public static PlayerManager Instance => instance;

    [SerializeField] private MyPlayer player;

    //==========================================Get Set===========================================
    public MyPlayer Player => player;

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
}
