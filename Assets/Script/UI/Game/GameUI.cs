using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    [Header("===Game UI===")]
    [Header("Health Bar")]
    [SerializeField] private Transform healthBarBackground;
    [SerializeField] private Slider healthSlider;

    [Header("Money")]
    [SerializeField] private Text moneyText;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.healthBarBackground, transform.Find("HealthBar").Find("Background"), "LoadHealthBarBackground()");
        this.LoadComponent(ref this.healthSlider, transform.Find("HealthBar").Find("Slider"), "LoadSlider()");
    }

    private void FixedUpdate()
    {
        this.healthSlider.maxValue = GameManager.Instance.Player.MaxHealth;
        this.healthSlider.value = GameManager.Instance.Player.Health;
    }

    //===========================================Method===========================================
}
