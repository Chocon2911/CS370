using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface TeleDoorUser
{
    void SetPos(Vector2 newPos);
}

[RequireComponent(typeof(CapsuleCollider2D))]
public class TeleDoor : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    [Header("Tele Door")]
    [SerializeField] protected Transform goalPoint;
    [SerializeField] protected bool isTriggered;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.goalPoint, transform.Find("GoalPoint"), "LoadGoalPoint()");
    }

    //===========================================Method===========================================
    public void Teleport(TeleDoorUser user)
    {
        user.SetPos(this.goalPoint.position);
    }
}
