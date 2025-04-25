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

    [SerializeField] private string playerId;
    [SerializeField] private Player player;
    [SerializeField] private int currSceneIndex;
    [SerializeField] private bool isPause;

    //==========================================Get Set===========================================
    public Player Player => this.player;
    public int CurrSceneIndex => this.currSceneIndex;

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
    }

    protected virtual void Update()
    {
        this.Pausing();
    }

    //===========================================Other============================================
    private void Pausing()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !this.player.IsRest)
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

    //======================================Go Through Door=======================================
    public void GoThroughDoor(int nextScene, int nextDoor)
    {
        // Update player data in database with current player in scene
        // Then load next scene and call method after scene loaded

        this.currSceneIndex = nextScene;
        DataBaseManager.Instance.Player.Update(this.player.PlayerDbData);
        this.player = null;
        EventManager.Instance.OnGoThroughDoor?.Invoke();
        LoadSceneWithEvent(nextScene, () => GoThroughDoorAfterSceneLoaded(nextDoor, nextScene));
    }

    protected void GoThroughDoorAfterSceneLoaded(int nextDoor, int nextScene)
    {
        // Find player in database with stored id
        // Then spawn player and call Exit() of door function

        PlayerDbData data = DataBaseManager.Instance.Player.Query(this.playerId);
        Door door = DoorManager.Instance.Doors[nextDoor];
        this.player = PlayerSpawner.Instance.SpawnPlayer(data);
        this.player.gameObject.SetActive(true);
        DoorManager.Instance.Doors[nextDoor].Exit(this.player);
        DataBaseManager.Instance.Player.Update(this.player.PlayerDbData);
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
        // Spawn Player

        Vector2 spawnPos = new Vector2(data.XPos, data.YPos);
        Quaternion spawnRot = Quaternion.Euler(data.XRot, data.YRot, data.ZRot);
        this.player = PlayerSpawner.Instance.SpawnPlayer(data);
        this.player.gameObject.SetActive(true);
    }
    
    private void NewGame()
    {
        this.currSceneIndex = 1;
        LoadSceneWithEvent(1, () => NewGameSceneLoaded());
    }

    private void NewGameSceneLoaded()
    {
        // Create a new player in database
        // Then spawn the player

        Vector2 spawnPos = new Vector2(0, 0);
        Quaternion spawnRot = Quaternion.Euler(0, 0, 0);
        this.player = PlayerSpawner.Instance.SpawnPlayer(spawnPos, spawnRot);
        this.playerId = this.player.PlayerDbData.Id;
        this.player.gameObject.SetActive(true);
        DataBaseManager.Instance.Player.Insert(this.player.PlayerDbData);
    }
}
