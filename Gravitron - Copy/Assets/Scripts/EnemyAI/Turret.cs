using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Andr3wDown.Math;

//see Enemy class for more details
public class Turret : Enemy
{
    Character chara;
    bool choiceMade = false;
    int choice = 0;
    public float minDistanceToPlanet;
    public float checkRadius;
    public Transform eye;
    public LayerMask planetLayer;
    Vector3 nextWaypoint;
    public Cooldown avCooldown;
    public Cooldown shootCooldown;
    public float shootDistance;
    public float stoppingDistance;
    bool isShooting;
    public float spread;
    public Transform[] barrels;
    public GameObject bullet;
    public float shootDelay;
    Animator anim;
    public float 
        minShootForce,
        maxShootForce;
    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        avCooldown.cooldown = avCooldown.delay;
        chara = FindObjectOfType<Character>();
    }
    private void FixedUpdate()
    {
        if (!isDead)
        {

            if(chara != null)
            {
                CheckForPlanet();
                switch (mode)
                {
                    case Mode.chasing:
                        choiceMade = false;
                        Move();
                        RotateTowards(chara.transform.position);
                        break;

                    case Mode.avoiding:

                        if (!choiceMade)
                        {
                            choice = ChooseDirection();
                            nextWaypoint = GetWorldPosition();
                        }
                        RotateTowards(nextWaypoint);
                        Move();
                        break;
                    case Mode.idling:
                        choiceMade = false;
                        RotateTowards(chara.transform.position);
                        break;
                }
                isShooting = InRange(shootDistance);
                if (isShooting)
                {
                    ShootControl();
                }

                if (mode == Mode.avoiding)
                {
                    avCooldown.cooldown -= Time.deltaTime;
                    if (avCooldown.cooldown <= 0)
                    {
                        mode = Mode.chasing;
                        avCooldown.cooldown = avCooldown.delay;
                    }

                }
            }
            else
            {
                StartCoroutine(FindPlayer(4));
            }
            
        }
    }
    IEnumerator FindPlayer(float timeseconds)
    {
        yield return new WaitForSeconds(timeseconds);
        chara = FindObjectOfType<Character>();
    }
    bool shootingInitiated = false;
    IEnumerator ShootingSequence()
    {
        shootingInitiated = true;
        anim.SetTrigger("Open");
        yield return new WaitForSeconds(shootDelay);
        Shoot();
        anim.SetTrigger("Close");
        shootingInitiated = false;
    }
    void ShootControl()
    {
        if (!shootingInitiated)
        {
            shootCooldown.cooldown -= Time.deltaTime;
            if (shootCooldown.cooldown <= 0)
            {
                shootCooldown.cooldown = shootCooldown.delay;
                StartCoroutine(ShootingSequence());             
            }
        }    
    }
    void Shoot()
    {     
            for(int i = 0; i < barrels.Length; i++)
            {
                Quaternion _rotation = Quaternion.Euler(0, 0, barrels[i].transform.rotation.eulerAngles.z + Random.Range(-spread, spread));
                GameObject go = Instantiate(bullet, barrels[i].position, _rotation);
                float force = Random.Range(minShootForce, maxShootForce);
                go.GetComponent<Rigidbody2D>().AddForce(go.transform.up * force, ForceMode2D.Impulse);
            }
                  
    }
    void CheckForPlanet()
    {
        if(Physics2D.CircleCast(eye.position, checkRadius, transform.up, minDistanceToPlanet, planetLayer))
        {
            mode = Mode.avoiding;
        }
        else if(mode != Mode.avoiding)
        {
            bool atStoppingD = InRange(stoppingDistance);
            if (atStoppingD)
            {
                mode = Mode.idling;
            }
            else
            {
                mode = Mode.chasing;
            }
        }
    }
    void RotateTowards(Vector3 _target)
    {
        Quaternion rot = MathOperations.LookAt2D(_target, transform.position, -90);
        Quaternion smoothedRot = Quaternion.Slerp(transform.rotation, rot, rotSpeed * Time.deltaTime);
        transform.rotation = smoothedRot;
    }
    void Move()
    {
        Vector3 pos = transform.position;
        pos += transform.up * Time.deltaTime * speed;
        transform.position = pos;
    }
    int ChooseDirection()
    {
        choiceMade = true;
        return Random.Range(0, 2);
    }
    Vector3 GetWorldPosition()
    {
        switch (choice)
        {
            case 0:
                return transform.position + (transform.right * 100);

            case 1:
                return transform.position + (-transform.right * 100);

            default:
                Debug.Log("Invalid direction!");
                return transform.position;
        }
    }
    bool InRange(float maxDistance)
    {      
        if(Vector3.Distance(transform.position, chara.transform.position) < maxDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
