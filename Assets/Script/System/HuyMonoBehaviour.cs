using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtensions
{
    public static void SafeSetParent(this Transform child, Transform newParent)
    {
        if (child == null) return;
        Vector3 originalLocalScale = child.localScale;
        child.SetParent(newParent, false);
        child.localScale = originalLocalScale;
    }

    public static void SafeSetParent(this RectTransform child, Transform newParent)
    {
        if (child == null) return;

        Vector3 worldPos = child.position;
        Quaternion worldRot = child.rotation;
        Vector3 localScale = child.localScale;

        Vector2 anchorMin = child.anchorMin;
        Vector2 anchorMax = child.anchorMax;
        Vector2 anchoredPosition = child.anchoredPosition;
        Vector2 sizeDelta = child.sizeDelta;
        Vector2 pivot = child.pivot;

        child.SetParent(newParent, false);

        child.position = worldPos;
        child.rotation = worldRot;
        child.localScale = localScale;

        child.anchorMin = anchorMin;
        child.anchorMax = anchorMax;
        child.anchoredPosition = anchoredPosition;
        child.sizeDelta = sizeDelta;
        child.pivot = pivot;
    }
}


public class HuyMonoBehaviour : MonoBehaviour
{
    //===========================================Unity============================================
    protected virtual void Awake()
    {
        //this.LoadComponents();
    }

    protected virtual void OnEnable()
    {
        // For Override
    }

    protected virtual void OnDisable()
    {
        // For Override
    }

    //===========================================Method===========================================
    public virtual void MyFixedUpdate()
    {
        // For Override
    }

    public virtual void MyUpdate()
    {
        // For Override
    }

    public virtual void LoadComponents()
    {
        //For override
    }

    protected void LoadComponent<T>(ref T component, Transform obj, string message)
    {
        if (obj == null) return;
        component = obj.GetComponent<T>();
        Debug.LogWarning(transform.name + ": " + message, transform.gameObject);
    }

    protected void LoadComponent<T>(ref T component, string message) where T : MonoBehaviour
    {
        component = FindAnyObjectByType<T>();
        Debug.LogWarning(transform.name + ": " + message, transform.gameObject);
    }

    protected void LoadComponent<T>(ref InterfaceReference<T> component, Transform obj, string message)
        where T : class
    {
        if (obj == null) return;
        component.Value = obj.GetComponent<T>();
        Debug.LogWarning(transform.name + ": " + message, transform.gameObject);
    }

    protected void LoadComponent<T>(ref List<InterfaceReference<T>> components, Transform obj, string message)
        where T : class
    {
        components = new List<InterfaceReference<T>>();
        if (obj == null) return;
        foreach (Transform child in obj)
        {
            InterfaceReference<T> component = new();
            component.Value = child.GetComponent<T>();
            components.Add(component);
        }
        Debug.LogWarning(transform.name + ": " + message, transform.gameObject);
    }

    protected void LoadChildComponent<T>(ref T component, Transform obj, string message)
    {
        if (obj == null) return;
        component = obj.GetComponentInChildren<T>(true);
        Debug.LogWarning(transform.name + ": " + message, transform.gameObject);
    }

    protected virtual void LoadComponent<T>(ref List<T> components, Transform obj, string message)
    {
        components = new List<T>();
        if (obj == null) return;
        foreach (Transform child in obj) components.Add(child.GetComponent<T>());
        Debug.LogWarning(transform.name + ": " + message, transform.gameObject);
    }

    protected virtual void LoadSO<T>(ref T so, string filePath) where T : ScriptableObject
    {
        if (so == null || so.Equals(null))
        {
            so = Resources.Load<T>(filePath);
            Debug.LogWarning(transform.name + ": LoadSO()", transform.gameObject);
        }
    }
}