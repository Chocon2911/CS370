using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerDbData : DbData
{
    //==========================================Variable==========================================
    public int RespawnSceneIndex { get; set; }
    public float RespawnXPos { get; set; }
    public float RespawnYPos { get; set; }
    public float RespawnZPos { get; set; }
    public float RespawnXRot { get; set; }
    public float RespawnYRot { get; set; }
    public float RespawnZRot { get; set; }
    public int CurrSceneIndex { get; set; }
    public float XPos { get; set; }
    public float YPos { get; set; }
    public float ZPos { get; set; }
    public float XRot { get; set; }
    public float YRot { get; set; }
    public float ZRot { get; set; }
    public int Health { get; set; }
    public float DashRestoreTimer { get; set; }
    public float CebRestoreTimer { get; set; }
    public bool HasDash { get; set; }
    public bool HasAirJump { get; set; }
    public bool HasCastEnergyBall { get; set; }

    //========================================Constructor=========================================
    public PlayerDbData(int respawnSceneIndex, Vector3 respawnPos, Vector3 respawnRot, int currSceneIndex, Vector3 position, Quaternion rotation, string id, int health, float dashRestoreTimer,
        float cebRestoreTimer, bool hasDash, bool hasAirJump, bool hasCastEnergyBall) : base(id)
    {
        this.RespawnSceneIndex = respawnSceneIndex;
        this.RespawnXPos = respawnPos.x;
        this.RespawnYPos = respawnPos.y;
        this.RespawnZPos = respawnPos.z;
        this.RespawnXRot = respawnRot.x;
        this.RespawnYRot = respawnRot.y;
        this.RespawnZRot = respawnRot.z;
        this.CurrSceneIndex = currSceneIndex;
        this.XPos = position.x;
        this.YPos = position.y;
        this.ZPos = position.z;
        this.XRot = rotation.eulerAngles.x;
        this.YRot = rotation.eulerAngles.y;
        this.ZRot = rotation.eulerAngles.z;
        this.Health = health;
        this.DashRestoreTimer = dashRestoreTimer;
        this.CebRestoreTimer = cebRestoreTimer;
        this.HasDash = hasDash;
        this.HasAirJump = hasAirJump;
        this.HasCastEnergyBall = hasCastEnergyBall;
    }

    public PlayerDbData() : base() { }
}
