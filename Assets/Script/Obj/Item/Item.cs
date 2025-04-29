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
    [SerializeField] protected ItemDbData dbData;

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
        this.dbData = DataBaseManager.Instance.Item.Query(this.id);
        if (this.dbData == null)
        {
            this.dbData = new ItemDbData(this.id, false);
            DataBaseManager.Instance.Item.Insert(this.dbData);
        }
        else if (this.dbData.IsTaken)
        {
            gameObject.SetActive(false);
        }

        EventManager.Instance.OnSave += () => DataBaseManager.Instance.Item.Update(this.dbData);
    }

    public void PickedUp()
    {
        this.dbData.IsTaken = true;
    }
}
