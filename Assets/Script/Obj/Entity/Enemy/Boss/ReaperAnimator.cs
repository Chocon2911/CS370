using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReaperAnimator : BaseAnimator
{
    public enum ReaperState
    {
        IDLE = 0,
        MOVE = 1,
        CHARGE_SLASH = 2,
        ATTACK_SLASH = 3,
        FINISH_SLASH = 4,
        CHARGE_CAST_SPELL = 5,
        ATTACK_CAST_SPELL = 6,
        Dead = 7,
        Hurt = 8,
    }

    //==========================================Variable==========================================
    [Header("===Reaper===")]
    [SerializeField] protected Reaper reaper;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.reaper, transform.parent, "LoadReaper()");
    }

    //===========================================Method===========================================
    protected override void HandlingStat()
    {
        this.animator.SetFloat("MoveSpeed", this.reaper.ChaseSpeed / 5);
        this.animator.SetFloat("ChargeSlashSpeed", 1 / this.reaper.SlashChargeCD.TimeLimit);
        this.animator.SetFloat("AttackSlashSpeed", 1 / this.reaper.SlashAttackCD.TimeLimit);
        this.animator.SetFloat("FinishSlashSpeed", 1 / this.reaper.SlashFinishCD.TimeLimit);
        this.animator.SetFloat("HurtSpeed", 1 / this.reaper.HurtCD.TimeLimit);
    }

    protected override void HandlingState()
    {
        this.animator.SetInteger("State", (int)ReaperState.IDLE);

        if (this.reaper.IsMoving)
        {
            this.animator.SetInteger("State", (int)ReaperState.MOVE);
        }

        if (this.reaper.IsSlashing)
        {
            if (this.reaper.CurrSlashState == Reaper.SlashState.CHARGE)
                this.animator.SetInteger("State", (int)ReaperState.CHARGE_SLASH);
            else if (this.reaper.CurrSlashState == Reaper.SlashState.ATTACK)
                this.animator.SetInteger("State", (int)ReaperState.ATTACK_SLASH);
            else if (this.reaper.CurrSlashState == Reaper.SlashState.FINISH)
                this.animator.SetInteger("State", (int)ReaperState.FINISH_SLASH);
        }

        if (this.reaper.IsCastingBall || this.reaper.IsRisingHand)
        {
            if (this.reaper.IsCastingBall)
            {
                this.animator.SetFloat("ChargeCastSpellSpeed", 1 / (this.reaper.CastBallCD.TimeLimit / 2));
                this.animator.SetFloat("AttackCastSpellSpeed", 1 / (this.reaper.CastBallCD.TimeLimit / 2));
                if (this.reaper.CastBallCD.Timer <= this.reaper.CastBallCD.TimeLimit / 2)
                    this.animator.SetInteger("State", (int)ReaperState.CHARGE_CAST_SPELL);
                else
                    this.animator.SetInteger("State", (int)ReaperState.ATTACK_CAST_SPELL);
            }
            else
            {
                this.animator.SetFloat("ChargeCastSpellSpeed", 1 / this.reaper.ChargeRiseHandCD.TimeLimit);
                this.animator.SetFloat("AttackCastSpellSpeed", 1 / this.reaper.RiseHandAttackCD.TimeLimit);
                if (this.reaper.CurrRiseHandState == Reaper.RiseHandState.CHARGE)
                    this.animator.SetInteger("State", (int)ReaperState.CHARGE_CAST_SPELL);
                else if (this.reaper.CurrRiseHandState == Reaper.RiseHandState.ATTACK)
                    this.animator.SetInteger("State", (int)ReaperState.ATTACK_CAST_SPELL);
            }
        }

        if (this.reaper.IsHurting)
        {
            this.animator.SetInteger("State", (int)ReaperState.Hurt);
        }

        if (this.reaper.Health <= 0)
        {
            this.animator.SetInteger("State", (int)ReaperState.Dead);
        }
    }
}
