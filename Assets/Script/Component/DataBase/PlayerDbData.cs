using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerDbData : DbData
{
    //==========================================Variable==========================================
    public float xPos { get; set; }
    public float yPos { get; set; }
    public float zPos { get; set; }
    public float xRot { get; set; }
    public float yRot { get; set; }
    public float zRot { get; set; }
    public int health { get; set; }
    public float dashRestoreTimer { get; set; }
    public float cebRestoreTimer { get; set; }

    //========================================Constructor=========================================
    public PlayerDbData(Vector3 position, Quaternion rotation, string id, int health, float dashRestoreTimer, float cebRestoreTimer) : base(id)
    {
        this.xPos = position.x;
        this.yPos = position.y;
        this.zPos = position.z;
        this.xRot = rotation.eulerAngles.x;
        this.yRot = rotation.eulerAngles.y;
        this.zRot = rotation.eulerAngles.z;
        this.health = health;
        this.dashRestoreTimer = dashRestoreTimer;
        this.cebRestoreTimer = cebRestoreTimer;
    }

    public PlayerDbData() : base() { }
}
