using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
public class FlyUp : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    [SerializeField] private InterfaceReference<FlyUpUser> user;
    [SerializeField] private CapsuleCollider2D groundCol;
    [SerializeField] private FlyUpSO so;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.groundCol, transform, "LoadGroundCol()");
        this.LoadComponent(ref this.user, transform.parent, "LoadUser()");
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        this.Fly();
    }

    //===========================================Method===========================================
    private void Fly()
    {
        this.user.Value.GetRb().velocity = Vector2.zero;
        float xDir = Random.Range(-1.0f, 1.0f);
        Vector2 dir = new Vector2(xDir / 2, 1);
        this.user.Value.GetRb().AddForce(dir * this.so.Speed, ForceMode2D.Impulse);
    }
}
