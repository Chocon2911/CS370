using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    private static SkillManager instance;
    private DashSkill dashSkill;
    private AirJumpSkill airJumpSkill;

    //==========================================Get Set===========================================
    public static SkillManager Instance => instance;
    public DashSkill DashSkill => dashSkill;
    public AirJumpSkill AirJumpSkill => airJumpSkill;

    //===========================================Unity============================================
    protected override void Awake()
    {
        base.Awake();
        if (instance != null)
        {
            Debug.LogError("One Instance only", transform.gameObject);
            return;
        }

        instance = this;
        this.dashSkill = new DashSkill();
        this.airJumpSkill = new AirJumpSkill();
    }
}
