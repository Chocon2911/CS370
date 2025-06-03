using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDbData : GameContentDbData
{
    //==========================================Variable==========================================
    public int SceneIndex { get; set; }
    public MonsterType Type { get; set; }
    public int Health { get; set; }
    public int MaxHealth { get; set; }

    //========================================Constructor=========================================
    public MonsterDbData(string accountId, int sceneIndex,  MonsterType type, int health, 
        int maxHealth, string id) : base(id, accountId)
    {
        this.SceneIndex = sceneIndex;
        this.Type = type;
        this.Health = health;
        this.MaxHealth = maxHealth;
    }

    public MonsterDbData() : base() { }
}
