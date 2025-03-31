using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    private static GameManager instance;
    public static GameManager Instance => instance;

    private bool isPause;

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

    protected virtual void Update()
    {
        this.Pausing();
    }

    //===========================================Method===========================================
    private void Pausing()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPause)
            {
                isPause = false;
                Time.timeScale = 1;
                EventManager.Instance.OnMenuDisappear?.Invoke();
            }
            else
            {
                isPause = true;
                Time.timeScale = 0;
                EventManager.Instance.OnMenuAppear?.Invoke();
            }
        }
    }
}
