using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CameraCtrl : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    [Header("===Camera===")]
    [Header("Component")]
    [SerializeField] protected Rigidbody2D rb;

    [Header("Move")]
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected Transform target;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.rb, transform, "LoadRb()");
    }

    protected override void Awake()
    {
        EventManager.Instance.OnPlayerAppear += OnPlayerAppear;
    }

    protected virtual void Update()
    {
        this.Moving();
    }

    //============================================Move============================================
    protected virtual void Moving()
    {
        if (this.target == null) return;
        this.rb.velocity = Vector2.zero;
        Util.Instance.ChaseTarget(transform, this.target, this.moveSpeed);
    }

    protected virtual void OnPlayerAppear() 
    {
        this.target = GameManager.Instance.Player.transform;
    }
}