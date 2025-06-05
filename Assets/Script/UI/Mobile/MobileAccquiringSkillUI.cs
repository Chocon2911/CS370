using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MobileAccquiringSkillUI : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    [Header("Component")]
    [SerializeField] protected Button skipBtn;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.skipBtn, transform.Find("SkipBtn"), "LoadSkipBtn()");
    }

    protected virtual void Start()
    {
        this.CheckBtnClicked();
    }

    //===========================================Method===========================================
    protected virtual void CheckBtnClicked()
    {
        this.skipBtn.onClick.AddListener(() => 
        {
            gameObject.SetActive(false);
            Time.timeScale = 1;
        });
    }
}
