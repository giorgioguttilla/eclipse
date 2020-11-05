using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShipAI : MonoBehaviour
{
    private Transform target;
    private Transform battleshipTransform;
    private BattleshipManager battleshipManager;
    
    public float rotationDamper;
    public float moveSpeed;
    public float dectectionRange;
    public float rayCastOffset;
    public float minDistanceToTurnAway;
    public float shootingRange;
    public float maxDistanceFromShip;
    private System.Random rand;
    private bool isRunningAway = false;
    public Gun gun;
    private Rigidbody rb;
    public bool showDebugRays;
    public bool showShootingRangeSphere;
    
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        battleshipTransform = GetComponent<EnemyShipController>().parentBattleship.transform;
        gun = gameObject.transform.GetChild(0).GetComponent<Gun>();
        rb = gameObject.GetComponent<Rigidbody>();
        battleshipManager = GetComponent<EnemyShipController>().parentBattleship;
        rand = new System.Random();
    }

    // Update is called once per frame
    void Update()
    {
        float currDistanceToPlayer = Vector3.Distance(target.position, transform.position);
        float currDistanceToBattleship = Vector3.Distance(battleshipTransform.position, transform.position);

      //  if (currDistanceToBattleship >= maxDistanceFromShip)
      //  {   //makes sure ships dont go too far
            //need to fix this
          //  battleshipManager.spawnedEnemies.Remove(this);
            //Destroy(gameObject);
           // Vector3 toBattleShip = battleshipTransform.position - transform.position;
           // StartCoroutine(MoveOverSeconds(this.gameObject, toBattleShip - Vector3.one * 5f, 1));
      //  }
        
        if (currDistanceToPlayer <= minDistanceToTurnAway && !isRunningAway)
        {    //pick random spot to move
            StartCoroutine(MoveOverSeconds(this.gameObject, transform.position - new Vector3(rand.Next(10,31), rand.Next(10,31), 0), rand.Next(7,15)));
            
        }
        else if (currDistanceToPlayer <= shootingRange && !isRunningAway)
        {
            if (rand.Next(1, 300) <=5)
            {
              //  Vector3 directionToPlayer = target.position - transform.position;
              //  gun.Shoot(directionToPlayer.normalized*5f);
         //     gun.Shoot(transform.forward*.1f);
               //gun.Shoot(rb.velocity);

                // print("Shot at: "+rb.velocity);
                 
            }
            
        }
        else if(!isRunningAway)
        {
            AiMove();
            Move();
        }
     
    }

    void Turn()
    {
        Vector3 pos = target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(pos, Vector3.up);
       // transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationDamper * Time.deltaTime);\
       //picks random value between .5 and 1 for rotation damper
       transform.rotation = Quaternion.Slerp(transform.rotation, rotation, (float)rand.NextDouble() % .6f +.4f * Time.deltaTime);
    }

    void Move()
    {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }

    void AiMove()
    {
        Vector3 left = transform.position - transform.right * rayCastOffset;
        Vector3 right = transform.position + transform.right * rayCastOffset;
        Vector3 up = transform.position + transform.up * rayCastOffset;
        Vector3 down = transform.position - transform.up * rayCastOffset;

        if (showDebugRays)
        {
            Debug.DrawRay(left, transform.forward * dectectionRange, Color.magenta);
            Debug.DrawRay(right, transform.forward * dectectionRange, Color.magenta);
            Debug.DrawRay(up, transform.forward * dectectionRange, Color.magenta);
            Debug.DrawRay(down, transform.forward * dectectionRange, Color.magenta);
        }
        
       
       
  

        RaycastHit hit;
        Vector3 offset = Vector3.zero;

        bool hitRight = Physics.Raycast(right, transform.forward, out hit, dectectionRange);
        bool hitLeft = Physics.Raycast(left, transform.forward, out hit, dectectionRange);
        bool hitUp = Physics.Raycast(up, transform.forward, out hit, dectectionRange);
        bool hitDown = Physics.Raycast(down, transform.forward, out hit, dectectionRange);

        if (hitRight && hitLeft && hitUp && hitDown)
        {    //if its running into something just turn around
            transform.Rotate(0f,180f,0f,Space.Self);
            hitRight = false;
            hitLeft = false;
            hitUp = false;
            hitDown = false;
        }
    
        
        if (hitRight)
        {
            offset += Vector3.right;
        }
        else if (hitLeft)
        {
            offset -= Vector3.right;
        }
        else if (hitUp)
        {
            offset += Vector3.up;
        }
        else if (hitDown)
        {
            offset -= Vector3.up;
        }

        if (offset != Vector3.zero)
        {
            float random = (float)(rand.NextDouble() * 3f + 1f);
            transform.Rotate(offset * random * Time.deltaTime);
        }
        else
        {
           if (rand.Next(1, 10) <= 5)
            {
                Turn();
                print("called turn");
            }
         
        }
    }

    
    public IEnumerator MoveOverSeconds (GameObject objectToMove, Vector3 end, float seconds)
    {
        isRunningAway = true;
        float elapsedTime = 0;
        Vector3 startingPos = objectToMove.transform.position;
        while (elapsedTime < seconds)
        {
            transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.position = end;
        isRunningAway = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        
        if (other.gameObject.tag == "Bullet")
        {
            //need to make it delete from giorgios list thing
            Destroy(gameObject);
            print("killed");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        if (showShootingRangeSphere)
        {
            Gizmos.DrawWireSphere(transform.position, shootingRange);

        }
    }
}


