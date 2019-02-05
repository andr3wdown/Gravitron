using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Range(0.0f, 25.0f)]
    public float smoothing = 2.5f;

    public float zOffset = -10;
    public Transform target;
    private void FixedUpdate()
    {
        if(target != null)
        {
            Vector3 dir = target.position;
            dir.z = zOffset;

            Vector3 smoothDir = Vector3.Lerp(transform.position, dir, smoothing * Time.deltaTime);
            transform.position = smoothDir;
        }
    }
}
