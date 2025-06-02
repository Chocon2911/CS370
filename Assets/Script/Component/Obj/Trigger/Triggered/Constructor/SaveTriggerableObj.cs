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
            return new TriggeredObjDbData(this.id, this.isTriggered);
        }
        set
        {
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
        TriggeredObjDbData data = DataBaseManager.Instance.TriggeredObj.Query(this.id);
        if (data == null)
        {
            this.isTriggered = false;
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
