using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
public class TeleDoor : DbObj
{
    //==========================================Variable==========================================
    [Header("Tele Door")]
    [SerializeField] protected Transform goalPoint;
    public ItemDbData Db
    {
        get
        {
            return new ItemDbData(this.id, gameObject.activeSelf ? true : false, false);
        }
    }

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.goalPoint, transform.Find("GoalPoint"), "LoadGoalPoint()");
    }

    protected override void Awake()
    {
        base.Awake();
        this.Register();
    }

    protected override void OnEnable()
    {
        this.LoadDb();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            player.transform.position = goalPoint.position;
        }
    }

    //=============================================Db=============================================
    protected virtual void LoadDb()
    {
        ItemDbData data = DataBaseManager.Instance.Item.Query(this.id);
        if (data == null)
        {
            ItemDbData newData = new ItemDbData(this.id, false, false);
            DataBaseManager.Instance.Item.Insert(newData);
            gameObject.SetActive(false);
        }
        else if (data.IsTaken)
        {
            gameObject.SetActive(false);
        }
    }

    //=======================================Register Event=======================================
    protected virtual void Register()
    {
        if (GameManager.Instance.IsFightingBoss) return;
        EventManager.Instance.OnQuit += OnQuit;
    }

    protected virtual void OnQuit()
    {
        DataBaseManager.Instance.Item.InsertUpdate(this.Db);
    }
}
