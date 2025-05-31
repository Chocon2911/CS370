using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitForwardVelObj : HuyMonoBehaviour
{
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected float moveSpeed;

    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.rb, transform.parent, "LoadRb()");
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        this.MoveForward();
    }

    protected virtual void MoveForward()
    {
        float angle = transform.parent.rotation.eulerAngles.z;
        float xDir = Mathf.Cos(angle * Mathf.Deg2Rad);
        float yDir = Mathf.Sin(angle * Mathf.Deg2Rad);
        Vector2 dir = new Vector2(xDir, yDir);

        this.rb.velocity = dir * this.moveSpeed;
    }
}
