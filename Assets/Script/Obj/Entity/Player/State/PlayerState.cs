using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{
    //==========================================Variable==========================================
    [Space(50)]
    [Header("Player State")]
    [SerializeField] protected Player player;
    [SerializeField] private PlayerAnimator.PlayerAnimatorState animatorState;

    //========================================Constructor=========================================
    public PlayerState(Player player)
    {
        this.player = player;
    }

    //==========================================Abstract==========================================
    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}
