using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartUI : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    [Header("===Start UI===")]
    [SerializeField] private Button startBtn;
    [SerializeField] private Button quitBtn;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.startBtn, transform.Find("StartBtn"), "LoadStartBtn()");
        this.LoadComponent(ref this.quitBtn, transform.Find("QuitBtn"), "LoadQuitBtn()");
    }

    private void Start()
    {
        this.startBtn.onClick.AddListener(this.StartBtnOnClick);
        this.quitBtn.onClick.AddListener(this.QuitBtnOnClick);
    }

    //===========================================Method===========================================
    private void StartBtnOnClick()
    {
        GameManager.Instance.StartGame();
    }

    private void QuitBtnOnClick()
    {
        Application.Quit();
    }
}
