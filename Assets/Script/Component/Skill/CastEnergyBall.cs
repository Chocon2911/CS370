using System.Collections;
using UnityEngine;

public interface ICastEnergyBall
{
    // Property
    Rigidbody2D GetRb();
    Cooldown GetSkillCD();
    Cooldown GetChargeCD();
    Cooldown GetEndCD();
    int GetDir();
    Vector2 GetBallSpawnPos();
    ref bool GetIsCharging();
    ref bool GetIsFinishing();

    // Condition
    bool CanRechargeSkill();
    bool CanCastEnergyBall();
}

public class CastEnergyBall
{
    //===========================================Method===========================================
    public void Update(ICastEnergyBall user)
    {
        this.Finishing(user);
        this.Shooting(user);
        this.RechargingSkill(user);
        this.ActivatingSkill(user);
    }

    public void Finish(ICastEnergyBall user)
    {
        user.GetIsCharging() = false;
        user.GetIsFinishing() = false;
        user.GetEndCD().ResetStatus();
        user.GetChargeCD().ResetStatus();
        if (user.GetRb().constraints == RigidbodyConstraints2D.FreezePositionY) user.GetRb().constraints &= ~RigidbodyConstraints2D.FreezePositionY;
        if (user.GetRb().constraints == RigidbodyConstraints2D.FreezePositionX) user.GetRb().constraints &= ~RigidbodyConstraints2D.FreezePositionX;
        user.GetRb().WakeUp();
    }

    //=======================================Recharge Skill=======================================
    private void RechargingSkill(ICastEnergyBall user)
    {
        if (user.GetIsCharging()) return;
        user.GetSkillCD().CoolingDown();
    }

    //=======================================Activate Skill=======================================
    private void ActivatingSkill(ICastEnergyBall user)
    {
        if (!user.CanCastEnergyBall()) return;
        if (!user.GetSkillCD().IsReady) return;
        this.ActivateSkill(user);
    }

    private void ActivateSkill(ICastEnergyBall user)
    {
        user.GetIsCharging() = true;
        user.GetSkillCD().ResetStatus();
        user.GetRb().velocity = Vector2.zero;
        user.GetRb().constraints |= RigidbodyConstraints2D.FreezePositionY;
        user.GetRb().constraints |= RigidbodyConstraints2D.FreezePositionX;
    }

    //===========================================Shoot============================================
    private void Shooting(ICastEnergyBall user)
    {
        if (!user.GetIsCharging()) return;
        user.GetChargeCD().CoolingDown();

        if (!user.GetChargeCD().IsReady) return;
        this.Shoot(user);
    }

    private void Shoot(ICastEnergyBall user)
    {
        float angle = 0;

        if (user.GetDir() == -1) angle = 180; 
        Vector2 spawnPos = user.GetBallSpawnPos();
        Quaternion spawnRot = Quaternion.Euler(0, 0, angle);

        Transform newEBall = BulletSpawner.Instance.Spawn(BulletType.ENERGY_BALL, spawnPos, spawnRot);
        newEBall.gameObject.SetActive(true);

        user.GetChargeCD().ResetStatus();
        user.GetIsCharging() = false;
        user.GetIsFinishing() = true;
    }

    //===========================================Finish===========================================
    private void Finishing(ICastEnergyBall user)
    {
        if (!user.GetIsFinishing()) return;
        user.GetEndCD().CoolingDown();

        if (!user.GetEndCD().IsReady) return;
        user.GetIsFinishing() = false;
        user.GetEndCD().ResetStatus();
        user.GetRb().constraints &= ~RigidbodyConstraints2D.FreezePositionY;
        user.GetRb().constraints &= ~RigidbodyConstraints2D.FreezePositionX;
        user.GetRb().WakeUp();
    }
}
