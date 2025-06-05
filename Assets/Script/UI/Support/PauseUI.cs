using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    [SerializeField] private Button resumeBtn;
    [SerializeField] private Button quitBtn;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.resumeBtn, transform.Find("Container").Find("ResumeBtn"), "LoadResumeBtn()");
        this.LoadComponent(ref this.quitBtn, transform.Find("Container").Find("QuitBtn"), "LoadQuitBtn()");
    }

    private void Start()
    {
        gameObject.SetActive(false);
        this.resumeBtn.onClick.AddListener(this.ResumeBtnOnClick);
        this.quitBtn.onClick.AddListener(this.QuitBtnOnClick);
    }

    //===========================================Method===========================================
    private void ResumeBtnOnClick()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    private void QuitBtnOnClick()
    {
        EventManager.Instance.OnQuit?.Invoke();
        GameManager.Instance.ComeBackToStartScene();
        Time.timeScale = 1;
    }
}
