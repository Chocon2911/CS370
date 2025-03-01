using UnityEngine;

public class Util
{
    //==========================================Variable==========================================
    [Header("Util")]
    private static Util instance;

    //==========================================Get Set===========================================
    public static Util Instance
    {
        get 
        {
            if (instance == null) instance = new Util();
            return instance;
        }
    }

    //===========================================Method===========================================
    public void IComponentErrorLog(Transform mainObj, Transform componentObj)
    {
        Debug.LogError("component not found", mainObj.gameObject);
        Debug.LogError("wrong component source", componentObj.gameObject);
    }
}
