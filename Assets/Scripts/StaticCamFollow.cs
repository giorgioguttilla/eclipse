using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticCamFollow : MonoBehaviour
{
    private Vector3 follow;

    // Start is called before the first frame update
    void Start()
    {
        follow = transform.position - Camera.main.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = follow + Camera.main.transform.position;
    }
}
