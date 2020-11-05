using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerShipController : ShipController
{

    public GameObject Ammo;
    public GameObject Missile;

    public Transform muzzlePoint;

    public Slider HealthBar;
    public Slider AmmoBar;
    public Slider MissileBar;

    public float maxHealth = 100f;
    public int maxAmmo = 100;
    public float ammoRechargeRate = 5f;
    public float missileRechargeRate = 40f;
    public int shootRechargeTimeInFrames = 20;

    int currentAmmo;
    float currentMissileRecharge;

    int shootRechargeCooldown = 0;
    

    void Start()
    {
        //health = maxHealth;
        team = Team.Player;

        HealthBar.maxValue = maxHealth;
        HealthBar.value = maxHealth;

        AmmoBar.maxValue = maxAmmo;
        AmmoBar.value = maxAmmo;

        MissileBar.maxValue = 1;
        MissileBar.value = 1;

        StartCoroutine(ammoRecharge());
        StartCoroutine(missileRecharge());
    }

    public void ShootAmmo()
    {
        if(currentAmmo > 0)
        {
            GameObject spawnedAmmo = Instantiate(Ammo, muzzlePoint.position, muzzlePoint.rotation);
            spawnedAmmo.GetComponent<Projectile>().target = UITargetIdentifier.instance.closestEnemyToCenter;
            spawnedAmmo.GetComponent<Projectile>().team = Team.Player;

            currentAmmo--;
            AmmoBar.value = currentAmmo;
            shootRechargeCooldown = shootRechargeTimeInFrames;
        }
        
    }

    public void ShootMissile()
    {
        if (currentMissileRecharge.CompareTo(1) >= 0f)
        {
            GameObject spawnedMissile = Instantiate(Missile, muzzlePoint.position, muzzlePoint.rotation);
            spawnedMissile.GetComponent<Projectile>().target = UITargetIdentifier.instance.closestEnemyToCenter;
            spawnedMissile.GetComponent<Projectile>().team = Team.Player;

            currentMissileRecharge = 0;
            
        }
        
    }

    public void Update()
    {
        MissileBar.value = currentMissileRecharge;
        AmmoBar.value = currentAmmo;
        HealthBar.value = health;

        if(shootRechargeCooldown > 0)
        {
            shootRechargeCooldown -= 1;
        }

        if (!IsAlive())
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public override float Damage(float num)
    {
        StartCoroutine(camShake());
        return health -= num;
        
    }

    public IEnumerator camShake()
    {
        yield return new WaitForSeconds(0.1f);

        Camera.main.transform.position += new Vector3(-0.05f, 0, 0);

        yield return new WaitForSeconds(0.1f);

        Camera.main.transform.position += new Vector3(0.1f, 0, 0);

        yield return new WaitForSeconds(0.1f);

        Camera.main.transform.position += new Vector3(-0.05f, 0, 0);
    }

    public IEnumerator ammoRecharge()
    {
        while (true)
        {
            yield return new WaitForSeconds(ammoRechargeRate / 60f);
            if(currentAmmo < maxAmmo && shootRechargeCooldown == 0)
            {
                currentAmmo++;
            }
            
        }
        
    }

    public IEnumerator missileRecharge()
    {
        while (true)
        {
            yield return new WaitForSeconds(missileRechargeRate / 60f);
            if(currentMissileRecharge < 1f)
            {
                currentMissileRecharge += 0.01f;
            }
            
        }

    }

}
