using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CustomItemMenu
{
    [MenuItem("CONTEXT/HuyMonoBehaviour/Load Component")]
    private static void LoadComponentButton()
    {
        HuyMonoBehaviour component = Selection.activeGameObject.GetComponent<HuyMonoBehaviour>();
        component.LoadComponents();
    }
}
