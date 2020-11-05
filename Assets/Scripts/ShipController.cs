using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipController : Destructible
{
    public Rigidbody rb;
    //public Gun gun;
    public float speed = 5f;

    

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        if(transform.childCount > 0)
        {
            //gun = gameObject.transform.GetChild(0).GetComponent<Gun>();
        }
    }

    public void Roll(float degrees)
    {
        transform.Rotate(new Vector3(0,0,degrees), Space.Self);
    }

    public void Pitch(float degrees)
    {
        transform.Rotate(new Vector3(degrees,0,0), Space.Self);
    }

    public void Yaw(float degrees)
    {
        transform.Rotate(new Vector3(0, degrees, 0), Space.Self);
    }

    //public void Shoot()
    //{
    //    if (gun != null)
    //    {
    //        gun.Shoot(rb.velocity);
    //    }
    //}

    public void Accelerate(Vector3 dir)
    {
        if (rb)
        {
            rb.AddForce(transform.forward * dir.z * speed);
            rb.AddForce(transform.right * dir.x * speed);
            rb.AddForce(transform.up * dir.y * speed);
        }
    }

    protected override void Kill()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //print("DEAD");
    }
}