using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerMovement_2
{
    bool CanMove(PlayerMovement_2 component);
    bool CanJump(PlayerMovement_2 component);
}

public class PlayerMovement_2 : Movement
{
    //==========================================Variable==========================================
    [Header("===Player_2===")]
    [SerializeField] protected InterfaceReference<IPlayerMovement_2> user1;
    
    [Header("Move")]
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float moveSpeedUpTime;
    [SerializeField] protected float moveSlowDownTime;

    [Header("Jump")]
    [SerializeField] protected float jumpSpeed;
    [SerializeField] protected float jumpSlowDownTime;

    //==========================================Get Set===========================================
    public IPlayerMovement_2 User1 { set => user1.Value = value; }
}
