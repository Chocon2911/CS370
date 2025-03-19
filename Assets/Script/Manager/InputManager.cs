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

    [SerializeField] private KeyCode hotBar1 = KeyCode.Alpha1;
    [SerializeField] private KeyCode hotBar2 = KeyCode.Alpha2;
    [SerializeField] private KeyCode hotBar3 = KeyCode.Alpha3;
    [SerializeField] private KeyCode hotBar4 = KeyCode.Alpha4;
    [SerializeField] private KeyCode hotBar5 = KeyCode.Alpha5;
    [SerializeField] private KeyCode hotBar6 = KeyCode.Alpha6;
    [SerializeField] private KeyCode hotBar7 = KeyCode.Alpha7;
    [SerializeField] private KeyCode hotBar8 = KeyCode.Alpha8;
    [SerializeField] private KeyCode hotBar9 = KeyCode.Alpha9;

    [SerializeField] private KeyCode leftMouse = KeyCode.Mouse0;
    [SerializeField] private KeyCode rightMouse = KeyCode.Mouse1;

    [Header("Stat")]
    [SerializeField] private Vector2 moveDir;
    [SerializeField] private int leftClickState;
    [SerializeField] private int rightClickState;
    [SerializeField] private int shiftState;
    [SerializeField] private int spaceState;
    [SerializeField] private Vector2 mousePos;
    [SerializeField] private int hotBarState;

    [Header("Support")]
    [SerializeField] private Cooldown leftClickCD = new Cooldown(0f, 0);
    [SerializeField] private Cooldown rightClickCD = new Cooldown(0f, 0);
    [SerializeField] private Cooldown shiftCD = new Cooldown(0f, 0);
    [SerializeField] private Cooldown spaceCD = new Cooldown(0f, 0);

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


    public KeyCode LeftMouse => leftMouse;
    public KeyCode RightClick => rightMouse;

    //===Stat===
    public Vector2 MoveDir => moveDir;
    public int LeftClickState => leftClickState;
    public int RightClickState => rightClickState;
    public int ShiftState => shiftState;
    public int SpaceState => spaceState;
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
        this.handleInput();
    }

    //===========================================Method===========================================
    private void handleInput()
    {
        //===Reset===
        this.moveDir = Vector2.zero;
        this.leftClickState = 0;
        this.rightClickState = 0;
        this.shiftState = 0;
        this.spaceState = 0;

        //===Handle===
        //MoveDir
        if (Input.GetKeyDown(this.rightMove) || Input.GetKey(this.rightMove)) this.moveDir.x = 1;
        else if (Input.GetKeyDown(this.leftMove) || Input.GetKey(this.leftMove)) this.moveDir.x = -1;

        if (Input.GetKeyDown(this.downMove) || Input.GetKey(this.downMove)) this.moveDir.y = -1;
        else if (Input.GetKeyDown(this.topMove) || Input.GetKey(this.topMove)) this.moveDir.y = 1;

        // LeftMouse State
        if (Input.GetKey(this.leftMouse))
        {
            if (this.leftClickCD.IsReady) this.leftClickState = 2;
            else
            {
                this.leftClickState = 1;
                this.leftClickCD.CoolingDown();
            }
        }
        else if (Input.GetKeyUp(this.leftMouse))
        {
            this.leftClickCD.ResetStatus();
        }

        // RightMouse State
        if (Input.GetKey(this.rightMouse))
        {
            if (this.rightClickCD.IsReady) this.rightClickState = 2;
            else 
            { 
                this.rightClickState = 1; 
                this.rightClickCD.CoolingDown(); 
            }
        }

        else if (Input.GetKeyUp(this.rightMouse))
        {
            this.rightClickCD.ResetStatus();
        }

        // Shift State
        if (Input.GetKey(this.shift))
        {
            if (this.shiftCD.IsReady) this.shiftState = 2;
            else 
            { 
                this.shiftCD.WaitTime = Time.deltaTime; 
                this.shiftCD.CoolingDown(); 
            }
        }
        else if (Input.GetKeyUp(this.shift))
        {
            this.shiftCD.ResetStatus();
        }

        // Space State
        if (Input.GetKey(this.space))
        {
            if (this.spaceCD.IsReady) this.spaceState = 2;
            else 
            { 
                this.spaceState = 1; 
                this.spaceCD.CoolingDown(); 
            }
        }
        else if (Input.GetKeyUp(this.space))
        {
            this.spaceCD.ResetStatus();
        }

        // MousePos
        this.mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // HotBarState
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
}
