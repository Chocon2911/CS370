using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    [SerializeField] protected CapsuleCollider2D groundCol;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected string groundTag = "Ground";
    [SerializeField] protected bool prevIsGround;
    [SerializeField] protected bool isGround;

    //==========================================Get Set===========================================
    public bool IsGround => isGround;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.groundCol, transform, "LoadGroundCol()");
    }

    public override void MyUpdate()
    {
        base.MyUpdate();
        this.CheckingGround();
    }

    //===========================================Method===========================================
    public virtual bool IsJustGround()
    {
        return this.isGround && !this.prevIsGround;
    }
    
    protected virtual void CheckingGround()
    {
        Vector2 size = this.groundCol.size;
        Vector2 pos = this.groundCol.transform.position;
        CapsuleDirection2D dir = this.groundCol.direction;
        float angle = 0;

        Collider2D[] targetCols = Physics2D.OverlapCapsuleAll(
            pos,
            size,
            dir,
            angle,
            this.groundLayer);

        foreach (Collider2D targetCol in targetCols)
        {
            if (targetCol.tag != this.groundTag) continue;
            this.prevIsGround = this.isGround;
            this.isGround = true;
            return;
        }

        this.prevIsGround = this.isGround;
        this.isGround = false;
    }
}
