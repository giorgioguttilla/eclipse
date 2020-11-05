using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveRotate : MonoBehaviour
{

    public float speed = 1f;

    void Update()
    {
        transform.Rotate(new Vector3(0, speed * Time.deltaTime, 0), Space.Self);
    }
}
