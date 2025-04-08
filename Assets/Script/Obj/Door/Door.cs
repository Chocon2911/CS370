using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
public class Door : HuyMonoBehaviour, Interactable
{
    //==========================================Variable==========================================
    [Header("===Door===")]
    [Header("Component")]
    [SerializeField] protected CapsuleCollider2D col;

    [Header("Enter")]
    [SerializeField] protected int nextScene;
    [SerializeField] protected int linkedDoor;

    [Header("Exit")]
    [SerializeField] protected Transform spawnPoint;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.col, transform, "LoadCol()");
        this.LoadComponent(ref this.spawnPoint, transform, "LoadSpawnPoint()");
    }

    //===========================================Method===========================================
    public void Exit(DoorUser user)
    {
        user.GetTrans().position = this.spawnPoint.position;
    }

    //========================================Interactable========================================
    void Interactable.Interact(Player player)
    {
        GameManager.Instance.GoThroughDoor(this.nextScene, this.linkedDoor);
    }
}
