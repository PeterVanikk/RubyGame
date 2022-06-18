using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarBehaviour : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    Animator animator;

    Vector2 lookDirection = new Vector2(1, 0);
    public Transform wallCheck;
    public Transform groundDetection;
    public LayerMask GroundLayers;
    public LayerMask platformLayers;

    public float speed;
    public bool slowDown = false;
    public float distance;
    public float jumpForcex;
    public float jumpForce;
    public bool jumpComplete;
    public bool alreadyJumped;
    void Start()
    {
        transform.localScale = new Vector2(-1, 1);
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        lookDirection.Set(transform.localScale.x, 0f);
    }

    // Update is called once per frame
    public void Update()
    {
        Vector2 movement = new Vector2(transform.localScale.x * speed, rigidbody2d.velocity.y);
        rigidbody2d.velocity = movement;
        if (slowDown)
        {
            speed -= Time.deltaTime;
        }

        RaycastHit2D wallInfo = Physics2D.Raycast(wallCheck.position, lookDirection, distance, GroundLayers);
        if (wallInfo.collider != null)
        {
            if (!alreadyJumped)
            {
                StartCoroutine(jumpOverStep());
                alreadyJumped = true;
            }
        }
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distance, GroundLayers);
        if (groundInfo.collider == false && rigidbody2d.velocity.y == 0)
        {
            if(!alreadyJumped)
            {
                StartCoroutine(jumpOverStep());
                alreadyJumped = true;
            }
        }
    }
    public IEnumerator DieProcess()
    {
        slowDown = true;
        animator.SetTrigger("Die");
        yield return new WaitForSeconds(3.2f);
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        enabled = false;
        yield return new WaitForSeconds(4.5f);
        Destroy(gameObject);
    }
    public void OnCollisionStay2D(Collision2D other)
    {
        MainCharController controller = other.gameObject.GetComponent<MainCharController>();
        if (controller != null)
        {
            controller.ChangeHealth(-1);
        }
    }
    IEnumerator jumpOverStep()
    {
        animator.SetBool("isRunning", false);
        animator.SetBool("Idle", true);
        speed = 0;
        yield return new WaitForSeconds(0.5f);
        animator.SetTrigger("Jump");
        animator.SetBool("Idle", false);
        Vector2 jumpVector = new Vector2(0, 1);
        rigidbody2d.AddForce(jumpForce * jumpVector);
        speed = 6;
        yield return new WaitForSeconds(0.9f);
        animator.SetBool("isRunning", true);
        speed = 3.2f;
        alreadyJumped = false;
    }
}
