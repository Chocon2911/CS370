using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    private static ShopUI instance;
    public static ShopUI Instance => instance;

    [Header("Component")]
    [SerializeField] private Button buyBtn;
    [SerializeField] private Button closeBtn;
    [SerializeField] private Transform shopItemContainer;
    [SerializeField] private List<ShopSlotUI> slots = new();
    [SerializeField] private ShopSlotUI chosenSlot;
    [SerializeField] private Player player;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadChildComponent(ref this.shopItemContainer, transform.Find("Scroll").Find("Grid"), "LoadShopItemContainer()");
        this.LoadComponent(ref this.buyBtn, transform.Find("BuyBtn"), "LoadBuyBtn()");
        this.LoadComponent(ref this.closeBtn, transform.Find("CloseBtn"), "LoadCloseBtn()");
    }

    //===========================================Unity============================================
    private void Start()
    {
        base.OnEnable();
        this.buyBtn.onClick.AddListener(this.BuyBtnClicked);
        this.closeBtn.onClick.AddListener(this.CloseBtnClicked);
    }

    protected override void OnDisable()
    {
        base.OnDisable(); 
        this.chosenSlot = null;
        foreach (ShopSlotUI slot in this.slots)
        {
            UISpawner.Instance.Despawn(slot.transform);
        }
    }

    //===========================================Method===========================================
    public void TurnOnUI(Shop shop, Player player)
    {
        this.player = player;
        foreach (ShopItem item in shop.Items)
        {
            // Spawn Slot UI
            Transform newSlotUI = UISpawner.Instance.SpawnByCode(UICode.SHOP_SLOT, Vector2.zero, Quaternion.identity);
            newSlotUI.SetParent(this.shopItemContainer, false);
            newSlotUI.gameObject.SetActive(true);

            // Spawn Item UI
            Transform newItemUI = UISpawner.Instance.SpawnByCode(UICode.SHOP_ITEM, Vector2.zero, Quaternion.identity);
            newItemUI.SetParent(newSlotUI, false);
            newItemUI.gameObject.SetActive(true);
            ShopItemUI itemUI = newItemUI.GetComponent<ShopItemUI>();
            itemUI.Default(item);

            this.slots.Add(newSlotUI.GetComponent<ShopSlotUI>());
        }
        gameObject.SetActive(true);
    }    

    public void SetChosenSlot(ShopSlotUI slot)
    {
        if (this.chosenSlot != null) this.chosenSlot.TurnOffChosenBackground();
        this.chosenSlot = slot;
    }
    
    private void BuyBtnClicked()
    {
        if (this.chosenSlot == null) return;
        this.UpdateUI();
    }

    private void CloseBtnClicked()
    {
        gameObject.SetActive(false);
    }

    private void UpdateUI()
    {
        foreach (ShopSlotUI slot in this.slots)
        {
            slot.Item.UpdateUI();
        }
    }
}
