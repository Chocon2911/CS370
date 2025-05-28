using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
public class Door : DbObj, Interactable
{
    //==========================================Variable==========================================
    [Header("===Door===")]
    [Header("Component")]
    [SerializeField] protected CapsuleCollider2D col;
    [SerializeField] protected Transform guideArrow;

    [Header("Stat")]
    [SerializeField] protected bool isDetected;

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
        this.LoadComponent(ref this.spawnPoint, transform.Find("SpawnPoint"), "LoadSpawnPoint()");
        this.LoadComponent(ref this.guideArrow, transform.Find("Arrow"), "LoadGuideArrow()");
    }

    protected virtual void LateUpdate()
    {
        if (this.isDetected)
        {
            this.guideArrow.gameObject.SetActive(true);
        }
        else
        {
            this.guideArrow.gameObject.SetActive(false);
        }
        this.isDetected = false;
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

    void Interactable.Detected(Player player)
    {
        this.isDetected = true;
    }
}
