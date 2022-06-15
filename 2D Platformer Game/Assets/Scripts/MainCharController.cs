using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharController : MonoBehaviour
{
    public float speed = 3.0f;
    Rigidbody2D rigidbody2d;

    public float jumpForce;
    public float dashForce;
    public Transform feet;

    float horizontal;
    float vertical;
    public LayerMask groundLayers;
    public LayerMask platformLayers;
    //dash
    public float dashTime = 0.3f;
    public float currentDashTime;
    public float dashCoolDown = 3f;
    public float currentDashCooldown;
    public bool dashAllowed;
    public bool dashActive = false;
    public bool dash = false;
    public bool nodash;
    public float dashSpeed;

    public float currentTimeProj;
    public GameObject projectilePrefab;
    public GameObject bouncyHeadPrefab;
    public GameObject bouncyGearPrefab;
    public float currentTimeProjectile;
    public float maxTimeProjectile = 0.5f;
    public bool canShoot;
    public Transform shootPoint;
    public Transform shootPointDown;
    public Transform shootPointUp;
    public float projectileSpeed;
    public float timeBTWShots;
    public float goombaLaunchForce;
    public bool ballzShot = false;

    //health
    public int health { get { return currentHealth; } }
    int currentHealth;

    public int maxHealth = 5;
    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;

    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        lookDirection.Set(1f, 0f);
        currentHealth = maxHealth;
        canShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (horizontal > 0)
        {
            transform.localScale = new Vector2(1f, 1f);
        }
        if (horizontal < 0)
        {
            transform.localScale = new Vector2(-1f, 1f);
        }
        Vector2 move = new Vector2(horizontal, vertical);
        //if (Mathf.Approximately(0.0f,0.0f))
        //{
        //lookDirection.Set(transform.localScale.x, 0.0f);
        //}
        animator.SetBool("lookingUp", false);
        animator.SetBool("lookingDown", false);
        animator.SetBool("isDashing", false);
        //look in direction of move vector
        lookDirection.Set(transform.localScale.x, 0.0f);
        //set length as 1
        lookDirection.Normalize();

        if (Input.GetButtonDown("Jump") && IsGrounded() && nodash == false && !Input.GetKey(KeyCode.S))
        {
            Jump();
        }
        if (Input.GetKey(KeyCode.W) && IsGrounded())
        {
            animator.SetBool("lookingUp", true);
            lookDirection.Set(0.0f, 1.0f);
        }
        if (Input.GetKey(KeyCode.S))
        {
            animator.SetBool("lookingDown", true);
            lookDirection.Set(0.0f, -1.0f);
        }
        if (!Mathf.Approximately(move.x, 0.0f))
        {
            animator.SetBool("isRunning", true);
        }
        if (Mathf.Approximately(move.x, 0.0f))
        {
            animator.SetBool("isRunning", false);
        }
        if (Input.GetKey(KeyCode.J))
        {
            nodash = true;
            if (rigidbody2d.velocity.y == 0)
            {
                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                animator.SetBool("isRunning", false);
            }
        }
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }
        if (!Input.GetKey(KeyCode.J))
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            nodash = false;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            /*if (dashAllowed)
             {
                 GameObject smokeObject = Instantiate(smokePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
             }*/
            if (dash == true)
            {
                return;
            }
            dash = true;
            currentDashCooldown = dashCoolDown;
            currentDashTime = dashTime;
            dashAllowed = false;
        }
        if (dash == true)
        {
            if (currentDashTime > 0)
            {
                Dash();
                currentDashTime -= Time.deltaTime;
            }
            currentDashCooldown -= Time.deltaTime;
            currentDashTime -= Time.deltaTime;
        }
        if (currentDashCooldown < 0)
        {
            dash = false;
            dashAllowed = true;
        }

        if (currentDashTime > 0)
        {
            dashActive = true;
        }
        else
        {
            dashActive = false;
        }
        RaycastHit2D dashchk = Physics2D.Raycast(rigidbody2d.position, lookDirection, 1.2f, LayerMask.GetMask("Ground"));
        if (dashchk.collider != null)
        {
            dash = true;
        }
        if (IsGrounded() && Input.GetKey(KeyCode.I) && canShoot)
        {
            StartCoroutine(Launch());
        }
        if (rigidbody2d.position.y <= -6.5)
        {
            ChangeHealth(-1);
            Vector2 movement = new Vector2(rigidbody2d.velocity.x, jumpForce * 1.64f);
            rigidbody2d.velocity = movement;
            animator.SetTrigger("Jump");
        }
        if (rigidbody2d.position.x >= 65)
        {
            if (ballzShot == false)
            {
                StartCoroutine(shootBallz());
            }
        }
    }

    void FixedUpdate()
    {
        Vector2 movement = new Vector2(horizontal * speed, rigidbody2d.velocity.y);
        rigidbody2d.velocity = movement;
    }
    void Jump()
    {
        Vector2 movement = new Vector2(rigidbody2d.velocity.x, jumpForce);
        rigidbody2d.velocity = movement;
        animator.SetTrigger("Jump");
    }
    public bool IsGrounded()
    {
        Collider2D groundCheck = Physics2D.OverlapCircle(feet.position, 0.5f, groundLayers);
        Collider2D platformCheck = Physics2D.OverlapCircle(feet.position, 0.5f, platformLayers);
        if (groundCheck != null || platformCheck != null)
        {
            return true;
        }
        return false;
    }
    void Dash()
    {
        if (nodash)
        {
            return;
        }
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        Vector2 movement = new Vector2(dashForce * transform.localScale.x, 0.0f);
        rigidbody2d.velocity = movement;
        //rigidbody2d.AddForce(movement * lookDirection);

        animator.SetBool("isDashing", true);
    }
    IEnumerator Launch()
    {
        canShoot = false;
        if (lookDirection.x > 0)
        {
            GameObject projectileObject = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
            projectileObject.GetComponent<Rigidbody2D>().velocity = (lookDirection * projectileSpeed);
        }
        if (lookDirection.x < 0)
        {
            GameObject projectileObject = Instantiate(projectilePrefab, shootPoint.position, Quaternion.Euler(new Vector3(0, 0, 180)));
            projectileObject.GetComponent<Rigidbody2D>().velocity = (lookDirection * projectileSpeed);
        }
        if (lookDirection.y > 0)
        {
            GameObject projectileObject = Instantiate(projectilePrefab, shootPointUp.position, Quaternion.Euler(new Vector3(0, 0, 90)));
            projectileObject.GetComponent<Rigidbody2D>().velocity = (lookDirection * projectileSpeed);
        }
        if (lookDirection.y < 0)
        {
            GameObject projectileObject = Instantiate(projectilePrefab, shootPointDown.position, Quaternion.Euler(new Vector3(0, 0, 270)));
            projectileObject.GetComponent<Rigidbody2D>().velocity = (lookDirection * projectileSpeed);
        }
        yield return new WaitForSeconds(timeBTWShots);
        canShoot = true;
    }
    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
            {
                return;
            }
            isInvincible = true;
            invincibleTimer = timeInvincible;
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        Debug.Log(currentHealth);
    }
    public void GoombaLaunch()
    {
        rigidbody2d.AddForce(goombaLaunchForce * Vector2.up);
    }
    public bool isFalling()
    {
        if (rigidbody2d.velocity.y < 0)
        {
            return true;
        }
        return false;
    }
    IEnumerator shootBallz()
    {
        ballzShot = true;
        GameObject bouncyHead = Instantiate(bouncyHeadPrefab, new Vector2(93f, 1.1f), Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        GameObject bouncyGear = Instantiate(bouncyGearPrefab, new Vector2(93f, 1.1f), Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        bouncyHead = Instantiate(bouncyHeadPrefab, new Vector2(93f, 1.1f), Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        bouncyHead = Instantiate(bouncyHeadPrefab, new Vector2(93f, 1.1f), Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        bouncyGear = Instantiate(bouncyGearPrefab, new Vector2(93f, 1.1f), Quaternion.identity);
    }
}
