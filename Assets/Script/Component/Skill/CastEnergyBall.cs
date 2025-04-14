using System;
using System.Collections;
using UnityEngine;

public interface ICastEnergyBall
{
    // Property
    Rigidbody2D GetRb();
    Cooldown GetRestoreCD();
    Cooldown GetChargeCD();
    Cooldown GetShootCD();
    int GetDir();
    Vector2 GetBallSpawnPos();
    ref bool GetIsCharging();
    ref bool GetIsShooting();
    ref bool GetIsCasting();

    // Condition
    bool CanCastEnergyBall();

    // Addition
    void Enter();
}

public class CastEnergyBall
{
    //===========================================Method===========================================
    public void Update(ICastEnergyBall user)
    {
        this.Finishing(user);
        this.Charging(user);
        this.RechargingSkill(user);
        this.ActivatingSkill(user);
    }

    public void Finish(ICastEnergyBall user)
    {
        user.GetIsCasting() = false;
        user.GetIsCharging() = false;
        user.GetIsShooting() = false;
        user.GetShootCD().ResetStatus();
        user.GetChargeCD().ResetStatus();
        if (user.GetRb().constraints == RigidbodyConstraints2D.FreezePositionY) user.GetRb().constraints &= ~RigidbodyConstraints2D.FreezePositionY;
        if (user.GetRb().constraints == RigidbodyConstraints2D.FreezePositionX) user.GetRb().constraints &= ~RigidbodyConstraints2D.FreezePositionX;
        user.GetRb().WakeUp();
    }

    //=======================================Recharge Skill=======================================
    private void RechargingSkill(ICastEnergyBall user)
    {
        if (user.GetIsCharging()) return;
        user.GetRestoreCD().CoolingDown();
    }

    //=======================================Activate Skill=======================================
    private void ActivatingSkill(ICastEnergyBall user)
    {
        if (!user.CanCastEnergyBall()) return;
        if (!user.GetRestoreCD().IsReady) return;
        user.Enter();
        this.ActivateSkill(user);
    }

    private void ActivateSkill(ICastEnergyBall user)
    {
        user.GetIsCasting() = true;
        user.GetIsCharging() = true;
        user.GetRestoreCD().ResetStatus();
        user.GetRb().velocity = Vector2.zero;
        user.GetRb().constraints |= RigidbodyConstraints2D.FreezePositionY;
        user.GetRb().constraints |= RigidbodyConstraints2D.FreezePositionX;
    }

    //===========================================Shoot============================================
    private void Charging(ICastEnergyBall user)
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
        user.GetIsShooting() = true;
    }

    //===========================================Finish===========================================
    private void Finishing(ICastEnergyBall user)
    {
        if (!user.GetIsShooting()) return;
        user.GetShootCD().CoolingDown();

        if (!user.GetShootCD().IsReady) return;
        user.GetIsCasting() = false;
        user.GetIsShooting() = false;
        user.GetShootCD().ResetStatus();
        user.GetRb().constraints &= ~RigidbodyConstraints2D.FreezePositionY;
        user.GetRb().constraints &= ~RigidbodyConstraints2D.FreezePositionX;
        user.GetRb().WakeUp();
    }
}
