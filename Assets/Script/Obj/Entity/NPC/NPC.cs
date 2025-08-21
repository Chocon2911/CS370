using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
public class NPC : HuyMonoBehaviour, Interactable
{
    //==========================================Variable==========================================
    [SerializeField] private CapsuleCollider2D bodyCol;
    [SerializeField] private Shop shop;
    [SerializeField] private SpriteRenderer model;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.bodyCol, transform, "LoadBodyCol()");
        this.LoadComponent(ref this.model, transform.Find("Model"), "LoadModel()");
    }

    //========================================Interactable========================================
    void Interactable.Detected(Player player)
    {
        this.model.color = Color.yellow;
    }

    void Interactable.Interact(Player player)
    {
        ShopUI.Instance.TurnOnUI(this.shop, player);
    }
}
