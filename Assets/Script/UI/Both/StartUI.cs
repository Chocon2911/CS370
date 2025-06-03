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
    [SerializeField] private List<NoSaveTriggerableObj> startTriggerableObjs;

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
        foreach (NoSaveTriggerableObj obj in this.startTriggerableObjs) obj.Trigger();
        gameObject.SetActive(false);
    }

    private void QuitBtnOnClick()
    {
        Application.Quit();
    }
}
