using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDbData : DbData
{
    //==========================================Variable==========================================
    public int SceneIndex { get; set; }
    public MonsterType Type { get; set; }
    public int Health { get; set; }
    public int MaxHealth { get; set; }

    //========================================Constructor=========================================
    public MonsterDbData(int sceneIndex, MonsterType type, int health, string id) : base(id)
    {
        this.SceneIndex = sceneIndex;
        this.Type = type;
        this.Health = health;
    }

    public MonsterDbData() : base() { }
}
