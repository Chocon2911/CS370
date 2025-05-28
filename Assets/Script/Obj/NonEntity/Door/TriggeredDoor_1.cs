using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggeredDoor_1 : HuyMonoBehaviour, TriggerableObj
{
    //==========================================Variable==========================================
    [SerializeField] protected Door door;
    [SerializeField] protected bool isTrigger;

    //==========================================Get Set===========================================
    public TriggeredObjDbData Db
    {
        get
        {
            return new TriggeredObjDbData(this.door.Id, this.isTrigger);
        }
        set
        {
            this.isTrigger = value.IsTriggered;
        }
    }

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.door, transform.parent, "LoadDoor()");
    }

    protected override void Awake()
    {
        base.Awake();
        TriggeredObjDbData data = DataBaseManager.Instance.TriggeredObj.Query(this.door.Id);
        if (data == null)
        {
            this.isTrigger = false;
            DataBaseManager.Instance.TriggeredObj.InsertUpdate(data);
            return;
        }

        this.Db = data;
        if (!this.isTrigger)
        {
            this.door.gameObject.SetActive(false);
        }
    }

    //===========================================Method===========================================
    void TriggerableObj.Trigger()
    {
        this.isTrigger = true;
        this.door.gameObject.SetActive(true);
    }
}
