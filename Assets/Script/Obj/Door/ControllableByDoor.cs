using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ControllableByDoor
{
    void Move(int dir);
    float GetXPos();
}
