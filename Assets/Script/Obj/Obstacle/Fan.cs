
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
    [SerializeField] protected Vector2 pushDir;
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
        float angle = transform.eulerAngles.z;

        Collider2D[] colliders = Physics2D.OverlapBoxAll(point, size, angle, this.pushLayer);
        foreach (Collider2D collider in colliders)
        {
            if (!this.pushTags.Contains(collider.tag)) continue;
            Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();

            if (rb == null) continue;
            float xSpeed = (this.pushDir.x <= Mathf.Pow(0.1f, 3) && this.pushDir.x >= Mathf.Pow(-0.1f, 3)) ? rb.velocity.x : this.pushDir.x * this.pushForce;
            float ySpeed = (this.pushDir.y <= Mathf.Pow(0.1f, 3) && this.pushDir.y >= Mathf.Pow(-0.1f, 3)) ? rb.velocity.y : this.pushDir.y * this.pushForce;
            rb.velocity = new Vector2(xSpeed, ySpeed);
        }
    }
}
