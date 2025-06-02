using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AccquiringSkillUI : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    [Header("Component")]
    [SerializeField] protected Animator pressAnyKeyTxtAnimator;
    [SerializeField] protected TextMeshProUGUI pressAnyKeyTxt;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.pressAnyKeyTxt, transform.Find("PressAnyKeyTxt"), "LoadPressAnyKeyTxt()");
        this.LoadComponent(ref this.pressAnyKeyTxtAnimator, transform.Find("PressAnyKeyTxt"), "LoadPressAnyKeyTxtAnimator()");
    }

    protected override void Awake()
    {
        base.Awake();
        this.pressAnyKeyTxtAnimator.SetBool("IsRunning", false);
    }

    protected virtual void Update()
    {
        this.HandlingInput();
    }

    //===========================================Method===========================================
    protected virtual void HandlingInput()
    {
        if (InputManager.Instance.InteractState == 1)
        {
            Time.timeScale = 1f;
            gameObject.SetActive(false);
        }
    }
}
