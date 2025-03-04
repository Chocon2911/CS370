using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDoubleJumpSkill
{
    bool CanRechargeSkill(DoubleJumpSkill component);
}

public class DoubleJumpSkill : Skill
{
    //==========================================Variable==========================================
    [Header("Double Jump")]
    [SerializeField] InterfaceReference<IDoubleJumpSkill> user1;
    [SerializeField] protected float jumpSpeed;
    [SerializeField] protected bool isJumping;
    [SerializeField] protected Cooldown skillCD;

    //==========================================Get Set===========================================
    public IDoubleJumpSkill User1 { set => this.user1.Value = value; }
}
