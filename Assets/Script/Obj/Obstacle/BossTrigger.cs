using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
public class BossTrigger : HuyMonoBehaviour
{
    [Header("===Boss Trigger===")]
    [SerializeField] protected CapsuleCollider2D bodyCol;
    [SerializeField] protected Boss boss;

    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.bodyCol, transform, "LoadBodyCol()");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null && !this.boss.gameObject.activeSelf && this.boss.Health > 0)
        {
            this.boss.gameObject.SetActive(true);
            EventManager.Instance.OnBossTriggered?.Invoke();
        }
    }
}
