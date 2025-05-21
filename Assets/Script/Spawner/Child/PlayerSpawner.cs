using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : Spawner
{
    //==========================================Variable==========================================
    private static PlayerSpawner instance;
    public static PlayerSpawner Instance => instance;

    //===========================================Unity============================================
    protected override void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("One Instance only", transform.gameObject);
            return;
        }

        instance = this;
        base.Awake();
    }

    //===========================================Method===========================================
    public Player SpawnPlayer(PlayerDbData data)
    {
        Transform newPlayerObj = this.Spawn(this.prefabs[0], Vector3.zero, Quaternion.identity);
        Player player = newPlayerObj.GetComponent<Player>();
        player.Db = data;
        return player;
    }

    public Player SpawnPlayer(Vector2 spawnPos, Quaternion spawnRot)
    {
        Transform newPlayerObj = this.SpawnByObj(this.prefabs[0], spawnPos, spawnRot);
        Player player = newPlayerObj.GetComponentInChildren<Player>();
        player.RandomId();
        return newPlayerObj.GetComponentInChildren<Player>();
    }
}
