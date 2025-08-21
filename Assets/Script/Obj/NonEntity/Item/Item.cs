using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Item : DbObj, ItemComponent
{
    //==========================================Variable==========================================
    [Header("===Item===")]
    [SerializeField] protected ItemSO so;
    [SerializeField] protected CircleCollider2D bodyCol;
    [SerializeField] protected bool isTaken;

    //==========================================Get Set===========================================
    // ===Event===
    public Action OnPickedUp { get; set; }
    
    // ===Db===
    public ItemDbData Db
    {
        get
        {
            Debug.Log("Id: " + this.id + " - isTaken: " + this.isTaken + " - isRestorable: " + this.so.IsRestorable);
            return new ItemDbData(this.dbId, GameManager.Instance.AccountId, this.id, this.isTaken, this.so.IsRestorable);
        }
        set
        {
            this.dbId = value.DbId;
            this.isTaken = value.IsTaken;
        }
    }

    //==========================================Get Set===========================================
    public ItemSO SO => so;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.bodyCol, transform, "LoadBodyCol()");
    }

    protected override void Awake()
    {
        base.Awake();
        this.Register();
        this.LoadDb();
    }

    //===========================================Method===========================================
    protected virtual void LoadDb()
    {
        ItemDbData dbData = DataBaseManager.Instance.Item.QueryByAccountId(this.id);
        if (dbData == null)
        {
            this.dbId = Util.Instance.RandomGUID();
            ItemDbData newData = new ItemDbData(this.dbId, GameManager.Instance.AccountId, this.id, this.isTaken, this.so.IsRestorable);
            DataBaseManager.Instance.Item.Insert(newData);

            return;
        }

        this.Db = dbData;
        if (this.isTaken)
        {
            gameObject.SetActive(false);
        }
    }

    protected virtual void Register()
    {
        EventManager.Instance.OnBonfireResting += this.Save;
        EventManager.Instance.OnQuit += this.Save;
        EventManager.Instance.OnPlayerDead += this.Save;
    }

    protected virtual void Save()
    {
        Debug.Log(DataBaseManager.Instance.Item.InsertUpdate(this.Db) ? "True" : "False");
    }

    //=======================================Item Component=======================================
    void ItemComponent.PickedUp(ItemUser user)
    {
        this.isTaken = true;
        user.AddHealth(this.so.RestoredHealth);
        user.UnlockSkill(this.so.UnlockedSkill);
        user.AddCoin(this.so.CoinAmount);
        this.OnPickedUp?.Invoke();
        gameObject.SetActive(false);
    }
}
