using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Andr3wDown.Math;

public class Pointer : MonoBehaviour
{
    [HideInInspector]
    public Transform target;
	void Update ()
    {
        if (target != null)
        {
            transform.rotation = MathOperations.LookAt2D(target.position, transform.position, -90);
        }
        else
            Destroy(gameObject);
	}
}
