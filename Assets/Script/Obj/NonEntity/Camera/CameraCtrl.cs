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

    protected virtual void FixedUpdate()
    {
        this.Moving();
    }

    //============================================Move============================================
    protected virtual void Moving()
    {
        if (target == null) return;

        Vector3 currentPos = this.transform.position;
        Vector3 targetPos = new Vector3(target.position.x, target.position.y, currentPos.z);

        Vector3 smoothedPos = Vector3.Lerp(currentPos, targetPos, moveSpeed * Time.fixedDeltaTime);
        this.transform.position = smoothedPos;
    }

    protected virtual void OnPlayerAppear() 
    {
        this.target = GameManager.Instance.Player.transform;
    }
}