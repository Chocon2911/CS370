using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    private static DoorManager instance;
    public static DoorManager Instance => instance;

    [SerializeField] protected List<Door> doors = new List<Door>();

    //==========================================Get Set===========================================
    public List<Door> Doors => doors;

    //===========================================Unity============================================
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

    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.doors, transform, "LoadDoors()");
    }
}
