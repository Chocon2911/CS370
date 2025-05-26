using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Item : DbObj
{
    //==========================================Variable==========================================
    [Header("===Item===")]
    [SerializeField] protected ItemSO so;
    [SerializeField] protected CircleCollider2D bodyCol;
    [SerializeField] protected bool isRestorable;

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
        ItemDbData data = DataBaseManager.Instance.Item.Query(this.id);
        if (data == null)
        {
            ItemDbData newData = new ItemDbData(this.id, false, this.isRestorable);
            DataBaseManager.Instance.Item.Insert(newData);
        }
        else if (data.IsTaken)
        {
            gameObject.SetActive(false);
        }
        this.isRestorable = data.IsRestorable;
    }

    //===========================================Method===========================================
    public void PickedUp()
    {
        ItemDbData data = DataBaseManager.Instance.Item.Query(this.id);
        data.IsTaken = true;
    }
}