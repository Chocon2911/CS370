using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AirJumpData
{
    [SerializeField] public float jumpSpeed;
    [SerializeField] public Cooldown jumpStartCD;
    [SerializeField] public bool isJumping;
    [SerializeField] public bool isJump;
}
