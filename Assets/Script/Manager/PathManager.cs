using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : HuyMonoBehaviour
{
    [Serializable]
    public class Path
    {
        [SerializeField] private List<Transform> points = new List<Transform>();
        public List<Transform> Points => points;

        public Path(Transform path)
        {
            foreach (Transform point in path)
            {
                this.points.Add(point);
            }
        }
    }

    //==========================================Variable==========================================
    private static PathManager instance;
    public static PathManager Instance => instance;

    [SerializeField] private List<Path> paths;
    public List<Path> Paths => paths;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        List<Transform> tempPaths = new List<Transform>();
        this.LoadComponent(ref tempPaths, transform, "LoadPaths()");
        foreach (Transform path in tempPaths)
        {
            this.paths.Add(new Path(path));
        }
    }

    protected override void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("instance not null (transform)", transform.gameObject);
            Debug.LogError("Instance not null (instance)", instance.gameObject);
            return;
        }

        instance = this;
        base.Awake();
    }
}
