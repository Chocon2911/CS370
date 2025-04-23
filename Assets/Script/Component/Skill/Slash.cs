using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISlash
{
    Cooldown GetRestoreCD();
    Cooldown GetAttackCD();
    ref bool GetIsAttacking();
}

public class Slash
{
    public void Restoring(ISlash user)
    {
        if (user.GetIsAttacking()) return;
        if (user.GetRestoreCD().IsReady) return;
        user.GetRestoreCD().CoolingDown();
    }

    public void TriggerAttack(ISlash user)
    {
        if (!user.GetIsAttacking()) return;
        if (!user.GetRestoreCD().IsReady) return;
        user.GetIsAttacking() = true;
        user.GetRestoreCD().ResetStatus();
    }

    public void Attacking(ISlash user)
    {
        if (!user.GetIsAttacking()) return;
        user.GetAttackCD().CoolingDown();

        if (!user.GetAttackCD().IsReady) return;
        user.GetAttackCD().ResetStatus();
        user.GetIsAttacking() = false;
    }
}
