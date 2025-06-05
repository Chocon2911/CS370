using System.Collections;
using UnityEngine;

public class DataBaseManager : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    private static DataBaseManager instance;
    public static DataBaseManager Instance => instance;

    [SerializeField] private string dbName = "Cs370.db";
    [SerializeField] private PlayerDb player;
    [SerializeField] private ItemDb item;
    [SerializeField] private MonsterDb monster;
    [SerializeField] private TriggeredObjDb triggeredObj;
    [SerializeField] private AccountDb account;

    //==========================================Get Set===========================================
    public string DbPath => Application.persistentDataPath + "/" + dbName;
    public PlayerDb Player => player;
    public ItemDb Item => item;
    public MonsterDb Monster => monster;
    public TriggeredObjDb TriggeredObj => triggeredObj;
    public AccountDb Account => this.account;

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
        DontDestroyOnLoad(gameObject);
        base.Awake();

        // handler init
        this.player = new PlayerDb();
        this.item = new ItemDb();
        this.monster = new MonsterDb();
        this.triggeredObj = new TriggeredObjDb();
        this.account = new AccountDb();

        // create table
        this.player.CreateTable();
        this.item.CreateTable();
        this.monster.CreateTable();
        this.triggeredObj.CreateTable();
        this.account.CreateTable();
    }

    public void OnSceneLoaded()
    {
        EventManager.Instance.DestroyAllDontDestroyOnLoad += DestroyGameObj;
    }

    private void DestroyGameObj()
    {
        Destroy(gameObject);
    }
}