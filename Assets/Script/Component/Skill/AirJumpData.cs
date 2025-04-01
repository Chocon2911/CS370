using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirJumpData
{
    [SerializeField] protected float jumpSpeed;
    [SerializeField] protected Cooldown jumpStartCD;
    [SerializeField] protected bool isJumping;
}
