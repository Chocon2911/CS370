using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    private static SkillManager instance;
    private Dash dashSkill;
    private AirJump airJumpSkill;

    //==========================================Get Set===========================================
    public static SkillManager Instance => instance;
    public Dash DashSkill => dashSkill;
    public AirJump AirJumpSkill => airJumpSkill;

    //===========================================Unity============================================
    protected override void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("instance not null (transform)", transform.gameObject);
            Debug.LogError("Instance not null (instance)", instance.gameObject);
            return;
        }

        instance = this;
        base.Awake();
        this.dashSkill = new Dash();
        this.airJumpSkill = new AirJump();
    }
}
