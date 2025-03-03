using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D),typeof(CapsuleCollider2D))]
public class Player : HuyMonoBehaviour, IPlayerMovement_2, IDashSkill
{
    //==========================================Variable==========================================
    [Header("===Player===")]
    [Header("Component")]
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected CapsuleCollider2D col;
    [SerializeField] protected Movement move;
    [SerializeField] protected Skill skill_1;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.rb, transform, "LoadRb()");
        this.LoadComponent(ref this.col, transform, "LoadCol()");
        this.LoadComponent(ref this.move, transform.Find("Move"), "LoadMove()");
        this.LoadComponent(ref this.skill_1, transform.Find("Skill_1"), "LoadSkill_1()");



        // ===Movement===
        // IMovement
        this.move.User = this;

        // IPlayerMovement_2
        if (this.move is PlayerMovement_2 playerMovement_2) playerMovement_2.User1 = this;



        // ===Skill===
        // IDashSkill
        if (this.skill_1 is DashSkill dashSkill) dashSkill.User1 = this;
    }

    protected virtual void FixedUpdate()
    {
        this.skill_1.MyFixedUpdate();
        this.move.MyFixedUpdate();
    }

    protected virtual void Update()
    {
        this.skill_1.MyUpdate();
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

    //=====================================IPlayerMovement_2======================================
    int IPlayerMovement_2.GetMoveDir(PlayerMovement_2 component)
    {
        if (this.move == component)
        {
            return (int)InputManager.Instance.MoveDir.x;
        }

        Util.Instance.IComponentErrorLog(transform, component.transform);
        return 0;
    }

    bool IPlayerMovement_2.GetIsJump(PlayerMovement_2 component)
    {
        if (this.move == component)
        {
            return InputManager.Instance.SpaceState != 0;
        }

        Util.Instance.IComponentErrorLog(transform, component.transform);
        return false;
    }
    
    bool IPlayerMovement_2.CanMove(PlayerMovement_2 component)
    {
        if (this.move == component)
        {
            if (this.skill_1 is DashSkill dashSkill) return dashSkill.IsDashing == false; 
            return true;
        }

        Util.Instance.IComponentErrorLog(transform, component.transform);
        return false;
    }

    bool IPlayerMovement_2.CanJump(PlayerMovement_2 component)
    {
        if (this.move == component)
        {
            if (this.skill_1 is DashSkill dashSkill) return dashSkill.IsDashing == false;
            return true;
        }

        Util.Instance.IComponentErrorLog(transform, component.transform);
        return false;
    }



    //============================================================================================
    //===========================================Skill============================================
    //============================================================================================

    //=========================================IDashSkill=========================================
    int IDashSkill.GetDashDir(DashSkill component)
    {
        if (this.skill_1 == component)
        {
            return (int)InputManager.Instance.MoveDir.x;
        }

        Util.Instance.IComponentErrorLog(transform, component.transform);
        return 0;
    }

    Rigidbody2D IDashSkill.GetRb(DashSkill component)
    {
        if (this.skill_1 == component)
        {
            return this.rb;
        }

        Util.Instance.IComponentErrorLog(transform, component.transform);
        return null;
    }

    bool IDashSkill.GetIsDash(DashSkill component)
    {
        if (this.skill_1 == component)
        {
            return Input.GetKey(KeyCode.L);
        }

        Util.Instance.IComponentErrorLog(transform, component.transform);
        return false;
    }

    bool IDashSkill.CanRechargeSkill(DashSkill component)
    {
        if (this.skill_1 == component)
        {
            return true;
        }

        Util.Instance.IComponentErrorLog(transform, component.transform);
        return false;
    }

    bool IDashSkill.CanDash(DashSkill component)
    {
        if (this.skill_1 == component)
        {
            return true;
        }

        Util.Instance.IComponentErrorLog(transform, component.transform);
        return false;
    }
}
