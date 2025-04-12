using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    private static GameManager instance;
    public static GameManager Instance => instance;

    [SerializeField] private string playerId;
    [SerializeField] private Player player;
    [SerializeField] private bool isPause;

    //==========================================Get Set===========================================
    public Player Player => this.player;

    //===========================================Unity============================================
    protected override void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("instance not null (transform)", transform.gameObject);
            Debug.LogError("Instance not null (instance)", instance.gameObject);
            Destroy(gameObject);
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

    //======================================Go Through Door=======================================
    public void GoThroughDoor(int nextScene, int nextDoor)
    {
        DataBaseManager.Instance.Player.Update(this.player.PlayerDbData);
        this.player = null;
        MySceneManager.Instance.ChangeScene(nextScene);
        SceneManager.sceneLoaded += (scene, mode) => GoThroughDoorSceneLoaded(nextDoor);
    }

    protected void GoThroughDoorSceneLoaded(int nextDoor)
    {
        PlayerDbData data = DataBaseManager.Instance.Player.Query(this.playerId);
        Vector2 spawnPos = new Vector2(data.xPos, data.yPos);
        Quaternion spawnRot = Quaternion.Euler(data.xRot, data.yRot, data.zRot);
        this.player = PlayerSpawner.Instance.SpawnPlayer(data, spawnPos, spawnRot);
        DoorManager.Instance.Doors[nextDoor].Exit(this.player);
    }

    //=========================================Start Game=========================================
    public void StartGame()
    {
        if (DataBaseManager.Instance.Player.IsPlayerExist()) this.ContinueGame();
        else this.NewGame();
    }

    private void ContinueGame()
    {
        SceneManager.sceneLoaded += (scene, mode) => ContinueGameSceneLoaded();
        SceneManager.LoadScene(1);
    }

    private void ContinueGameSceneLoaded()
    {
        PlayerDbData data = DataBaseManager.Instance.Player.Query(this.playerId);
        Vector2 spawnPos = new Vector2(data.xPos, data.yPos);
        Quaternion spawnRot = Quaternion.Euler(data.xRot, data.yRot, data.zRot);
        this.player = PlayerSpawner.Instance.SpawnPlayer(data, spawnPos, spawnRot);
        this.player.gameObject.SetActive(true);
    }
    
    private void NewGame()
    {
        SceneManager.sceneLoaded += (scene, mode) => NewGameSceneLoaded();
        SceneManager.LoadScene(1);
    }

    private void NewGameSceneLoaded()
    {
        Vector2 spawnPos = new Vector2(0, 0);
        Quaternion spawnRot = Quaternion.Euler(0, 0, 0);
        this.player = PlayerSpawner.Instance.SpawnPlayer(spawnPos, spawnRot);
        this.player.gameObject.SetActive(true);
        this.playerId = "01";
        this.player.FirstBorn(this.playerId);
        DataBaseManager.Instance.Player.Insert(this.player.PlayerDbData);
    }
}
