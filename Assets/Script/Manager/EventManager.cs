using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    private static EventManager instance;
    public static EventManager Instance => instance;

    public Action OnPlayerDead;
    public Action OnPlayerAppear;
    public Action OnPlayerGetNewSkill;
    public Action OnPlayerHit;
    public Action OnMenuAppear;
    public Action OnMenuDisappear;
    public Action OnGoThroughDoor;

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

        this.OnPlayerDead += PrintPlayerDead;
        this.OnPlayerAppear += PrintPlayerAppear;
    }

    //===========================================Method===========================================
    private void PrintPlayerDead()
    {
        Debug.Log("Player Dead");
    }

    private void PrintPlayerAppear()
    {
        Debug.Log("Player Appear");
    }
}
