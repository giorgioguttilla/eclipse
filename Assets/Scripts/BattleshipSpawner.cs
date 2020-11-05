using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleshipSpawner : Destructible
{

    public float cooldownTime = 5f;

    public BattleshipManager manager;

    public GameObject enemyPrefab;

    void Start()
    {
        health = 1500;
        team = Team.Enemy;
    }

    public void StartSpawn()
    {
        StartCoroutine(DoSpawn());
    }

    IEnumerator DoSpawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(cooldownTime);

            if(manager.spawnedEnemies.Count < manager.maxEnemies)
            {
                GameObject spawnedEnemy = Instantiate(enemyPrefab, transform.position, transform.rotation);
                manager.spawnedEnemies.Add(spawnedEnemy.GetComponent<EnemyShipController>());
                spawnedEnemy.GetComponent<EnemyShipController>().parentBattleship = manager;
            }
        }
    }


    protected override void Kill()
    {
        //gameObject.GetComponent<Renderer>().enabled = false;
        transform.GetChild(0).gameObject.GetComponent<Renderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        gameObject.tag = "Untagged";

        StopAllCoroutines();
        
    }
}
