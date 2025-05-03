using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDbData : DbData
{
    //==========================================Variable==========================================
    public int SceneIndex { get; set; }
    public MonsterType Type { get; set; }
    public int Health { get; set; }
    public float XPos { get; set; }
    public float YPos { get; set; }
    public float ZPos { get; set; }
    public float XRot { get; set; }
    public float YRot { get; set; }
    public float ZRot { get; set; }

    //========================================Constructor=========================================
    public MonsterDbData(int sceneIndex, MonsterType type, int health, Vector3 pos, Quaternion rot, string id) : base(id)
    {
        this.SceneIndex = sceneIndex;
        this.Type = type;
        this.Health = health;
        this.XPos = pos.x;
        this.YPos = pos.y;
        this.ZPos = pos.z;
        this.XRot = rot.eulerAngles.x;
        this.YRot = rot.eulerAngles.y;
        this.ZRot = rot.eulerAngles.z;
    }

    public MonsterDbData() : base() { }
}
