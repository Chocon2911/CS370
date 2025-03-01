using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D),typeof(CapsuleCollider2D))]
public class Player : HuyMonoBehaviour, IPlayerMovement
{
    //==========================================Variable==========================================
    [Header("===Player===")]
    [Header("Component")]
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected CapsuleCollider2D col;
    [SerializeField] protected Movement move;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.rb, transform, "LoadRb()");
        this.LoadComponent(ref this.col, transform, "LoadCol()");
        this.LoadComponent(ref this.move, transform.Find("Move"), "LoadMove()");

        // IMovement
        this.move.User = this;

        // IPlayerMovement
        if (this.move is PlayerMovement playerMovement) playerMovement.User1 = this;
    }

    protected virtual void FixedUpdate()
    {
        this.move.MyFixedUpdate();
    }

    protected virtual void Update()
    {
        this.move.MyUpdate();
    }


    //============================================================================================
    //==========================================Movement==========================================
    //============================================================================================

    //=========================================IMovement==========================================
    Rigidbody2D IMovement.GetRb(Movement component)
    {
        if (this.move == component)
        {
            return this.rb;
        }

        Util.Instance.IComponentErrorLog(transform, component.transform);
        return null;
    }

    //======================================IPlayerMovement=======================================
    bool IPlayerMovement.CanJump(PlayerMovement component)
    {
        if (this.move == component)
        {
            return true;
        }

        Util.Instance.IComponentErrorLog(transform, component.transform);
        return false;
    }

    bool IPlayerMovement.CanMove(PlayerMovement component)
    {
        if (this.move == component)
        {
            return true;
        }

        Util.Instance.IComponentErrorLog(transform, component.transform);
        return false;
    }

    bool IPlayerMovement.CanStopMove(PlayerMovement component)
    {
        if (this.move == component)
        {
            return true;
        }

        Util.Instance.IComponentErrorLog(transform, component.transform);
        return false;
    }
}
