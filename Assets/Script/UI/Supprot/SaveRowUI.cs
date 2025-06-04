using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveRowUI : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    [SerializeField] private Button playBtn;
    [SerializeField] private Button deleteBtn;
    [SerializeField] private TextMeshProUGUI rowName;
    [SerializeField] private DeleteAccountUI deleteAccountUI;
    [SerializeField] private string accountId;



    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.playBtn, transform.Find("PlayBtn"), "LoadPlayBtn()");
        this.LoadComponent(ref this.deleteBtn, transform.Find("DeleteBtn"), "LoadDeleteBtn()");
        this.LoadComponent(ref this.rowName, transform.Find("Name"), "LoadRowName()");
    }

    private void Start()
    {
        this.playBtn.onClick.AddListener(() => this.PlayBtnOnClick());
        this.deleteBtn.onClick.AddListener(() => this.DeleteBtnOnClick());
    }

    //===========================================Method===========================================
    public void Default(string rowName, string accountId, DeleteAccountUI deleteAccountUI)
    {
        this.rowName.text = rowName;
        this.accountId = accountId;
        this.deleteAccountUI = deleteAccountUI;
    }
    
    private void PlayBtnOnClick()
    {
        GameManager.Instance.ContinueGame(this.accountId);
    }

    private void DeleteBtnOnClick()
    {
        this.deleteAccountUI.Default(this.accountId);
        this.deleteAccountUI.gameObject.SetActive(true);
    }
}
