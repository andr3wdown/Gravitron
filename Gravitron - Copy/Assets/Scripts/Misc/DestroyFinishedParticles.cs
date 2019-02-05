using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class DestroyFinishedParticles : MonoBehaviour
{
	void Update ()
    {
        if (!GetComponent<ParticleSystem>().isPlaying)
        {
            Destroy(gameObject);
        }
	}
}
