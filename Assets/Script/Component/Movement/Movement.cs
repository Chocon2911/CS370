using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovement
{
    Rigidbody2D GetRb(Movement component);
}

public abstract class Movement : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    [Header("===Movement===")]
    [SerializeField] protected InterfaceReference<IMovement> user;

    //==========================================Get Set===========================================
    public virtual IMovement User { set => user.Value = value; }
}
