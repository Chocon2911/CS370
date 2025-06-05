using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MobileUI : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    private static MobileUI instance;
    public static MobileUI Instance => instance;

    [SerializeField] protected JoyStick joyStick;
    [SerializeField] protected DefaultButton dashBtn;
    [SerializeField] protected DefaultButton jumpBtn;
    [SerializeField] protected DefaultButton attackBtn;
    [SerializeField] protected DefaultButton interactBtn;
    [SerializeField] protected Button menuBtn;
    [SerializeField] protected PauseUI pauseUI;

    //==========================================Get Set===========================================
    public Vector2 MoveDir
    {
        get
        {
            return joyStick.Direction;
        }
    }

    public int DashBtnState => this.dashBtn.State;
    public int JumpBtnState => this.jumpBtn.State;
    public int AttackBtnState => this.attackBtn.State;
    public int InteractBtnState => this.interactBtn.State;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.joyStick, transform.Find("JoyStick"), "LoadJoyStick()");
        this.LoadComponent(ref this.dashBtn, transform.Find("DashBtn"), "LoadDashBtn()");
        this.LoadComponent(ref this.jumpBtn, transform.Find("JumpBtn"), "LoadJumpBtn");
        this.LoadComponent(ref this.attackBtn, transform.Find("AttackBtn"), "LoadAttackBtn()");
        this.LoadComponent(ref this.interactBtn, transform.Find("InteractBtn"), "LoadInteractBtn()");
        this.LoadComponent(ref this.menuBtn, transform.Find("MenuBtn"), "LoadMenuBtn()");
    }

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
    }

    protected virtual void Start()
    {
        this.menuBtn.onClick.AddListener(this.MenuBtnOnClick);
    }

    protected virtual void Update()
    {
        this.CheckPlayerStat();
    }

    //===========================================Method===========================================
    protected virtual void CheckPlayerStat()
    {
        Player player = GameManager.Instance.Player;
        this.dashBtn.gameObject.SetActive(player.HasDash);
        this.interactBtn.gameObject.SetActive(player.InteractableObj != null);
    }

    protected virtual void MenuBtnOnClick()
    {
        this.pauseUI.gameObject.SetActive(true);
        Time.timeScale = 0;
    }
}
