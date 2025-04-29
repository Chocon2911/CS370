using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDbData : DbData
{
    //==========================================Variable==========================================
    public int scene;
    public int health;

    //========================================Constructor=========================================
    public MonsterDbData(int scene, int health, string id) : base(id)
    {
        this.scene = scene;
        this.health = health;
    }

    public MonsterDbData() : base() { }
}
