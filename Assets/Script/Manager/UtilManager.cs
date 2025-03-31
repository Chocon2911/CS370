using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilManager : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    private static UtilManager instance;
    public static UtilManager Instance => instance;

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

    //===========================================Rotate===========================================
    public void RotateFaceDir(int dir, Transform obj)
    {
        if (dir >= 0) obj.rotation = Quaternion.Euler(0, 0, 0);
        else obj.rotation = Quaternion.Euler(0, 180, 0);
    }

    //==========================================Raycast===========================================
    public void CheckIsGround(CapsuleCollider2D col, LayerMask layer, string tag, ref bool prevIsGround, ref bool isGround)
    {
        Vector2 size = col.size;
        Vector2 pos = col.transform.position;
        CapsuleDirection2D dir = col.direction;
        float angle = 0;

        Collider2D[] targetCols = Physics2D.OverlapCapsuleAll(pos, size, dir, angle, layer);

        foreach (Collider2D targetCol in targetCols)
        {
            if (targetCol.tag != tag) continue;
            prevIsGround = isGround;
            isGround = true;
            return;
        }

        prevIsGround = isGround;
        isGround = false;
    }

    public Transform ShootRaycast(float distance, LayerMask layer, string tag, Vector2 start, Vector2 dir)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(start, dir.normalized, distance, layer);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && hit.collider.CompareTag(tag)) return hit.transform;
        }

        return null;
    }
}
