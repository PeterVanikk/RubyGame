using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAI : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    Animator animator;

    public float speed, shootSpeed;
    public float distance;
    public bool alive;
    public float range;
    private float distToPlayer;
    public float timeBTWShots = 2.0f;
    private bool canShoot;

    public bool noplayer;
    public Transform groundDetection;
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public Transform player;

    //shoot
    public float currentShootCooldown;
    public float maxShootCooldown = 2f;
    public float timeToLaunch = 0f;
    Vector2 lookDirection = new Vector2(1, 0);

    //health
    public int health { get { return currentHealth; } }
    int currentHealth;
    public int maxHealth = 5;
    void Start()
    {
        animator = GetComponent<Animator>();
        canShoot = true;
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
            noplayer = false;
            if (player.position.x > transform.position.x && transform.localScale.x < 0)
            {
                transform.localScale = new Vector2(1f, 1f);
            }
            if (player.position.x < transform.position.x && transform.localScale.x > 0)
            {
                transform.localScale = new Vector2(-1f, 1f);
            }
            if (canShoot)
            {
                StartCoroutine(Shoot());
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
        Debug.Log(lookDirection);
        if (currentHealth <= 0)
        {
            StartCoroutine(Kill());
        }
    }
    void OnCollisionStay2D(Collision2D other)
    {
        MainCharController controller = other.gameObject.GetComponent<MainCharController>();
        if (controller != null)
        {
            controller.ChangeHealth(-1);
        }
    }

    public void ChangeHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        Debug.Log(currentHealth);
    }
    IEnumerator Kill()
    {
        alive = false;
        //animator.SetTrigger("Dead");
        yield return new WaitForSeconds(0.5f);
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.GetComponent<Renderer>().enabled = false;
    }
    IEnumerator Shoot()
    {
        canShoot = false;
        yield return new WaitForSeconds(timeBTWShots);
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.8f);
        if (noplayer == false)
        {
            GameObject projectileObject = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
            projectileObject.GetComponent<Rigidbody2D>().velocity = new Vector2(shootSpeed * transform.localScale.x * Time.fixedDeltaTime, 0f);
        }
        canShoot = true;
    }
}
