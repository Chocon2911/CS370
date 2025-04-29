using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Item : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    [Header("===Item===")]
    [SerializeField] protected string id;
    [SerializeField] protected ItemSO so;
    [SerializeField] protected CircleCollider2D bodyCol;

    //==========================================Get Set===========================================
    public ItemSO SO => so;
    public string ID => id;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.bodyCol, transform, "LoadBodyCol()");
    }

    protected virtual void Reset()
    {
        this.id = Util.Instance.RandomGUID();
        this.so = null;
        this.bodyCol = null;
    }

    protected override void Awake()
    {
        base.Awake();
        ItemDbData data = DataBaseManager.Instance.Item.Query(this.id);
        if (data == null)
        {
            ItemDbData newData = new ItemDbData(this.id, false);
            DataBaseManager.Instance.Item.Insert(newData);
        }
        else if (data.IsTaken)
        {
            gameObject.SetActive(false);
        
        }
    }

    public void PickedUp()
    {
        ItemDbData data = DataBaseManager.Instance.Item.Query(this.id);
        data.IsTaken = true;
        DataBaseManager.Instance.Item.Update(data);
    }
}