using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SaveTriggerableObj : TriggerableObj
{
    //==========================================Get Set===========================================
    public TriggeredObjDbData Db
    {
        get
        {
            return new TriggeredObjDbData(this.dbId, this.id, GameManager.Instance.AccountId, this.isTriggered);
        }
        set
        {
            this.dbId = value.DbId;
            this.isTriggered = value.IsTriggered;
        }
    }

    //===========================================Unity============================================
    protected override void Awake()
    {
        base.Awake();
        this.Register();
        this.LoadDb();
    }

    //===========================================Method===========================================
    protected virtual void LoadDb()
    {
        TriggeredObjDbData data = DataBaseManager.Instance.TriggeredObj.QueryByAccountId(this.id);
        if (data == null)
        {
            this.isTriggered = false;
            this.dbId = Util.Instance.RandomGUID();
            DataBaseManager.Instance.TriggeredObj.Insert(this.Db);
            this.NotTriggeredHandling();
            return;
        }

        this.Db = data;
        if (!this.isTriggered)
        {
            this.NotTriggeredHandling();
        }
        else
        {
            this.Trigger();
        }
    }

    protected virtual void Register()
    {
        EventManager.Instance.OnGoThroughDoor += this.Save;
        EventManager.Instance.OnQuit += this.Save;
        EventManager.Instance.OnBonfireResting += this.Save;
    }

    protected virtual void Save()
    {
        DataBaseManager.Instance.TriggeredObj.Update(this.Db);
    }
}
