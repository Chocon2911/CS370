using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopSlotUI : HuyMonoBehaviour, IPointerClickHandler
{
    //==========================================Variable==========================================
    [SerializeField] private Transform background;
    [SerializeField] private Transform chosenBackground;
    [SerializeField] private ShopItemUI item;
    [SerializeField] private ShopUI shopUI;

    //==========================================Get Set===========================================
    public ShopItemUI Item => this.item;
    public ShopUI ShopUI => this.shopUI;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.background, transform.Find("Background"), "LoadBackground");
        this.LoadComponent(ref this.chosenBackground, transform.Find("ChosenBackground"), 
            "LoadChosenBackground");
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        this.shopUI = transform.parent.parent.parent.GetComponent<ShopUI>();
        this.background.gameObject.SetActive(true);
        this.chosenBackground.gameObject.SetActive(false);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        this.chosenBackground.gameObject.SetActive(false);
        this.shopUI = null;
        this.item = null;
        UISpawner.Instance.Despawn(this.transform);
    }

    //===========================================Method===========================================
    public void TurnOnChosenBackground()
    {
        this.chosenBackground.gameObject.SetActive(true);
    }

    public void TurnOffChosenBackground()
    {
        this.chosenBackground.gameObject.SetActive(false);
    }

    public void SetItem(ShopItemUI item)
    {
        if (this.item != null)
        {
            this.item.transform.SetParent(null);
            this.item.Slot = null;
            UISpawner.Instance.Despawn(this.item.transform);
        }

        this.item = item;
        this.item.transform.SetParent(this.transform);
        this.item.Slot = this;
    }

    //===================================Pointer Click Handler====================================
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        this.TurnOnChosenBackground();
        this.shopUI.SetChosenSlot(this);
    }
}
