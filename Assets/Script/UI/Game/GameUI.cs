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
    }

    //===========================================Method===========================================
}
