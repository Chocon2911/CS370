using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
public class TeleDoor : DbObj
{
    [Header("Tele Door")]
    [SerializeField] protected Transform goalPoint;
    public ItemDbData Db
    {
        get
        {
            return new ItemDbData(this.id, gameObject.activeSelf ? true : false);
        }
    }

    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.goalPoint, transform.Find("GoalPoint"), "LoadGoalPoint()");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            player.transform.position = goalPoint.position;
        }
    }
}
