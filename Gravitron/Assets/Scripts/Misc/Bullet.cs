using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public static List<Bullet> allBullets;
    public GameObject particle;
    public float damage;
    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<Character>() == null)
        {
            if(other.GetComponent<Enemy>() != null)
            {
                other.GetComponent<Enemy>().TakeDamage(damage);
            }
            Instantiate(particle, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
    public void OnEnable()
    {
        if(allBullets == null)
        {
            allBullets = new List<Bullet>();
        }
        Bullet.allBullets.Add(this);
    }
    public void OnDisable()
    {
        Bullet.allBullets.Remove(this);
    }
}
