using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleDoor : HuyMonoBehaviour, PlayerCollidable
{
    [Header("Tele Door")]
    [SerializeField] protected Transform goalPoint;

    void PlayerCollidable.Interact(Player player)
    {
        player.transform.position = goalPoint.position;
    }
}
