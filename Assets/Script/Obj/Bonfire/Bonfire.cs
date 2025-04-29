using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BonfireUser
{
    Vector2 GetPos();
    void Teleport(Vector2 pos);
    void Rest();
}

[RequireComponent(typeof(CircleCollider2D))]
public class Bonfire : HuyMonoBehaviour, Interactable
{
    //==========================================Variable==========================================
    [SerializeField] protected List<Transform> restPoints;
    [SerializeField] protected BonfireUser tempUser;
    [SerializeField] protected Transform chosentRestPoint;
    [SerializeField] protected bool isResting;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.restPoints, transform.Find("RestPoints"), "LoadRestPoints()");
    }

    protected override void Awake()
    {
        base.Awake();
        EventManager.Instance.OnBonfireStopResting += StopResting;
    }

    //===========================================Method===========================================
    protected virtual void StopResting()
    {
        this.isResting = false;
        this.tempUser = null;
    }

    //========================================Interactable========================================
    void Interactable.Interact(Player player)
    {
        this.tempUser = player;
        EventManager.Instance.OnBonfireResting?.Invoke();
        float distance = float.MaxValue;
        this.isResting = true;
        this.tempUser.Rest();

        foreach (Transform restPoint in this.restPoints)
        {
            float tempDistance = Vector2.Distance(this.transform.position, restPoint.position);

            if (tempDistance >= distance) continue;
            distance = tempDistance;
            this.chosentRestPoint = restPoint;
        }

        this.tempUser.Teleport(this.chosentRestPoint.position);
    }
}
