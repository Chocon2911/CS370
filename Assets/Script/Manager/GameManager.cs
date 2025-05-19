using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    private static GameManager instance;
    public static GameManager Instance => instance;

    [Header("System")]
    [SerializeField] private bool isPause;

    [Header("Player")]
    [SerializeField] private int respawnSceneIndex;
    [SerializeField] private Vector3 respawnPos;
    [SerializeField] private Quaternion respawnRot;
    [SerializeField] private string playerId;
    [SerializeField] private Player player;
    [SerializeField] private int currSceneIndex;
    [SerializeField] private Vector2 restPos;
    [SerializeField] private Quaternion restRot;

    [Header("Respawn")]
    [SerializeField] private Cooldown respawnCD;
    [SerializeField] private bool isRespawning;

    [Header("Boss")]
    [SerializeField] private bool isFightingBoss;

    //==========================================Get Set===========================================
    // Player
    public int RespawnSceneIndex => this.respawnSceneIndex;
    public Vector3 RespawnPos => this.respawnPos;
    public Quaternion RespawnRot => this.respawnRot;
    public Player Player => this.player;
    public int CurrSceneIndex => this.currSceneIndex;

    // Boss
    public bool IsFightingBoss => this.isFightingBoss;

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
        DontDestroyOnLoad(gameObject);
    }

    protected virtual void Update()
    {
        this.Pausing();
        this.Respawning();
    }

    //===========================================Other============================================
    private void Pausing()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (this.isPause)
            {
                this.isPause = false;
                Time.timeScale = 1;
                EventManager.Instance.OnMenuDisappear?.Invoke();
            }
            else
            {
                this.isPause = true;
                Time.timeScale = 0;
                EventManager.Instance.OnMenuAppear?.Invoke();
            }
        }
    }

    private void LoadSceneWithEvent(int nextScene, UnityAction onSceneLoaded)
    {
        UnityAction<Scene, LoadSceneMode> callback = null;

        callback = (scene, mode) =>
        {
            if (scene.buildIndex == nextScene)
            {
                onSceneLoaded?.Invoke();
                SceneManager.sceneLoaded -= callback;
            }
        };

        SceneManager.sceneLoaded += callback;
        SceneManager.LoadScene(nextScene);
    }

    private void GameSceneLoaded()
    {
        this.isFightingBoss = false;
        EventManager.Instance.OnBonfireResting += OnPlayerResting;
        EventManager.Instance.OnPlayerDead += OnPlayerDead;
        EventManager.Instance.OnBonfireResting += DataBaseManager.Instance.Monster.OnPlayerRest;
        EventManager.Instance.OnBonfireResting += DataBaseManager.Instance.Item.OnPlayerRest;
        EventManager.Instance.OnBossTriggered += OnBossTriggered;
        EventManager.Instance.OnBossDead += OnBossDead;
    }

    //=====================================On Boss Triggeered=====================================
    protected virtual void OnBossTriggered() 
    {
        this.isFightingBoss = true;
    }

    protected virtual void OnBossDead()
    {
        this.isFightingBoss = false;
    }

    //======================================Go Through Door=======================================
    public void GoThroughDoor(int nextScene, int nextDoor)
    {
        // Update player data in database with current player in scene
        // Then load next scene and call method after scene loaded

        this.currSceneIndex = nextScene;
        DataBaseManager.Instance.Player.Update(this.player.Db);
        this.player = null;
        EventManager.Instance.OnGoThroughDoor?.Invoke();
        LoadSceneWithEvent(nextScene, () => GoThroughDoorAfterSceneLoaded(nextDoor, nextScene));
    }

    protected void GoThroughDoorAfterSceneLoaded(int nextDoor, int nextScene)
    {
        this.GameSceneLoaded();

        // Find player in database with stored id
        // Then spawn player and call Exit() of door function

        PlayerDbData data = DataBaseManager.Instance.Player.Query(this.playerId);
        Door door = DoorManager.Instance.Doors[nextDoor];
        this.player = PlayerSpawner.Instance.SpawnPlayer(data);
        this.player.gameObject.SetActive(true);
        DoorManager.Instance.Doors[nextDoor].Exit(this.player);
        DataBaseManager.Instance.Player.Update(this.player.Db);
        Debug.Log(this.player.transform.position, transform.gameObject);
        Debug.Log("Finish going through door", transform.gameObject);
    }

    //=========================================Start Game=========================================
    public void StartGame()
    {
        if (DataBaseManager.Instance.Player.IsPlayerExist()) this.ContinueGame();
        else this.NewGame();
    }

    private void ContinueGame()
    {
        List<PlayerDbData> players = DataBaseManager.Instance.Player.QueryAll();
        PlayerDbData data = players[0];
        this.currSceneIndex = data.CurrSceneIndex;
        this.playerId = data.Id;
        this.LoadSceneWithEvent(this.currSceneIndex, () => this.ContinueGameAfterSceneLoaded(data));
    }

    private void ContinueGameAfterSceneLoaded(PlayerDbData data)
    {
        this.GameSceneLoaded();

        // Spawn Player

        Vector2 spawnPos = new Vector2(data.XPos, data.YPos);
        Quaternion spawnRot = Quaternion.Euler(data.XRot, data.YRot, data.ZRot);
        this.player = PlayerSpawner.Instance.SpawnPlayer(data);
        this.player.gameObject.SetActive(true);
    }
    
    private void NewGame()
    {
        this.respawnSceneIndex = 1;
        this.currSceneIndex = 1;
        LoadSceneWithEvent(this.currSceneIndex, () => NewGameSceneLoaded());
    }

    private void NewGameSceneLoaded()
    {
        this.GameSceneLoaded();

        // Create a new player in database
        // Then spawn the player

        this.respawnPos = Vector3.zero;
        this.respawnRot = Quaternion.Euler(0, 0, 0);
        this.player = PlayerSpawner.Instance.SpawnPlayer(this.respawnPos, this.respawnRot);
        this.playerId = this.player.Db.Id;
        this.player.gameObject.SetActive(true);
        DataBaseManager.Instance.Player.Insert(this.player.Db);
    }

    //=====================================On Player Resting======================================
    private void OnPlayerResting()
    {
        this.respawnSceneIndex = SceneManager.GetActiveScene().buildIndex;
        this.respawnPos = this.player.transform.position;
        this.respawnRot = this.player.transform.rotation;
        DataBaseManager.Instance.Player.Update(this.player.Db);

        this.LoadSceneWithEvent(this.currSceneIndex, () => StartCoroutine(this.OnResting()));
    }

    private IEnumerator OnResting()
    {
        yield return null;

        PlayerDbData data = DataBaseManager.Instance.Player.Query(this.playerId);
        this.player = PlayerSpawner.Instance.SpawnPlayer(data);
        this.player.transform.position = this.restPos;
        this.player.transform.rotation = this.restRot;
        this.player.gameObject.SetActive(true);
    }

    public void SetRestPoint(Vector2 pos, Quaternion rot)
    {
        this.respawnPos = pos;
        this.respawnRot = rot;
    }


    //=======================================On Player Dead=======================================
    private void OnPlayerDead()
    {
        this.isRespawning = true;
    }    

    private void Respawning()
    {
        if (!this.isRespawning) return;
        this.respawnCD.CoolingDown();

        if (!this.respawnCD.IsReady) return;
        this.isRespawning = false;
        this.respawnCD.ResetStatus();
        this.currSceneIndex = this.respawnSceneIndex;
        DataBaseManager.Instance.Player.Update(this.player.Db);
        this.LoadSceneWithEvent(this.currSceneIndex, () => this.RespawnAfterSceneLoaded());
    }

    private void RespawnAfterSceneLoaded()
    {
        this.GameSceneLoaded();

        PlayerDbData data = DataBaseManager.Instance.Player.Query(this.playerId);
        this.player = PlayerSpawner.Instance.SpawnPlayer(data);
        this.player.transform.position = this.respawnPos;
        this.player.transform.rotation = this.respawnRot;
        this.player.Revive();
        this.player.gameObject.SetActive(true);
    }
}
