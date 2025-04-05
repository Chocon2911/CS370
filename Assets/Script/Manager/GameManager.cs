using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    private static GameManager instance;
    public static GameManager Instance => instance;

    [SerializeField] private Player player;
    [SerializeField] private bool isPause;

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

    //===========================================Method===========================================
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

    public void GoThroughDoor(int nextScene, int nextDoor)
    {
        MySceneManager.Instance.ChangeScene(nextScene);
        SceneManager.sceneLoaded += (scene, mode) => DoorManager.Instance.Doors[nextDoor].Exit(this.player);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
