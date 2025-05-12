using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleLightning : HuyMonoBehaviour
{
    [Header("===Tele Lightning===")]
    [SerializeField] protected Animator animator;

    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.animator, transform, "LoadAnimator()");
    }

    public void CallDisappear()
    {
        this.animator.SetTrigger("Disappear");
    }

    public void CallAppear()
    {
        this.animator.SetTrigger("Appear");
    }
}
