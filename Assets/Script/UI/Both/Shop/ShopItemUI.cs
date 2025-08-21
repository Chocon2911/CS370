using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItemUI : HuyMonoBehaviour, IPointerClickHandler
{
    //==========================================Variable==========================================
    [Header("Component")]
    [SerializeField] private ShopSlotUI slot;
    [SerializeReference] private ShopItem item;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI priceTxt;

    //==========================================Get Set===========================================
    public ShopSlotUI Slot { get => slot; set => slot = value; }

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.icon, transform.Find("Icon"), "LoadIcon()");
        this.LoadComponent(ref this.priceTxt, transform.Find("PriceTxt"), "LoadPrice()");
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        this.slot = transform.parent.GetComponent<ShopSlotUI>();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        this.slot = null;
        UISpawner.Instance.Despawn(transform);
    }

    //===========================================Method===========================================
    public void Default(ShopItem item)
    {
        this.item = item;
        this.icon.sprite = item.so.Item.SO.Icon;
        this.priceTxt.text = item.so.Cost.ToString();
    }

    public void UpdateUI()
    {
        this.icon.sprite = this.item.so.Item.SO.Icon;
        this.icon.sprite = item.so.Item.SO.Icon;
        this.priceTxt.text = item.so.Cost.ToString();
    }

    //===================================Pointer Click Handler====================================
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        this.slot.TurnOnChosenBackground();
        this.Slot.ShopUI.SetChosenSlot(this.slot);
    }
}
