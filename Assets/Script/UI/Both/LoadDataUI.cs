using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadDataUI : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    [Header("Component")]
    [SerializeField] private Button closeBtn;
    [SerializeField] private Transform container;
    [SerializeField] private Transform saveRowPrefab;
    [SerializeField] private DeleteAccountUI deleteAccountUI;
    [SerializeField] private StartUI startUI;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.closeBtn, transform.Find("CloseBtn"), "LoadCloseBtn()");
        this.LoadComponent(ref this.container, transform.Find("Scroll").Find("Container"), "LoadContainer()");
        this.LoadComponent(ref this.deleteAccountUI, transform.Find("DeleteUI"), "LoadDeleteAccountUI()");
        this.LoadComponent(ref this.startUI, transform.Find("StartUI"), "LoadStartUI()");
    }

    protected override void Awake()
    {
        base.Awake();
        this.LoadDb();
    }

    private void Start()
    {
        this.CloseBtnOnClick();
    }

    //===========================================Method===========================================
    private void LoadDb()
    {
        List<AccountDbData> accounts = DataBaseManager.Instance.Account.QueryAll();
        
        foreach (AccountDbData account in accounts)
        {
            Transform saveRow = Instantiate(this.saveRowPrefab, this.container);
            saveRow.GetComponent<SaveRowUI>().Default(account.Name, account.Id, this.deleteAccountUI);
        }
    }

    private void CloseBtnOnClick()
    {
        this.closeBtn.onClick.AddListener(() =>
        {
            this.startUI.gameObject.SetActive(true);
            gameObject.SetActive(false);
        });
    }
}
