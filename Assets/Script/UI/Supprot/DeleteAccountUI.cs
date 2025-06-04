using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeleteAccountUI : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    [SerializeField] private Button acceptBtn;
    [SerializeField] private Button cancelBtn;
    [SerializeField] private string accountId;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
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
        this.acceptBtn.onClick.AddListener(() => 
        {
            DataBaseManager.Instance.Account.RemoveRowByObjId(this.accountId);
        });
    }

    private void RemoveBtnOnClick()
    {
        gameObject.SetActive(false);
    }
}
