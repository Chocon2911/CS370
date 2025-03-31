using System.Collections;
using UnityEngine;

public interface ICastEnergyBall
{
    // Property
    Rigidbody2D GetRb();
    Cooldown GetSkillCD();
    Cooldown GetChargeCD();
    int GetDir();
    Vector2 GetBallSpawnPos();
    ref bool GetIsCharging();

    // Condition
    bool CanRechargeSkill();
    bool CanCastEnergyBall();
}

public class CastEnergyBall
{
    //===========================================Method===========================================
    public void Update(ICastEnergyBall user)
    {
        this.Shooting(user);
        this.RechargingSkill(user);
        this.ActivatingSkill(user);
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
        float angle = user.GetDir() * Mathf.Deg2Rad;
        Vector2 spawnPos = user.GetBallSpawnPos();
        Quaternion spawnRot = Quaternion.Euler(0, 0, angle * user.GetDir());

        Transform newEBall = BulletSpawner.Instance.Spawn(BulletType.ENERGY_BALL, spawnPos, spawnRot);
        newEBall.gameObject.SetActive(true);

        user.GetChargeCD().ResetStatus();
        user.GetIsCharging() = false;
    }
}
