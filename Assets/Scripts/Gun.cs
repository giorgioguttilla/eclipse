using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab;

    // Start is called before the first frame update
    public void Shoot(Vector3 initialVelocity)
    {
        GameObject bullet = Instantiate(bulletPrefab,gameObject.transform.position,gameObject.transform.rotation);
        Rigidbody rb = bullet.gameObject.GetComponent<Rigidbody>();
        if (rb)
        {
            var locVel = transform.InverseTransformDirection(initialVelocity);
            rb.velocity = transform.TransformDirection(locVel);
            rb.AddForce(bullet.transform.forward * 400);
        }
    }
}
