using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    [SerializeField] private PlayerState currState;

    //===========================================Unity============================================
    public void Update()
    {
        this.currState.Update();
    }

    //===========================================Method===========================================
    public void ChangeState(PlayerState state) 
    {
        this.currState.Exit();
        this.currState = state;
        this.currState.Enter();
    }
}
