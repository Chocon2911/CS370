using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface TeleDoorUser
{
    void SetPos(Vector2 newPos);
}

[RequireComponent(typeof(CapsuleCollider2D))]
public class TeleDoor : DbObj
{
    //==========================================Variable==========================================
    [Header("Tele Door")]
    [SerializeField] protected Transform goalPoint;
    [SerializeField] protected bool isTriggered;
    
    public TriggeredObjDbData Db
    {
        get
        {
            return new TriggeredObjDbData(this.id, this.isTriggered);
        }
        set
        {
            this.isTriggered = value.IsTriggered;
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
        this.LoadDb();
    }

    //=============================================Db=============================================
    protected virtual void LoadDb()
    {
        TriggeredObjDbData data = DataBaseManager.Instance.TriggeredObj.Query(this.id);
        if (data == null)
        {
            DataBaseManager.Instance.TriggeredObj.Insert(this.Db);
            gameObject.SetActive(false);
            return;
        }

        this.Db = data;
        if (!this.isTriggered)
        {
            gameObject.SetActive(false);
        }
    }

    //===========================================Method===========================================
    public void Teleport(TeleDoorUser user)
    {
        user.SetPos(this.goalPoint.position);
    }

    protected virtual void Register()
    {
        EventManager.Instance.OnQuit += Save;
        EventManager.Instance.OnBonfireResting += Save;
        EventManager.Instance.OnPlayerDead += this.Save;
    }

    protected virtual void Save()
    {
        DataBaseManager.Instance.TriggeredObj.InsertUpdate(this.Db);
    }
}
