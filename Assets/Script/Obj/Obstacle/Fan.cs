
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Fan : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    [Header("===Fan===")]
    [Header("Component")]
    [SerializeField] protected BoxCollider2D bodyCol;

    [Header("Push")]
    [SerializeField] protected BoxCollider2D pushCol;
    [SerializeField] protected float pushForce;
    [SerializeField] protected LayerMask pushLayer;
    [SerializeField] protected List<string> pushTags;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.bodyCol, transform, "LoadBodyCol()");
        this.LoadComponent(ref this.pushCol, transform.Find("Push"), "LoadPushCol()");
    }

    protected virtual void FixedUpdate()
    {
        this.PushingOnCollide();
    }

    //============================================Push============================================
    public void PushingOnCollide()
    {
        Vector2 point = this.pushCol.transform.position;
        Vector2 size = this.pushCol.size;
        float angle = 0;

        Collider2D[] colliders = Physics2D.OverlapBoxAll(point, size, angle, this.pushLayer);
        foreach (Collider2D collider in colliders)
        {
            if (!this.pushTags.Contains(collider.tag)) continue;
            Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();

            if (rb == null) continue;
            rb.velocity = new Vector2(rb.velocity.x, this.pushForce);
        }
    }
}
