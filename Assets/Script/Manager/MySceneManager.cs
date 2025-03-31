using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    private static MySceneManager instance;
    public static MySceneManager Instance => instance;

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

    //===========================================Method===========================================
    public void ChangeScene(int sceneIndex)
    {
        EventManager.Instance.OnGoThroughDoor?.Invoke();
        SceneManager.LoadScene(sceneIndex);
    }
}
