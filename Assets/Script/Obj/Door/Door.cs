using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    [Header("===Door===")]
    [Header("Component")]
    [SerializeField] protected ControllableByDoor ctrlObj;
    [SerializeField] protected CapsuleCollider2D col;

    [Header("Enter")]
    [SerializeField] protected string targetTag;
    [SerializeField] protected int nextScene;
    [SerializeField] protected int linkedDoor;

    [Header("Exit")]
    [SerializeField] protected Transform spawnPoint;
    [SerializeField] protected Transform stopPoint;

    //===========================================Unity============================================
    protected virtual void Update()
    {
        this.MovingPlayer();
    }
    
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(this.targetTag))
        {
            MySceneManager.Instance.ChangeScene(this.nextScene);
        }
    }

    //===========================================Method===========================================
    public virtual void PlayerExit()
    {
        this.ctrlObj = PlayerManager.Instance.Player.GetComponent<ControllableByDoor>();
    }

    protected virtual void MovingPlayer()
    {
        if (this.ctrlObj != null) return;
        
        if (this.ctrlObj.GetXPos() >= this.stopPoint.position.x + 0.1f || this.ctrlObj.GetXPos() <= this.stopPoint.position.x - 0.1f)
        {
            this.ctrlObj = null;
            return;
        }
        else
        {
            int moveDir = (this.stopPoint.position.x - this.spawnPoint.position.x) > 0 ? 1 : -1;
            this.ctrlObj.Move(moveDir);
        }
    }
}
