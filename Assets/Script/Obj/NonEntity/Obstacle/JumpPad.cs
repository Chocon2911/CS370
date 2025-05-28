using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class JumpPad : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    [Header("Component")]
    [SerializeField] protected BoxCollider2D bodyCol;
    [SerializeField] protected CapsuleCollider2D headCol;
    [SerializeField] protected Animator animator;

    [Header("Stat")]
    [SerializeField] protected float pushForce;
    [SerializeField] protected Vector2 pushDir;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.bodyCol, transform, "LoadHitCol()");
        this.LoadComponent(ref this.headCol, transform.Find("Head"), "LoadHeadCol()");
        this.LoadComponent(ref this.animator, transform.Find("Model"), "LoadModel()");
    }

    //===========================================Method===========================================
    public virtual void Push(Damagable user) 
    {
        user.Push(this.pushForce * this.pushDir);
        this.animator.SetTrigger("Push");
    }
}
