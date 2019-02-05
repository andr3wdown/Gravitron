using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public Cooldown shootDelay;
    public LineRenderer shootAimRenderer;
    public GameObject bullet;
    public float shootSpeed;

	void Update ()
    {
        shootDelay.cooldown -= Time.deltaTime;
#if UNITY_STANDALONE
        if (Input.GetKey(KeyCode.Mouse1) && shootDelay.cooldown <= 0)
        {
            shootAimRenderer.enabled = true;
            Vector3 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            dir.z = 0;

            shootAimRenderer.SetPosition(0, transform.position - transform.forward);
            shootAimRenderer.SetPosition(1, dir -transform.forward);
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1) && shootDelay.cooldown <= 0)
        {
            shootAimRenderer.enabled = false;
            shootDelay.cooldown = shootDelay.delay;
            Shoot(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
#endif
#if UNITY_ANDROID
        if(Input.touches.Length > 1)
        {
            if(Input.touches[1].phase == TouchPhase.Began || Input.touches[1].phase == TouchPhase.Stationary || Input.touches[1].phase == TouchPhase.Moved)
            {
                shootAimRenderer.enabled = true;
                Vector3 dir = Camera.main.ScreenToWorldPoint(Input.touches[1].position);
                dir.z = 0;

                shootAimRenderer.SetPosition(0, transform.position - transform.forward);
                shootAimRenderer.SetPosition(1, dir - transform.forward);
            }
            if(Input.touches[1].phase == TouchPhase.Ended)
            {
                shootAimRenderer.enabled = false;
                shootDelay.cooldown = shootDelay.delay;
                Shoot(Camera.main.ScreenToWorldPoint(Input.touches[1].position));
            }
        }
        else
        {
            shootAimRenderer.enabled = false;
        }
#endif
    }
    void Shoot(Vector3 _dir)
    {
        _dir.z = 0;
       //_dir.y *= 1.5f;
        Vector3 barrel = shootAimRenderer.transform.position;
        Vector3 toTarget = _dir - (barrel);
        GameObject go = Instantiate(bullet, barrel, transform.rotation);
        go.GetComponent<Rigidbody2D>().AddForce(toTarget * shootSpeed, ForceMode2D.Impulse);
    }
}
