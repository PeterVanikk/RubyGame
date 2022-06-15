using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SpiderBehaviour : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    Animator animator;
    public LayerMask groundLayers;
    public LayerMask platformLayers;
    public LayerMask playerlayer;

    public float speed;
    public float distance;
    public bool alive;
    public float range;
    private float distToPlayer;
    public float jumpForce;
    public float jumpForcex;
    public bool notHitting = true;

    public bool noplayer;
    public Transform groundDetection;
    public Transform wallDetection;
    public Transform player;
    public Transform feet;
    public bool flipAllowed = true;

    Vector2 lookDirection = new Vector2(1, 0);

    //health
    public int health { get { return currentHealth; } }
    int currentHealth;
    public int maxHealth = 5;
    void Start()
    {
        animator = GetComponent<Animator>();
        lookDirection.Set(1f, 0f);
        rigidbody2d = GetComponent<Rigidbody2D>();
        GameObject player = GameObject.Find("MainCharacter");
        currentHealth = maxHealth;
    }
    void Update()
    {
        if (noplayer && notHitting)
        {
            animator.SetBool("isRunning", true);
        }
        if (!notHitting)
        {
            animator.SetBool("isRunning", false);
        }
        distToPlayer = Vector2.Distance(transform.position, player.position);
        if (distToPlayer <= range && player.position.y + 2.0f > transform.position.y)
        {
            if (player.position.y - 2.0f < transform.position.y)
            {
                //noplayer = false;
                if (player.position.x > transform.position.x && transform.localScale.x < 0)
                {
                    StartCoroutine(flipRight());
                }
                if (player.position.x < transform.position.x && transform.localScale.x > 0)
                {
                    StartCoroutine(flipLeft());
                }
            }
        }
        else
        {
            StartCoroutine(noPlayerTrue());
        }
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        lookDirection.Set(transform.localScale.x, 0.0f);
        if (noplayer && notHitting)
        {
            transform.Translate(lookDirection * speed * Time.deltaTime);
        }

        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distance);
        if (groundInfo.collider == false)
        {
            if (distToPlayer <= range && player.position.y <= transform.position.y - 1.0f && IsGrounded())
            {
                Jump();
            }
            else if (flipAllowed)
            {
                if (transform.localScale.x == 1f)
                {
                    transform.localScale = new Vector2(-1f, 1f);
                }
                else
                {
                    transform.localScale = new Vector2(1f, 1f);
                }
            }
        }
        if (currentHealth <= 0)
        {
            if (alive == true)
            {
                StartCoroutine(Kill());
                alive = false;
            }
        }
        RaycastHit2D wallInfo = Physics2D.Raycast(wallDetection.position, lookDirection, distance);
        if (wallInfo.collider != null)
        {
            if (wallInfo.collider.gameObject.CompareTag("player") || wallInfo.collider.gameObject.CompareTag("spider") || wallInfo.collider.gameObject.CompareTag("ball") || wallInfo.collider.gameObject.CompareTag("Projectile"))
            {
                return;
            }
            if (transform.localScale.x == 1f)
            {
                transform.localScale = new Vector2(-1f, 1f);
            }
            else
            {
                transform.localScale = new Vector2(1f, 1f);
            }
        }
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
    void OnCollisionEnter2D(Collision2D other)
    {
        MainCharController controller = other.gameObject.GetComponent<MainCharController>();
        if (controller != null)
        {
            if (controller.GetComponent<MainCharController>().IsGrounded() == false)
            {
                StartCoroutine(Kill());
            }
        }
    }
    void OnCollisionStay2D(Collision2D other)
    {
        MainCharController controller = other.gameObject.GetComponent<MainCharController>();
        if (controller != null)
        {
            if (controller.GetComponent<MainCharController>().IsGrounded())
            {
                controller.ChangeHealth(-1);
                notHitting = false;
            }
        }
    }
    void OnCollisionExit2D(Collision2D other)
    {
        MainCharController controller = other.gameObject.GetComponent<MainCharController>();
        if (controller != null)
        {
            notHitting = true;
        }
    }

    public void ChangeHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        Debug.Log(currentHealth);
    }
    IEnumerator Kill()
    {
        animator.SetTrigger("Die");

        //launch up
        MainCharController bounce = player.GetComponent<MainCharController>();
        if (bounce != null)
        {
            bounce.GoombaLaunch();
        }

        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        enabled = false;
        yield return new WaitForSeconds(4.5f);
        animator.SetTrigger("Revive");
        yield return new WaitForSeconds(0.8f);
        enabled = true;
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        gameObject.GetComponent<CircleCollider2D>().enabled = true;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;

    }
    IEnumerator flipLeft()
    {
        yield return new WaitForSeconds(0.2f);
        transform.localScale = new Vector2(-1f, 1f);
    }
    IEnumerator flipRight()
    {
        yield return new WaitForSeconds(0.2f);
        transform.localScale = new Vector2(1f, 1f);
    }
    IEnumerator noPlayerTrue()
    {
        yield return new WaitForSeconds(1);
        noplayer = true;
    }
    void Jump()
    {
        Vector2 movement = new Vector2(transform.localScale.x * jumpForcex, jumpForce);
        rigidbody2d.velocity = movement;
        flipAllowed = false;
    }
}