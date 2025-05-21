using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseUI : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    [Header("===Pause===")]
    [SerializeField] protected Button resumeBtn;
    [SerializeField] protected Button quitBtn;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.resumeBtn, transform.Find("Resume"), "LoadResumeBtn()");
        this.LoadComponent(ref this.quitBtn, transform.Find("Quit"), "LoadQuitBtn()");
    }

    private void Start()
    {
        this.resumeBtn.onClick.AddListener(this.ResumeBtnOnClick);
        this.quitBtn.onClick.AddListener(this.QuitBtnOnClick);
    }

    //===========================================Method===========================================
    protected virtual void ResumeBtnOnClick()
    {
        Time.timeScale = 1f;
    }

    protected virtual void QuitBtnOnClick()
    {
        EventManager.Instance.OnQuit?.Invoke();
        SceneManager.LoadScene(0);
    }
}
