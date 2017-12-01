using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public float range;
    public LayerMask playerLayer;
    public void Update()
    {
        Collider2D p = Physics2D.OverlapCircle(transform.position, range, playerLayer);
        if (p != null)
        {
            transform.position = Vector2.Lerp(transform.position, p.transform.position, 10f * Time.deltaTime);
        }
    }

}
