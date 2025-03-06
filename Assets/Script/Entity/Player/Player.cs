using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D),typeof(CapsuleCollider2D))]
public class Player : HuyMonoBehaviour, IPlayerMovement_2, IDashSkill, IAirJumpSkill
{
    //==========================================Variable==========================================
    [Header("===Player===")]
    [Header("Component")]
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected CapsuleCollider2D col;
    [SerializeField] protected Movement move;
    [SerializeField] protected Skill skill_1;
    [SerializeField] protected Skill spaceSkill;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.rb, transform, "LoadRb()");
        this.LoadComponent(ref this.col, transform, "LoadCol()");
        this.LoadComponent(ref this.move, transform.Find("Move"), "LoadMove()");
        this.LoadComponent(ref this.skill_1, transform.Find("Skill_1"), "LoadSkill_1()");
        this.LoadComponent(ref this.spaceSkill, transform.Find("SpaceSkill"), "LoadSpaceSkill()");



        // ===Move===
        // IMovement
        this.move.User = this;

        // IPlayerMovement_2
        if (this.move is PlayerMovement_2 playerMovement_2) playerMovement_2.User1 = this;



        // ===Skill_1===
        // IDashSkill
        if (this.skill_1 is DashSkill dashSkill) dashSkill.User1 = this;


        // ===SpaceSkill===
        // IAirJumpSkill
        if (this.spaceSkill is AirJumpSkill airJumpSkill) airJumpSkill.User1 = this;
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
        this.spaceSkill.MyUpdate();
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

    //=======================================IAirJumpSkill========================================
    Rigidbody2D IAirJumpSkill.GetRb(AirJumpSkill component)
    {
        if (this.spaceSkill == component)
        {
            return this.rb;
        }

        Util.Instance.IComponentErrorLog(transform, component.transform);
        return null;
    }

    bool IAirJumpSkill.CanUseSkill(AirJumpSkill component)
    {
        if (this.spaceSkill == component)
        {
            bool canUseSkill = true;

            // Movement
            if (this.move is PlayerMovement_2 playerMovement_2)
            {
                if (playerMovement_2.IsJumping) canUseSkill = false;
                else canUseSkill = true;
            }

            // Skill_1
            if (this.skill_1 is DashSkill dashSkill)
            {
                if (dashSkill.IsDashing) canUseSkill = false;
                else canUseSkill = true;
            }

            return canUseSkill;
        }

        Util.Instance.IComponentErrorLog(transform, component.transform);
        return false;
    }

    bool IAirJumpSkill.GetIsUseSkill(AirJumpSkill component)
    {
        if (this.spaceSkill == component)
        {
            return Input.GetKey(KeyCode.Space);
        }

        Util.Instance.IComponentErrorLog(transform, component.transform);
        return false;
    }
}
