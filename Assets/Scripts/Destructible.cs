using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Destructible : MonoBehaviour
{
    float _health = 100f;

    public Team team = Team.Enemy;
    public float health
    {
        get { return _health; }
        set
        {
            _health = value;
            //print(_health);     //////
            if (!IsAlive())
            {
                Kill();
            }
        }
    }
    public bool IsAlive()
    {
        if (health > 0f)
        {
            return true;
        }

        return false;
    }

    void OnDestroy()
    {
        health = -1;
    }

    //void OnCollisionEnter(Collision coll)
    //{
    //    Projectile hit = coll.gameObject.GetComponent<Projectile>();

    //    if (hit != null)
    //    {
    //        Damage(hit.damage);
    //    }
    //}

    public virtual float Damage(float num)
    {
        return health -= num;
    }

    //do whatever you need to do to disable the object
    protected abstract void Kill();

}
