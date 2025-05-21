using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map2Manager : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    private Map2Manager instance;
    public Map2Manager Instance => instance;

    [Header("Elite Fly Dead")]
    [SerializeField] private Fly eliteFly;
    [SerializeField] private List<Transform> portals1;

    [Header("On Boss Dead")]
    [SerializeField] protected Transform bossPortal;

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
        EventManager.Instance.OnBossDead += this.OnBossDead;
    }

    protected virtual void FixedUpdate()
    {
        this.CheckingEliteFlyDead();
    }

    //=======================================Elite Fly Dead=======================================
    private void CheckingEliteFlyDead()
    {
        if (this.eliteFly.Health <= 0)
        {
            this.OpenPortal1();
        }
    }
    
    private void OpenPortal1()
    {
        foreach (Transform portal in this.portals1)
        {
            portal.gameObject.SetActive(true);
        }
    }

    //=========================================Boss Dead==========================================
    protected virtual void OnBossDead()
    {
        this.bossPortal.gameObject.SetActive(true);
    }
}
