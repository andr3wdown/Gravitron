using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Andr3wDown.Math;

public class FirstBoss : Enemy
{
    public LayerMask obstacles;
    Character chara;
    public Cooldown movementC;
    Vector2 targetPos;
    EnemyWaveManager ewm;
    public int bossMode;
    /*
     * 0: front cannon
     * 1: homing
     * 2: flower pattern
     */
    public float shootingDistance;
    public Cooldown shootingCooldown;
    Animator anim;
    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        chara = FindObjectOfType<Character>();
        ewm = FindObjectOfType<EnemyWaveManager>();
    }
    private void Update()
    {
        if(chara != null)
        {
            movementC.cooldown -= Time.deltaTime;
            if (movementC.cooldown <= 0)
            {
                movementC.cooldown = movementC.delay;
                targetPos = ewm.GetBossPos(0.4f, 1f);
            }
            if (Vector2.Distance(transform.position, targetPos) < shootingDistance)
            {
                shootingCooldown.cooldown -= Time.deltaTime;
                if (shootingCooldown.cooldown <= 0)
                {
                    if(Physics2D.Linecast(transform.position, chara.transform.position, obstacles))
                    {
                        StartCoroutine(Shoot(2));
                    }
                    else
                    {
                        StartCoroutine(Shoot(1));
                    }
                    
                    shootingCooldown.cooldown = shootingCooldown.delay;
                }
                transform.rotation = Quaternion.Lerp(transform.rotation, MathOperations.LookAt2D(transform.position, chara.transform.position, 90), rotSpeed * Time.deltaTime);
            }
            else
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, MathOperations.LookAt2D(transform.position, targetPos, 90), rotSpeed * Time.deltaTime);
            }
         

            transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            
        }
        else
        {
            StartCoroutine(FindPlayer(4));
        }
    }
    public float minShootForce, maxShootForce;
    public Transform[] frontBarrels;
    public Transform[] homingBarrels;
    public Transform[] flowerBarrels;
    public GameObject bullet;
    public GameObject homingBullet;
    public float frontDelay;
    public float flowerDelay;
    public float spread;
    IEnumerator Shoot(int index)
    {
        anim.SetBool("Active", true);
        yield return new WaitForSeconds(1.7f);
        switch (index)
        {
            case 0:
                int shotCount = 12;
                for(int i = 0; i < shotCount; i++)
                {
                    for(int j = 0; j < frontBarrels.Length; j++)
                    {
                        Quaternion _rotation = Quaternion.Euler(0, 0, frontBarrels[j].transform.rotation.eulerAngles.z + Random.Range(-spread, spread));
                        GameObject go = Instantiate(bullet, frontBarrels[j].position, _rotation);
                        float force = Random.Range(minShootForce, maxShootForce);
                        go.GetComponent<Rigidbody2D>().AddForce(go.transform.up * force, ForceMode2D.Impulse);
                    }
                    yield return new WaitForSeconds(frontDelay);
                }
                break;
            case 1:
                int sd = 12;
                for (int i = 0; i < sd; i++)
                {
                    for (int j = 0; j < frontBarrels.Length; j++)
                    {
                        Quaternion _rotation = Quaternion.Euler(0, 0, frontBarrels[j].transform.rotation.eulerAngles.z + Random.Range(-spread, spread));
                        GameObject go = Instantiate(bullet, frontBarrels[j].position, _rotation);
                        float force = Random.Range(minShootForce, maxShootForce);
                        go.GetComponent<Rigidbody2D>().AddForce(go.transform.up * force, ForceMode2D.Impulse);
                    }
                    yield return new WaitForSeconds(frontDelay);
                }
                break;

            case 2:
                int sc = 6;
                float deg = 360f / sc;
                int lenght = 10;
                for(int y = 0; y < lenght; y++)
                {
                    for (int j = 0; j < flowerBarrels.Length; j++)
                    {
                        float rot = flowerBarrels[j].transform.rotation.eulerAngles.z;
                        rot += 15 * Time.deltaTime;
                        flowerBarrels[j].transform.rotation = Quaternion.Euler(0, 0, rot);
                        for (int i = 0; i < sc; i++)
                        {
                            Quaternion _rotation = Quaternion.Euler(0, 0, deg * i + rot);
                            GameObject go = Instantiate(bullet, flowerBarrels[j].position, _rotation);
                            float force = maxShootForce;
                            go.GetComponent<Rigidbody2D>().AddForce(go.transform.up * force, ForceMode2D.Impulse);
                        }
                    }
                    yield return new WaitForSeconds(flowerDelay);
                }
             
                
                break;
        }
        anim.SetBool("Active", false);
        yield return null;
    }
    IEnumerator FindPlayer(float timeseconds)
    {
        yield return new WaitForSeconds(timeseconds);
        chara = FindObjectOfType<Character>();
    }
    public override void TakeDamage(float amount, int a = 5)
    {
        base.TakeDamage(amount, 25);
    }
}
