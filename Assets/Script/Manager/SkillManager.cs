using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    private static SkillManager instance;
    private Slash slash;

    //==========================================Get Set===========================================
    public static SkillManager Instance => instance;
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
        this.slash = new Slash();
    }
}
