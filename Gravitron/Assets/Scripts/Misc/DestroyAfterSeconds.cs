using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{
    public float seconds = 1f;
    private void Update()
    {
        seconds -= Time.deltaTime;
        if(seconds <= 0)
        {
            Destroy(gameObject);
        }
    }
}
