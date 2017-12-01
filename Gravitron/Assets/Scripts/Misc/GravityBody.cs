using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GravityBody : MonoBehaviour
{
    const float G = 6.674f * 0.5f;
    public float gravityDistance;
    public LayerMask playerLayer;
    Rigidbody2D rb;
    Character chara;
    public static List<Transform> bodiesInRange = new List<Transform>();
    public static List<GravityBody> allBodies = new List<GravityBody>();
    private void OnEnable()
    {
        allBodies.Add(this);
    }
    private void OnDisable()
    {
        allBodies.Remove(this);
    }

    void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        s = Random.Range(-30f, 30f);
    }
    float s;
    float rotZ = 0;
	void FixedUpdate ()
    {
        rotZ += s * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, 0, rotZ);
        Collider2D col  = Physics2D.OverlapCircle(transform.position, gravityDistance, playerLayer);
        if (col != null)
        {
            if (!bodiesInRange.Contains(this.transform))
            {
                bodiesInRange.Add(this.transform);
            }
            chara = col.GetComponent<Character>();
            Attract(chara);

        }
        else
        {
            if (bodiesInRange.Contains(this.transform))
            {
                bodiesInRange.Remove(this.transform);
            }
            chara = null;
        }
        if(Bullet.allBullets != null)
        {
            for(int i = 0; i < Bullet.allBullets.Count; i++)
            {
                Attract(Bullet.allBullets[i].transform);
            }
        }
            
	}
    void Attract(Character _chara)
    {
        Rigidbody2D otherRb = _chara.rb;
       
        Vector2 direction = rb.position - otherRb.position;
        float distance = direction.magnitude;

        float forceMagnitude = G * (rb.mass * otherRb.mass) / Mathf.Pow(distance, 2);
        Vector2 force = direction.normalized * forceMagnitude;
        otherRb.AddForce(force);
    }
    void Attract(Transform target)
    {
        Rigidbody2D otherRb = target.GetComponent<Rigidbody2D>();

        Vector2 direction = rb.position - otherRb.position;
        float distance = direction.magnitude;

        float forceMagnitude = G * (rb.mass * otherRb.mass) / Mathf.Pow(distance, 2);
        Vector2 force = direction.normalized * forceMagnitude;
        otherRb.AddForce(force);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.GetComponent<Character>() != null)
        {
            collision.transform.SetParent(this.transform);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.transform.GetComponent<Character>() != null)
        {
            collision.transform.SetParent(null);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, gravityDistance);
    }
    public LayerMask planetLayer;
    public static Vector2 GetSpawnPos()
    {
        Vector2 pos = Vector2.zero;
        int iter = 0;
        while(pos == Vector2.zero && iter < 1000)
        {
            int planetIndex = Random.Range(0, allBodies.Count);
            Vector2 checkVector = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
            Transform t = allBodies[planetIndex].transform;
            if (!Physics2D.OverlapCircle((Vector2)t.position + checkVector * allBodies[planetIndex].gravityDistance, 0.1f, allBodies[planetIndex].planetLayer))
            {
                return (Vector2)t.position + checkVector * allBodies[planetIndex].gravityDistance;
            }
        }
        return Vector2.zero;
        
    }
    public static Vector2 GetSpawnPos(float rad, float checkRad=0.1f)
    {
        Vector2 pos = Vector2.zero;
        int iter = 0;
        while (pos == Vector2.zero && iter < 1000)
        {
            int planetIndex = Random.Range(0, allBodies.Count);
            Vector2 checkVector = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
            Transform t = allBodies[planetIndex].transform;
            if (!Physics2D.OverlapCircle((Vector2)t.position + checkVector * allBodies[planetIndex].gravityDistance * rad, checkRad, allBodies[planetIndex].planetLayer))
            {
                return (Vector2)t.position + checkVector * allBodies[planetIndex].gravityDistance * rad;
            }
        }
        return Vector2.zero;

    }

}
