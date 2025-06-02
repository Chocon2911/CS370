using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    private static InputManager instance;

    [Header("Input")]
    //===Input===
    [SerializeField] private KeyCode leftMove = KeyCode.A;
    [SerializeField] private KeyCode rightMove = KeyCode.D;
    [SerializeField] private KeyCode topMove = KeyCode.W;
    [SerializeField] private KeyCode downMove = KeyCode.S;

    [SerializeField] private KeyCode shift = KeyCode.LeftShift;
    [SerializeField] private KeyCode space = KeyCode.Space;
    [SerializeField] private KeyCode kKey = KeyCode.K;
    [SerializeField] private KeyCode lKey = KeyCode.L;
    [SerializeField] private KeyCode jKey = KeyCode.J;
    [SerializeField] private KeyCode eKey = KeyCode.E;

    [SerializeField] private KeyCode hotBar1 = KeyCode.Alpha1;
    [SerializeField] private KeyCode hotBar2 = KeyCode.Alpha2;
    [SerializeField] private KeyCode hotBar3 = KeyCode.Alpha3;
    [SerializeField] private KeyCode hotBar4 = KeyCode.Alpha4;
    [SerializeField] private KeyCode hotBar5 = KeyCode.Alpha5;
    [SerializeField] private KeyCode hotBar6 = KeyCode.Alpha6;
    [SerializeField] private KeyCode hotBar7 = KeyCode.Alpha7;
    [SerializeField] private KeyCode hotBar8 = KeyCode.Alpha8;
    [SerializeField] private KeyCode hotBar9 = KeyCode.Alpha9;

    [Header("Stat")]
    [SerializeField] private Vector2 moveDir;
    [SerializeField] private int dashState;
    [SerializeField] private int jumpState;
    [SerializeField] private int attackState;
    [SerializeField] private int interactState;
    [SerializeField] private Vector2 mousePos;
    [SerializeField] private int hotBarState;

    [Header("Support")]
    [SerializeField] private Cooldown dashCD = new Cooldown(0f, 0);
    [SerializeField] private Cooldown jumpCD = new Cooldown(0f, 0);
    [SerializeField] private Cooldown attackCD = new Cooldown(0f, 0);
    [SerializeField] private Cooldown interactCD = new Cooldown(0f, 0);

    //==========================================Get Set===========================================
    public static InputManager Instance => instance;

    //===Input===
    public KeyCode LeftMove => leftMove;
    public KeyCode RightMove => rightMove;
    public KeyCode TopMove => topMove;
    public KeyCode DownMove => downMove;


    public KeyCode Shift => shift;
    public KeyCode Space => space;


    public KeyCode HotBar1 => hotBar1;
    public KeyCode HotBar2 => hotBar2;
    public KeyCode HotBar3 => hotBar3;
    public KeyCode HotBar4 => hotBar4;
    public KeyCode HotBar5 => hotBar5;
    public KeyCode HotBar6 => hotBar6;
    public KeyCode HotBar7 => hotBar7;
    public KeyCode HotBar8 => hotBar8;
    public KeyCode HotBar9 => hotBar9;

    //===Stat===
    public Vector2 MoveDir => moveDir;
    public int DashState => dashState;
    public int JumpState => jumpState;
    public int AttackState => attackState;
    public int InteractState => interactState;
    public Vector2 MousePos => mousePos;
    public int HotBarState => hotBarState;

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
    }

    private void Update()
    {
#if UNITY_STANDALONE || UNITY_EDITOR
        this.HandlePCInput();
#endif
#if UNITY_ANDROID || UNITY_EDITOR
        this.HandleMobileInput();
#endif
    }

    //===========================================Method===========================================
    private void HandlePCInput()
    {
        //===Reset===
        this.moveDir = Vector2.zero;
        this.dashState = 0;
        this.jumpState = 0;
        this.attackState = 0;
        this.interactState = 0;

        //===Handle===
        // MoveDir
        if (Input.GetKeyDown(this.rightMove) || Input.GetKey(this.rightMove)) this.moveDir.x = 1;
        else if (Input.GetKeyDown(this.leftMove) || Input.GetKey(this.leftMove)) this.moveDir.x = -1;

        if (Input.GetKeyDown(this.downMove) || Input.GetKey(this.downMove)) this.moveDir.y = -1;
        else if (Input.GetKeyDown(this.topMove) || Input.GetKey(this.topMove)) this.moveDir.y = 1;

        // Dash State
        List<KeyCode> dashKeyCodes = new List<KeyCode>();
        dashKeyCodes.Add(this.shift);
        dashKeyCodes.Add(this.lKey);
        this.HandleHoldState(dashKeyCodes, this.dashCD, ref this.dashState);

        // Jump State
        List<KeyCode> jumpKeyCodes = new List<KeyCode>();
        jumpKeyCodes.Add(this.space);
        jumpKeyCodes.Add(this.kKey);
        this.HandleHoldState(jumpKeyCodes, this.jumpCD, ref this.jumpState);

        // Attack State
        List<KeyCode> attackKeyCodes = new List<KeyCode>();
        attackKeyCodes.Add(this.jKey);
        this.HandleHoldState(attackKeyCodes, this.attackCD, ref this.attackState);

        // Interact State
        List<KeyCode> interactKeyCodes = new List<KeyCode>();
        interactKeyCodes.Add(this.eKey);
        this.HandleHoldState(interactKeyCodes, this.interactCD, ref this.interactState);

        // MousePos
        this.mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // HotBar State
        this.hotBarState = -1;

        if (Input.GetKey(this.hotBar1) || Input.GetKeyDown(this.hotBar1)) this.hotBarState = 1;
        else if (Input.GetKey(this.hotBar2) || Input.GetKeyDown(this.hotBar2)) this.hotBarState = 2;
        else if (Input.GetKey(this.hotBar3) || Input.GetKeyDown(this.hotBar3)) this.hotBarState = 3;
        else if (Input.GetKey(this.hotBar4) || Input.GetKeyDown(this.hotBar4)) this.hotBarState = 4;
        else if (Input.GetKey(this.hotBar5) || Input.GetKeyDown(this.hotBar5)) this.hotBarState = 5;
        else if (Input.GetKey(this.hotBar6) || Input.GetKeyDown(this.hotBar6)) this.hotBarState = 6;
        else if (Input.GetKey(this.hotBar7) || Input.GetKeyDown(this.hotBar7)) this.hotBarState = 7;
        else if (Input.GetKey(this.hotBar8) || Input.GetKeyDown(this.hotBar8)) this.hotBarState = 8;
        else if (Input.GetKey(this.hotBar9) || Input.GetKeyDown(this.hotBar9)) this.hotBarState = 9;
    }

    private void HandleHoldState(List<KeyCode> keyCodes, Cooldown stateCD, ref int state)
    {
        bool isKeyDown = false;
        bool isKeyUp = false;
        foreach (KeyCode key in keyCodes)
        {
            if (Input.GetKey(key)) isKeyDown = true;
            if (Input.GetKeyUp(key)) isKeyUp = true;
        }
        if (isKeyDown)
        {
            if (stateCD.IsReady) state = 2;
            else
            {
                state = 1;
                stateCD.CoolingDown();
            }
        }
        else if (isKeyUp)
        {
            stateCD.ResetStatus();
        }
    }

    private void HandleMobileInput() 
    {
        this.moveDir = MobileUI.Instance.MoveDir;
        this.dashState = MobileUI.Instance.DashBtnState;
        this.jumpState = MobileUI.Instance.JumpBtnState;
        this.attackState = MobileUI.Instance.AttackBtnState;
        this.interactState = MobileUI.Instance.InteractBtnState;
    }
}
