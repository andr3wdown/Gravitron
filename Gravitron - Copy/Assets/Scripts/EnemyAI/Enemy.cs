using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum Mode
    {
        chasing,
        avoiding,
        shooting,
        idling
    }
    public static List<Enemy> enemyList;
    [Header("Basic Attributes")]
    [Space(10)]
    public float speed;
    public float hp;
    public float rotSpeed;
    public bool isDead;
    public Mode mode;
    public GameObject deathParticle;
    public GameObject pickup;
    public virtual void TakeDamage(float amount, int a=5)
    {
        hp -= amount;
        if (hp <= 0)
        {
            isDead = true;
            Instantiate(deathParticle, transform.position, transform.rotation);
            for(int i = 0; i < a; i++)
            {
                Vector3 pos = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), 0);
                pos += transform.position;
                Instantiate(pickup, pos, transform.rotation);
            }
            
            Destroy(gameObject);
        }
            

    }
    private void OnEnable()
    {
        if (enemyList == null)
            enemyList = new List<Enemy>();

        enemyList.Add(this);
    }
    private void OnDisable()
    {
        enemyList.Remove(this);
    }
}
