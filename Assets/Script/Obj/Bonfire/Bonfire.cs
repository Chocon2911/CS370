using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BonfireUser
{
    Vector2 GetPos();
    void Rest();
}

[RequireComponent(typeof(CircleCollider2D))]
public class Bonfire : HuyMonoBehaviour, Interactable
{
    //==========================================Variable==========================================
    [SerializeField] protected Transform guideArrow;
    [SerializeField] protected List<Transform> restPoints;
    [SerializeField] protected BonfireUser tempUser;
    [SerializeField] protected Transform chosentRestPoint;
    [SerializeField] protected bool isResting;
    [SerializeField] protected bool isDetected;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.restPoints, transform.Find("RestPoints"), "LoadRestPoints()");
        this.LoadComponent(ref this.guideArrow, transform.Find("Arrow"), "LoadGuideArrow()");
    }

    protected override void Awake()
    {
        base.Awake();
        EventManager.Instance.OnBonfireStopResting += StopResting;
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
    protected virtual void StopResting()
    {
        this.isResting = false;
        this.tempUser = null;
    }

    //========================================Interactable========================================
    void Interactable.Interact(Player player)
    {
        this.tempUser = player;
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

        GameManager.Instance.SetRestPoint(this.chosentRestPoint.position, this.chosentRestPoint.rotation);
        EventManager.Instance.OnBonfireResting?.Invoke();
    }

    void Interactable.Detected(Player player)
    {
        this.isDetected = true;
    }
}
