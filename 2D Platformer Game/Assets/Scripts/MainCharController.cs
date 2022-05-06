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
    public bool nodash;
    public float dashSpeed;

    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        lookDirection.Set(1f, 0f);
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
        //look in direction of move vector
        lookDirection.Set(transform.localScale.x, 0.0f);
        //set length as 1
        lookDirection.Normalize();

        if (Input.GetButtonDown("Jump") && IsGrounded() && nodash == false)
        {
            Jump();
        }
        if (Input.GetKey(KeyCode.W))
        {
            animator.SetBool("lookingUp", true);
            lookDirection.Set(0.0f, 1.0f);
        }
        Debug.Log(lookDirection);
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
        if (!Input.GetKey(KeyCode.J))
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            nodash = false;
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
        RaycastHit2D dashchk = Physics2D.Raycast(rigidbody2d.position, lookDirection, 0.7f, LayerMask.GetMask("Ground"));
        if (dashchk.collider != null)
        {
            dash = true;
            //GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;

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
        if (groundCheck != null)
        {
            return true;
        }
        return false;
    }
    void Dash()
    {
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;

        if (nodash)
        {
            return;
        }

        Vector2 movement = new Vector2(dashForce, 0.0f);
        //rigidbody2d.velocity = movement * lookDirection;
        rigidbody2d.AddForce(movement * lookDirection);

        /* Vector2 dashmovement = new Vector2(dashForce, 0.0f);
         if (transform.localScale.x > 0)
         {
             transform.Translate(dashmovement * 1.5f);
         }
         if (transform.localScale.x < 0)
         {
             transform.Translate(-dashmovement * 1.5f);
         GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
         }*/
        animator.SetTrigger("Dash");
    }
}
