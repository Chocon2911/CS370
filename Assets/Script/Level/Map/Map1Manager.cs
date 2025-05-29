using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map1Manager : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    private static Map1Manager instance;
    public static Map1Manager Instance => instance;

    [Header("Elite Bat Dead")]
    [SerializeField] private Bat eliteBat;
    [SerializeField] private Transform door;


}
