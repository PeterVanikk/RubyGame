using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarBehaviour : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    Animator animator;

    Vector2 lookDirection = new Vector2(1, 0);
    public Transform wallCheck;
    public LayerMask GroundLayers;

    public float speed;
    public bool run;
    public bool slowDown = false;
    public float distance;
    public float jumpForcex;
    public float jumpForce;
    public bool jumpComplete;
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        lookDirection.Set(transform.localScale.x, 0f);
    }

    // Update is called once per frame
    public void Update()
    {
        if (run)
        {
            Vector2 movement = new Vector2(transform.localScale.x * speed, rigidbody2d.velocity.y);
            rigidbody2d.velocity = movement;
        }
        if (slowDown)
        {
            speed -= Time.deltaTime;
        }

        RaycastHit2D wallInfo = Physics2D.Raycast(wallCheck.position, lookDirection, distance, GroundLayers);
        if (wallInfo.collider != null)
        {
                StartCoroutine(jumpOverStep());
        }
    }
    public IEnumerator DieProcess()
    {
        slowDown = true;
        animator.SetTrigger("Die");
        yield return new WaitForSeconds(5f);
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
        run = false;
        animator.SetBool("Idle", true);
        yield return new WaitForSeconds(0.5f);
        Vector2 jump = new Vector2(jumpForcex * transform.localScale.x, jumpForce);
        rigidbody2d.velocity = jump;
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("isRunning", true);
        run = true;
    }
}
