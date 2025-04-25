using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface DoorUser
{
    void Move(int dir);
    float GetXPos();
    Transform GetTrans();
}
