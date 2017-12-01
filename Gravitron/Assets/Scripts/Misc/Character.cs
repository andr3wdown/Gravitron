using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Andr3wDown.Math;

[RequireComponent(typeof(Rigidbody2D))]
public class Character : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody2D rb;
    [HideInInspector]
    public float hp;
    public float maxHp;
    [HideInInspector]
    public Transform closestPlanet;
    public float jumpForce;
    public float rotationSpeed;
    Vector3 dir;
    bool isCharging;
    public float groundCheckDistance;
    public LayerMask planetLayer;
    bool isGrounded;
    bool isTooHigh;
    public LineRenderer ln;
    public Cooldown deathCooldown;
    public Cooldown jumpRecovery;
    public Cooldown chargeCooldown;
    public Transform flareSpot;
    public GameObject flare;
    public float chargeSpeed = 2.5f;
    Animator anim;
    public int maxJumpCount = 3;
    [HideInInspector]
    public int jumpCount;
    GameManager gm;

    GameObject warning;
	void Start ()
    {      
        gm = FindObjectOfType<GameManager>();
        warning = gm.warning;
        if (center.childCount < 1)
        {
            ConfigurePointers();
        }
        hp = maxHp;
        chargeCooldown.cooldown = chargeCooldown.delay;
        jumpCount = maxJumpCount;
        jumpRecovery.cooldown = jumpRecovery.delay;
        anim = transform.GetChild(0).GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        InputChecks();
    }
    void InputChecks()
    {
        if(isGrounded && !isCharging)
        {
            jumpCount = maxJumpCount;
        }
#if UNITY_ANDROID

        if (Input.touches.Length > 0 && Input.touches.Length < 2)
        {
            if (isGrounded)
            {
                if (Input.touches[0].phase == TouchPhase.Stationary || Input.touches[0].phase == TouchPhase.Moved)
                {
                    isTooHigh = true;
                    SetJumpMeter(Input.touches[0].position);
                }
            }
            else
            {
                Vector3 dir = SetJumpMeter(Input.mousePosition);
                isTooHigh = TooCloseToPlanet();
                if (!isTooHigh)
                {
                    jumpRecovery.cooldown = jumpRecovery.delay;
                    Quaternion desiredRot = MathOperations.LookAt2D(dir, transform.position, 90 + 180);
                    transform.rotation = Quaternion.Slerp(transform.rotation, desiredRot, rotationSpeed * Time.deltaTime);
                }
            }

            if (Input.touches[0].phase == TouchPhase.Ended && !GameManager.isPaused)
            {
                ln.enabled = false;
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                dir = mousePos;
                Jump(dir);
                dir = Vector3.zero;
            }

        }
        else
        {
            ln.enabled = false;
            if (!isGrounded)
            {
                jumpRecovery.cooldown -= Time.deltaTime;
                if (jumpRecovery.cooldown <= 0)
                {
                    isTooHigh = true;
                }

            }
            else
            {
                isTooHigh = true;
            }
        }

#endif

#if UNITY_STANDALONE
        if (Input.GetKey(KeyCode.Mouse0) && isGrounded)
        {         
            isTooHigh = true;
            SetJumpMeter(Input.mousePosition);

        }
        else if (Input.GetKey(KeyCode.Mouse0) && !isGrounded)
        {
            Vector3 dir = SetJumpMeter(Input.mousePosition);
            isTooHigh = TooCloseToPlanet();
            if (!isTooHigh)
            {
                jumpRecovery.cooldown = jumpRecovery.delay;
                Quaternion desiredRot = MathOperations.LookAt2D(dir, transform.position, 90 + 180);
                transform.rotation = Quaternion.Slerp(transform.rotation, desiredRot, rotationSpeed * Time.deltaTime);
            }
        }
        else
        {
            if (!isGrounded)
            {
                jumpRecovery.cooldown -= Time.deltaTime;
                if(jumpRecovery.cooldown <= 0)
                {            
                    isTooHigh = true;
                }
              
            }
            else
            {
                isTooHigh = true;
            }
        }
            

        if (Input.GetKeyUp(KeyCode.Mouse0) && !GameManager.isPaused)
        {
            ln.enabled = false;
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            dir = mousePos;
            Jump(dir);
            dir = Vector3.zero;
        }
#endif
        if (isCharging)
        {
            chargeCooldown.cooldown -= Time.deltaTime;
            if(chargeCooldown.cooldown <= 0)
            {
                isCharging = false;
                chargeCooldown.cooldown = chargeCooldown.delay;
            }
        }
    }
    public float minDistanceToPlanet;
    bool TooCloseToPlanet()
    {
        if(closestPlanet != null)
        {
            float distance = Vector3.Distance(transform.position, closestPlanet.position);
            if (distance < minDistanceToPlanet)
            {
                return true;
            }
            else
                return false;
        }
        else
        {
            return false;
        }
    }
    Vector3 SetJumpMeter(Vector3 inputVector)
    {
        ln.enabled = true;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(inputVector);
        dir = mousePos;//Vector3.Lerp(transform.position, mousePos, (Mathf.Sin(Time.time * 5) + 1) / 2);

        ln.SetPosition(0, transform.position - Vector3.forward);
        ln.SetPosition(1, dir - Vector3.forward);
        return mousePos;
    }
    public float maxSpeed;
    void Jump(Vector3 _dir)
    {
        if(jumpCount > 0)
        {
            isCharging = true;
            Instantiate(flare, flareSpot);
            if (isGrounded)
                anim.SetTrigger("Jump");

            _dir = _dir - transform.position;
            _dir.y *= 1.5f;
            if (_dir.magnitude > maxSpeed)
            {
                _dir = _dir.normalized * maxSpeed;
            }
            rb.AddForce(_dir * jumpForce, ForceMode2D.Impulse);
            jumpCount--;
        }
        else
        {
            //TODO: add player message!!!
        }
        
    }
	void FixedUpdate ()
    {
        MoveChecks();
    }
    void MoveChecks()
    {
       

        if (GravityBody.bodiesInRange != null && GravityBody.bodiesInRange.Count > 0)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            closestPlanet = MathOperations.FindClosestTarget(GravityBody.bodiesInRange, transform);
        }
        else
        {
            closestPlanet = null;
            rb.constraints = RigidbodyConstraints2D.None;
        }
        if (closestPlanet == null)
        {
            warning.SetActive(true);
        }
        else
        {
            warning.SetActive(false);
        }

        if (closestPlanet != null && isTooHigh)
        {
            deathCooldown.delay = 3;
            Quaternion desiredRot = MathOperations.LookAt2D(closestPlanet.position, transform.position, 90);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRot, rotationSpeed * Time.deltaTime);
        }
        else if (closestPlanet != null && !isTooHigh)
        {
            deathCooldown.delay = 3;
        }
        else
        {
            TakeDamage(Time.deltaTime * 6.66667f);
        }
        CheckForGround();
    }
    void CheckForGround()
    {
        if (Physics2D.Linecast(transform.position, transform.position - (transform.up * groundCheckDistance), planetLayer))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
       // anim.SetBool("isGrounded", isGrounded);
    }
    void EyeMovement()
    {

    }
    private void OnDrawGizmosSelected()
    {
        Debug.DrawLine(transform.position, transform.position - (transform.up * groundCheckDistance));
    }
    public void TakeDamage(float amount)
    {
        hp -= amount;
        if(hp <= 0)
        {
            GameManager.Death();
        }
    }
    public void AddHealth(float amount)
    {
        hp += amount;
    }
    public Transform center;
    public GameObject pointer;
    public void ConfigurePointers()
    {
        if(center != null)
        {
            if (Enemy.enemyList != null)
            {
                if (Enemy.enemyList.Count > 0)
                {
                    for (int i = 0; i < Enemy.enemyList.Count; i++)
                    {
                        GameObject go = Instantiate(pointer, center.position, center.rotation);
                        go.transform.SetParent(center);
                        go.transform.position = center.position;
                        go.GetComponent<Pointer>().target = Enemy.enemyList[i].transform;
                    }
                }
            }
        }
      
    }
    private void OnEnable()
    {
        EnemyWaveManager.chara = this;
    }
    private void OnDisable()
    {
        EnemyWaveManager.chara = null;
        if(warning != null)
        {
            warning.SetActive(false);        
        }                   
    }
}

[System.Serializable]
public class Cooldown
{
    [HideInInspector]
    public float cooldown;
    public float delay;
}
