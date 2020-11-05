using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShipController : ShipController
{
    public BattleshipManager parentBattleship;

    public GameObject ammo;
    public Transform muzzlePoint;

    GameObject player;

    void Start()
    {
        team = Team.Enemy;
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(shootCoroutine());
    }

    public IEnumerator shootCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.5f);

            Shoot();

            yield return new WaitForSeconds(0.25f);

            Shoot();

            yield return new WaitForSeconds(0.25f);

            Shoot();

        }
        
    }

    void Shoot()
    {
        if (Vector3.Distance(player.transform.position, transform.position) > 150f) return;
        GameObject spawnedAmmo = Instantiate(ammo, muzzlePoint.position, Quaternion.LookRotation(player.transform.position - transform.position));
        print(spawnedAmmo);
        spawnedAmmo.GetComponent<Projectile>().target = player;
        spawnedAmmo.GetComponent<Projectile>().team = Team.Enemy;
    }

    protected override void Kill()
    {
        parentBattleship.spawnedEnemies.Remove(this);
        print("SHIP " + gameObject.ToString() + " DOWN");
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        parentBattleship.spawnedEnemies.Remove(this);
    }
}
