using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    private static SkillManager instance;
    private Dash dash;
    private AirJump airJump;
    private CastEnergyBall castEBall;
    private Slash slash;

    //==========================================Get Set===========================================
    public static SkillManager Instance => instance;
    public Dash Dash => dash;
    public AirJump AirJump => airJump;
    public CastEnergyBall CastEBall => castEBall;
    public Slash Slash => slash;

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
        this.dash = new Dash();
        this.airJump = new AirJump();
        this.castEBall = new CastEnergyBall();
        this.slash = new Slash();
    }
}
