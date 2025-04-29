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

    //==========================================Get Set===========================================
    public string DbPath => Application.persistentDataPath + "/" + dbName;
    public PlayerDb Player => player;
    public ItemDb Item => item;

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

        this.player = new PlayerDb();
        this.item = new ItemDb();
        this.player.CreateTable();
        this.item.CreateTable();
    }
}