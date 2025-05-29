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

    public string RandomGUID()
    {
        return System.Guid.NewGuid().ToString();
    }

    //===========================================Rotate===========================================
    public void RotateFaceDir(int dir, Transform obj)
    {
        if (dir >= 0) obj.rotation = Quaternion.Euler(0, 0, 0);
        else obj.rotation = Quaternion.Euler(0, 180, 0);
    }

    //==========================================Raycast===========================================
    public void CheckIsGround(CapsuleCollider2D groundCol, LayerMask layer, string tag, ref bool prevIsGround, ref bool isGround)
    {
        Vector2 size = groundCol.size;
        Vector2 pos = groundCol.transform.position;
        CapsuleDirection2D dir = groundCol.direction;
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

    public void CheckIsGround(CapsuleCollider2D groundCol, LayerMask layer, string tag, ref bool prevIsGround, ref bool isGround, Vector2 scale)
    {
        Vector2 size = groundCol.size * scale;
        Vector2 pos = groundCol.transform.position;
        CapsuleDirection2D dir = groundCol.direction;
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

    //============================================Move============================================
    public void Move(Rigidbody2D rb, Vector2 vel)
    {
        rb.velocity += vel;
    }

    public void MoveForward(Rigidbody2D rb, float speed)
    {
        float angle = rb.transform.eulerAngles.z;
        float xDir = Mathf.Cos(angle * Mathf.Deg2Rad);
        float yDir = Mathf.Sin(angle * Mathf.Deg2Rad);
        Vector2 dir = new Vector2(xDir, yDir).normalized;
        rb.velocity = dir * speed;
    }

    public void FlyingWithAcceleration(Rigidbody2D rb, Vector2 dir, float speed, float speedUpTime, float slowDownTime)
    {
        float xVel = rb.velocity.x;
        float yVel = rb.velocity.y;

        // x dir
        int xDir = dir.x > 0 ? 1 : -1;
        if (xDir <= Mathf.Pow(0.1f, 3) && xDir >= -Mathf.Pow(0.1f, 3)) xDir = 0;
        float xSpeed = Mathf.Abs(dir.x * speed);
        this.MovingWithAccelerationInHorizontal(rb, xDir, xSpeed, speedUpTime, slowDownTime);

        // y dir
        int yDir = dir.y > 0 ? 1 : -1;
        if (yDir <= Mathf.Pow(0.1f, 3) && yDir >= -Mathf.Pow(0.1f, 3)) yDir = 0;
        float ySpeed = Mathf.Abs(dir.y * speed);
        this.MovingWithAccelerationInVertical(rb, yDir, ySpeed, speedUpTime, slowDownTime);
    }

    public void MovingWithAccelerationInHorizontal(Rigidbody2D rb, int dir, float speed, float speedUpTime, float slowDownTime)
    {
        float xVel = rb.velocity.x;
        float applySpeed = 0;

        if (dir > 0)
        {
            if (xVel >= speed)
            {
                this.SlowingDownWithAccelerationInHorizontal(rb, speed, slowDownTime);
                return;
            }
            else if (xVel > Mathf.Pow(-0.1f, 3))
            {
                applySpeed = dir * speed / speedUpTime * Time.deltaTime;
                if (applySpeed > speed - xVel) applySpeed = speed - xVel;
            }
            else
            {
                applySpeed = dir * speed / slowDownTime * Time.deltaTime;
                if (applySpeed > -xVel) applySpeed = -xVel;
            }
        }

        else if (dir < 0)
        {
            if (xVel <= -speed)
            {
                this.SlowingDownWithAccelerationInHorizontal(rb, speed, slowDownTime);
                return;
            }
            else if (xVel < Mathf.Pow(0.1f, 3))
            {
                applySpeed = dir * speed / speedUpTime * Time.deltaTime;
                if (applySpeed < -speed - xVel) applySpeed = -speed - xVel;
            }
            else
            {
                applySpeed = dir * speed / slowDownTime * Time.deltaTime;
                if (-applySpeed > xVel) applySpeed = -xVel;
            }
        }

        else
        {
            this.SlowingDownWithAccelerationInHorizontal(rb, speed, slowDownTime);
            return;
        }

        this.Move(rb, new Vector2(applySpeed, 0));
    }

    public void MovingWithAccelerationInVertical(Rigidbody2D rb, int dir, float speed, float speedUpTime, float slowDownTime)
    {
        float yVel = rb.velocity.y;
        float applySpeed = 0;

        if (dir > 0)
        {
            if (yVel >= speed)
            {
                this.SlowingDownWithAccelerationInVertical(rb, speed, slowDownTime);
                return;
            }
            else if (yVel > Mathf.Pow(-0.1f, 3))
            {
                applySpeed = dir * speed / speedUpTime * Time.deltaTime;
                if (applySpeed > speed - yVel) applySpeed = speed - yVel;
            }
            else
            {
                applySpeed = dir * speed / slowDownTime * Time.deltaTime;
                if (applySpeed > -yVel) applySpeed = -yVel;
            }
        }

        else if (dir < 0)
        {
            if (yVel <= -speed)
            {
                this.SlowingDownWithAccelerationInVertical(rb, speed, slowDownTime);
                return;
            }
            else if (yVel < Mathf.Pow(0.1f, 3))
            {
                applySpeed = dir * speed / speedUpTime * Time.deltaTime;
                if (applySpeed < -speed - yVel) applySpeed = -speed - yVel;
            }
            else
            {
                applySpeed = dir * speed / slowDownTime * Time.deltaTime;
                if (-applySpeed > yVel) applySpeed = -yVel;
            }
        }

        else
        {
            this.SlowingDownWithAccelerationInVertical(rb, speed, slowDownTime);
            return;
        }

        this.Move(rb, new Vector2(0, applySpeed));
    }

    public void SlowingDownWithAccelerationInHorizontal(Rigidbody2D rb, float speed, float slowDownTime)
    {
        float xVel = rb.velocity.x;
        float applySpeed = 0;

        if (xVel > 0)
        {
            applySpeed = -speed / slowDownTime * Time.deltaTime;
            if (-applySpeed > xVel) applySpeed = -xVel;
        }
        else if (xVel < 0)
        {
            applySpeed = speed / slowDownTime * Time.deltaTime;
            if (applySpeed > -xVel) applySpeed = -xVel;
        }

        this.Move(rb, new Vector2(applySpeed, 0));
    }

    public void SlowingDownWithAccelerationInVertical(Rigidbody2D rb, float speed, float slowDownTime)
    {
        float yVel = rb.velocity.y;
        float applySpeed = 0;

        if (yVel > 0)
        {
            applySpeed = -speed / slowDownTime * Time.deltaTime;
            if (-applySpeed > yVel) applySpeed = -yVel;
        }
        else if (yVel < 0)
        {
            applySpeed = speed / slowDownTime * Time.deltaTime;
            if (applySpeed > -yVel) applySpeed = -yVel;
        }

        this.Move(rb, new Vector2(0, applySpeed));
    }

    public virtual void ChaseTarget(Transform user, Transform target, float speed)
    {
        float xVel = target.position.x - user.position.x;
        float yVel = target.position.y - user.position.y;
        user.Translate(new Vector3(xVel, yVel, 0) * speed * Time.deltaTime);
    }

    //============================================Jump============================================
    public void Jump(Rigidbody2D rb, float jumpSpeed)
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
    }

    //============================================Stop============================================
    public void StopMove(Rigidbody2D rb)
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    public void StopJump(Rigidbody2D rb)
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
    }

    public void Stop(Rigidbody2D rb)
    {
        rb.velocity = Vector2.zero;
    }

    //===========================================Method===========================================
    public void Despawn(Spawner spawner, Transform despawnObj)
    {
        spawner.Despawn(despawnObj);
    }

    //======================================Despawn By Time=======================================
    public bool DespawnByTime(ref Cooldown despawnCD)
    {
        despawnCD.CoolingDown();

        if (!despawnCD.IsReady) return false;
        return true;
    }

    public void DespawnByTime(ref Cooldown despawnCD, Transform despawnObj, Spawner spawner)
    {
        if (this.DespawnByTime(ref despawnCD)) this.Despawn(spawner, despawnObj);
    }

    //====================================Despawn By Distance=====================================
    public bool DespawnByDistance(float despawnDistance, Vector2 despawnObjPos, Vector2 targetPos)
    {
        float xDistance = Mathf.Abs(despawnObjPos.x - targetPos.x);
        float yDistance = Mathf.Abs(despawnObjPos.y - targetPos.y);
        float currDistance = Mathf.Sqrt(xDistance * xDistance + yDistance * yDistance);

        if (currDistance > despawnDistance) return true;
        return false;
    }

    public void DespawnByDistance(float despawnDistance, Vector2 despawnObjPos, Vector2 targetPos, Transform despawnObj, Spawner spawner)
    {
        if (this.DespawnByDistance(despawnDistance, despawnObjPos, targetPos)) this.Despawn(spawner, despawnObj);
    }
}
