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
    //dash
    public float dashTime = 0.3f;
    public float currentDashTime;
    public float dashCoolDown = 3f;
    public float currentDashCooldown;
    public bool dashAllowed;
    public bool dashActive = false;
    public bool dash = false;

    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Jump") && IsGrounded())
        {
            Jump();
        }
        horizontal = Input.GetAxisRaw("Horizontal");

        Vector2 move = new Vector2(horizontal, vertical);

        if (horizontal > 0)
        {
            transform.localScale = new Vector2(1f, 1f);
        }
        if (horizontal < 0)
        {
            transform.localScale = new Vector2(-1f, 1f);
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
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
            //animator.enabled = false;
        }
        if (!Input.GetKey(KeyCode.J))
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
               /* if (dashAllowed)
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
        if(groundCheck != null)
        {
            return true;
        }
        return false;
    }
    void Dash()
    {
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
        Vector2 movement = new Vector2(dashForce, 0.0f);
        if(transform.localScale.x > 0)
        {
            if(horizontal==0)
            {
                transform.Translate(movement);
            }
            transform.Translate(movement*1.5f);
        }
        if(transform.localScale.x < 0)
        {
            if(horizontal==0)
            {
                transform.Translate(-movement);
            }
            transform.Translate(-movement*1.5f);
        }
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        animator.SetTrigger("Dash");
    }
}
