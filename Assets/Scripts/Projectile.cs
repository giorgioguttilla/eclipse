using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage = 1f;
    public float despawnTime = 5f;
    public float speed = 100f;

    Rigidbody rb;
    public GameObject target;
    public Team team;
    public float homingAmount = 10;

    public GameObject explosionPrefab;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * speed, ForceMode.Impulse);
        
        StartCoroutine(despawn());
    }

    void Update()
    {
        transform.RotateAround(transform.position, transform.forward, 200 * Time.deltaTime);
        if (target)
        {
            //rb.AddForce((target.transform.position - transform.position).normalized * homingAmount);
            //rb.velocity = Vector3.Lerp(rb.velocity, (target.transform.position - transform.position).normalized * rb.velocity.magnitude, homingAmount / 100);
            Vector3 dir = (target.transform.position) - transform.position;
            Quaternion rot = Quaternion.LookRotation(dir);
            if(Vector3.Angle(transform.forward, dir) < 100f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, rot, homingAmount * Time.deltaTime);
            }
            
            rb.velocity = transform.forward * speed;
        }
    }

    public IEnumerator despawn()
    {
        yield return new WaitForSeconds(despawnTime);
        Destroy(this.gameObject);
    }

    public void OnDestroy()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        Destructible hit = other.gameObject.GetComponent<Destructible>();

        if (hit != null && hit.team != team)
        {
            hit.Damage(damage);
            if (explosionPrefab)
            {
                Instantiate(explosionPrefab, this.transform.position, this.transform.rotation, this.transform.parent);
            }
            Destroy(gameObject);
        }
    }
}
