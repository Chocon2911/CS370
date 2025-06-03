using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadDataUI : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    [Header("Component")]
    [SerializeField] private Transform container;
    [SerializeField] private Transform saveRowPrefab;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.container, transform.Find("Scroll").Find("Container"), "LoadContainer()");
    }

    protected override void Awake()
    {
        base.Awake();
        this.LoadDb();
    }

    //===========================================Method===========================================
    private void LoadDb()
    {
        
    }
}
