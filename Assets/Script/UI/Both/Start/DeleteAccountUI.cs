using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeleteAccountUI : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    [SerializeField] private LoadDataUI loadDataUI;
    [SerializeField] private Button acceptBtn;
    [SerializeField] private Button cancelBtn;
    [SerializeField] private string accountId;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.loadDataUI, transform.parent, "LoadLoadDataUI()");
        this.LoadComponent(ref this.acceptBtn, transform.Find("Dialog").Find("AcceptBtn"), "LoadAcceptBtn()");
        this.LoadComponent(ref this.cancelBtn, transform.Find("Dialog").Find("CancelBtn"), "LoadCancelBtn()");
    }

    protected void Start()
    {
        this.acceptBtn.onClick.AddListener(() => this.AcceptBtnOnClick());
        this.cancelBtn.onClick.AddListener(() => this.RemoveBtnOnClick());
    }

    //===========================================Method===========================================
    public void Default(string accountId)
    {
        this.accountId = accountId;
    }

    private void AcceptBtnOnClick()
    {
        DataBaseManager.Instance.Account.RemoveRowByObjId(this.accountId);
        DataBaseManager.Instance.Player.RemoveAllByAccountId(this.accountId);
        DataBaseManager.Instance.Monster.RemoveAllByAccountId(this.accountId);
        DataBaseManager.Instance.Item.RemoveAllByAccountId(this.accountId);
        DataBaseManager.Instance.TriggeredObj.RemoveAllByAccountId(this.accountId);
        gameObject.SetActive(false);
        this.loadDataUI.LoadDb();
    }

    private void RemoveBtnOnClick()
    {
        gameObject.SetActive(false);
    }
}
