using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Bullet
{
    //==========================================Variable==========================================
    [Header("===Arrow===")]
    [Header("Move")]
    [SerializeField] protected float flySpeed;
<<<<<<< HEAD
    [SerializeField] protected bool isCollided;
=======
    [SerializeField] protected bool canMove;
>>>>>>> 2f2a3976610e0664b609c5e885ef2642e65f8eb7

    //===========================================Unity============================================
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
<<<<<<< HEAD
        if (this.isCollided) return;
        base.OnTriggerEnter2D(collision);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        this.isCollided = false;
    }

=======
        if (this.canMove) return;
        base.OnTriggerEnter2D(collision);
    }

>>>>>>> 2f2a3976610e0664b609c5e885ef2642e65f8eb7
    protected override void Update()
    {
        base.Update();
        this.Moving();
    }

    //============================================Move============================================
    protected virtual void Moving()
    {
<<<<<<< HEAD
        if (this.isCollided)
        {
            this.rb.velocity = Vector2.zero;
            return;
        }

=======
        if (!this.canMove) return;
>>>>>>> 2f2a3976610e0664b609c5e885ef2642e65f8eb7
        Util.Instance.MoveForward(this.rb, this.flySpeed);
    }

    protected override void CollideWithTarget(Collider2D collision)
    {
        base.CollideWithTarget(collision);
        transform.parent = collision.transform;
<<<<<<< HEAD
        this.isCollided = true;
=======
>>>>>>> 2f2a3976610e0664b609c5e885ef2642e65f8eb7
    }
}
