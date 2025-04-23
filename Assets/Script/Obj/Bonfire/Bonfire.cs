using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonfire : HuyMonoBehaviour, Interactable
{
    //========================================Interactable========================================
    void Interactable.Interact(Player player)
    {
        EventManager.Instance.OnBonfireResting?.Invoke();

    }
}
