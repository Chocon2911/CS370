using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public abstract class Spawner : HuyMonoBehaviour
{
    [Header("Spawner")]
    [SerializeField] protected Transform prefabObj;
    public Transform PrefabObj => prefabObj;

    [SerializeField] protected Transform holderObj;
    public Transform HolderObj => holderObj;

    [SerializeField] protected List<Transform> prefabs;
    public List<Transform> Prefabs => prefabs;

    [SerializeField] protected List<Transform> holders;
    public List<Transform> Holders => holders;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadPrefabs();
        foreach (Transform obj in this.prefabs) obj.gameObject.SetActive(false);
        this.LoadHolders();
        foreach (Transform obj in this.holders) obj.gameObject.SetActive(false);
    }

    //=======================================Load Component=======================================
    protected virtual void LoadPrefabObj()
    {
        if (this.prefabObj != null) return;
        this.prefabObj = transform.Find("Prefab");
        Debug.LogWarning(transform.name + ": Load PrefabObj", transform.gameObject);
    }

    protected virtual void LoadHolderObj()
    {
        if (this.holderObj != null) return;
        this.holderObj = transform.Find("Holder");
        Debug.LogWarning(transform.name + ": Load HolderObj", transform.gameObject);
    }

    protected virtual void LoadPrefabs()
    {
        this.prefabs = new List<Transform>();
        this.LoadPrefabObj();

        foreach (Transform obj in this.prefabObj) this.prefabs.Add(obj);
        Debug.LogWarning(transform.name + ": Load Prefabs", transform.gameObject);
    }

    protected virtual void LoadHolders()
    {
        this.holders = new List<Transform>();
        this.LoadHolderObj();

        foreach (Transform obj in this.holderObj) this.holders.Add(obj);
        Debug.LogWarning(transform.name + ": Load Holders", transform.gameObject);
    }

    //===========================================Spawn============================================
    public virtual Transform SpawnByName(string objName, Vector3 spawnPos, Quaternion spawnRot)
    {
        Transform spawnObj = this.GetObjByName(objName);
        Transform newObj = this.SpawnByObj(spawnObj, spawnPos, spawnRot);
        return newObj;
    }

    public virtual Transform SpawnByObj(Transform spawnObj, Vector3 spawnPos, Quaternion spawnRot)
    {
        Transform prefabObj = null;
        foreach (Transform obj in this.prefabs)
        {
            if (!obj.name.Equals(spawnObj.name)) continue;
            prefabObj = obj;
            break;
        }

        if (prefabObj == null)
        {
            Debug.LogError("SpawnObj not in Prefabs", transform.gameObject);
            return null;
        }

        return this.Spawn(prefabObj, spawnPos, spawnRot);
    }

    protected virtual Transform Spawn(Transform obj, Vector3 spawnPos, Quaternion spawnRot)
    {
        Transform newObj = this.GetObjFromHolder(obj);
        newObj.SetParent(this.holderObj);
        newObj.SetPositionAndRotation(spawnPos, spawnRot);
        return newObj;
    }

    //==========================================Despawn===========================================
    public virtual void Despawn(Transform obj)
    {
        this.holders.Add(obj);
        obj.SetParent(this.holderObj);
        obj.gameObject.SetActive(false);
    }

    //============================================Get=============================================
    protected virtual Transform GetObjByName(string objName)
    {
        foreach (Transform obj in this.prefabs)
        {
            if (obj.name == objName) return obj;
        }

        Debug.LogError(transform.name + ": Can't find any obj with name = " + objName, transform.gameObject);
        return null;
    }

    protected virtual Transform GetObjFromHolder(Transform spawnObj)
    {
        foreach (Transform obj in this.holders)
        {
            if (obj.name == spawnObj.name)
            {
                //Debug.Log(transform.name + ": From Holder", transform.gameObject);
                this.holders.Remove(obj);
                return obj;
            }
        }

        //Debug.Log(transform.name + ": Create new", transform.gameObject);
        Transform newObj = Instantiate(spawnObj);
        newObj.name = spawnObj.name;
        return newObj;
    }
}
