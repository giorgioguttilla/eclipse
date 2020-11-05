using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleshipTarget : Destructible
{

    public GameObject damaged;

    void Start()
    {
        health = 3000;
        team = Team.Enemy;
        if (damaged)
        {
            damaged.SetActive(false);
        }
        
    }
    protected override void Kill()
    {
        if (damaged)
        {
            damaged.SetActive(true);
        }
        //gameObject.GetComponent<Renderer>().enabled = false;
        gameObject.tag = "Untagged";
        transform.GetChild(0).gameObject.GetComponent<Renderer>().enabled = false;
        //GetComponent<Collider>().enabled = false;
    }
}
