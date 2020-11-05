using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingDetatch : MonoBehaviour
{
    public BattleshipTarget[] targets;
    public BattleshipSpawner[] spawners;
    Rigidbody rb;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        int num = 0;
        foreach (BattleshipTarget target in targets)
        {
            if (target.IsAlive())
            {
                num++;
            }
        }

        if(num == 0)
        {
            foreach(BattleshipTarget target in targets)
            {
                if (target.damaged)
                {
                    Destroy(target.damaged);
                }
                target.GetComponent<Collider>().enabled = false;
            }
            //rb.isKinematic = false;
            int sign = 1;
            if(this.gameObject.name == "Ring (2)")
            {
                sign = -1;
            }
            transform.position += sign * transform.right * 30 * Time.deltaTime;
            transform.Rotate(Random.Range(0,8) * Time.deltaTime, Random.Range(0, 3) * Time.deltaTime, Random.Range(0, 3) * Time.deltaTime);
            //rb.AddTorque(new Vector3(Random.Range(0, 100), Random.Range(0, 100), Random.Range(0, 100)), ForceMode.Impulse);

            foreach(BattleshipSpawner spawner in spawners)
            {
                spawner.health = -1;
            }

            //this.enabled = false;
        }
    }
}
