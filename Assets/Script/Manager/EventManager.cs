using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    [Header("===Event Manager===")]
    private static EventManager instance;
    public static EventManager Instance => instance;

    //===Player===
    public Action OnPlayerDead { get; set; }
    public Action OnPlayerAppear { get; set; }
    public Action OnPlayerGetNewSkill { get; set; }
    public Action OnPlayerHit { get; set; }

    //===Game===
    public Action OnMenuAppear { get; set; }
    public Action OnMenuDisappear { get; set; }
    public Action OnGoThroughDoor { get; set; }
    public Action OnBonfireResting { get; set; }
    public Action OnBonfireStopResting { get; set; }
    public Action OnQuit { get; set; }
    public Action OnBossTriggered { get; set; }
    public Action OnBossDead { get; set; }

    //===Skill===
    public Action OnPlayerGetDash { get; set; }
    public Action OnPlayerGetAirJump { get; set; }
    public Action OnPlayergetCastEnergyBall { get; set; }

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
        Debug.Log("Player Dead", gameObject);
    }

    private void PrintPlayerAppear()
    {
        Debug.Log("Player Appear", gameObject);
        //Debug.Log(GameManager.Instance.Player.transform.position, gameObject);
    }
}
