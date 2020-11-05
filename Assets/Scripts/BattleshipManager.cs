using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleshipManager : MonoBehaviour
{
    //weak points list
    public BattleshipTarget[] targets;

    //max spawnable enemies at one time
    public int maxEnemies = 20;

    //enemy spawners
    public BattleshipSpawner[] spawners;

    //list of spawned enemies, enemies need to remove themselves from list upon deletion
    public List<EnemyShipController> spawnedEnemies;

    void Start()
    {
        foreach(BattleshipSpawner spawner in spawners)
        {
            spawner.manager = this;
            spawner.StartSpawn();
        }

        spawnedEnemies = new List<EnemyShipController>();
    }

    void Update()
    {
        if(GetHealth() == 0)
        {
            Kill();
        }
    }

    public int GetHealth()
    {
        int num = 0;

        foreach(BattleshipTarget target in targets)
        {
            if (target.IsAlive())
            {
                num++;
            }
        }

        return num;
    }

    public bool IsAlive()
    {
        return GetHealth() > 0 ? true : false;
    }


    //Important that this method does not simply destroy the object as that will mess with win conditions
    void Kill()
    {
        print("Dead");
    }
}
