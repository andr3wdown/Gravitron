using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyBullet : Bullet
{

    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Enemy>() == null)
        {
            if(other.GetComponent<Character>() != null)
            {
                other.GetComponent<Character>().TakeDamage(damage);
            }
            Instantiate(particle, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
