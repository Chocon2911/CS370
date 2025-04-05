using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : DefaultButton
{
    public override void OnClick()
    {
        GameManager.Instance.StartGame();
    }
}
