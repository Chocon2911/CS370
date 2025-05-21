#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomItemMenu
{
    [MenuItem("CONTEXT/HuyMonoBehaviour/Load Component")]
    private static void LoadComponentButton()
    {
        HuyMonoBehaviour component = Selection.activeGameObject.GetComponent<HuyMonoBehaviour>();
        component.LoadComponents();
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
    }

    [MenuItem("CONTEXT/DbObj/Random Id")]
    private static void RandomIdButton()
    {
        Entity entity = Selection.activeGameObject.GetComponent<Entity>();
        entity.RandomId();
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
    }
}

#endif
