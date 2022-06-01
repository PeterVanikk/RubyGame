using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SpiderBehaviour : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    Animator animator;

    public float speed;
    public float distance;
    public bool alive;
    public float range;
    private float distToPlayer;

    public bool noplayer;
    public Transform groundDetection;
    public Transform player;

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
        if (noplayer)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
        distToPlayer = Vector2.Distance(transform.position, player.position);
        if (distToPlayer <= range && player.position.y >= transform.position.y - 1.0f)
        {
            //noplayer = false;
            if (player.position.x > transform.position.x && transform.localScale.x < 0)
            {
                transform.localScale = new Vector2(1f, 1f);
            }
            if (player.position.x < transform.position.x && transform.localScale.x > 0)
            {
                transform.localScale = new Vector2(-1f, 1f);
            }
        }
        else
        {
            noplayer = true;
        }
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        lookDirection.Set(transform.localScale.x, 0.0f);
        if (noplayer == true)
        {
            transform.Translate(lookDirection * speed * Time.deltaTime);
        }

        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distance);
        if (groundInfo.collider == false)
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
        if (currentHealth <= 0)
        {
            if (alive == true)
            {
                StartCoroutine(Kill());
                alive = false;
            }
        }
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
            }
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
        yield return new WaitForSeconds(6);
        gameObject.SetActive(false);
    }
}