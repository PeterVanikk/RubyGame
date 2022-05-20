using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAI : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    Animator animator;

    public float speed;
    public float distance;
    public bool alive;

    public bool noplayer;
    public Transform groundDetection;
    public Transform eyes;
    public Transform backeyes;
    public GameObject bulletPrefab;
    public Transform shootPoint;

    //shoot
    public float currentShootCooldown;
    public float maxShootCooldown = 2f;

    Vector2 lookDirection = new Vector2(1, 0);

    //health
    public int health { get { return currentHealth; } }
    int currentHealth;
    public int maxHealth = 5;
    void Start()
    {
        lookDirection.Set(1f, 0f);
        rigidbody2d = GetComponent<Rigidbody2D>();
        GameObject player = GameObject.Find("MainCharacter");
        currentHealth = maxHealth;
    }
    void Update()
    {
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
        RaycastHit2D playerchk = Physics2D.Raycast(eyes.position, lookDirection, 6.5f, LayerMask.GetMask("Player"));
        RaycastHit2D playerbehindchk = Physics2D.Raycast(backeyes.position, -lookDirection, 3.5f, LayerMask.GetMask("Player"));
        if (playerchk.collider != null)
        {
            noplayer = false;
        }
        if (playerbehindchk.collider != null)
        {
            noplayer = false;
            transform.localScale = new Vector2(-transform.localScale.x, 1f);
        }
        if (playerbehindchk.collider == null && playerchk.collider == null)
        {
            noplayer = true;
        }
        if (noplayer == false)
        {
            if(currentShootCooldown < 0)
            {
                currentShootCooldown = maxShootCooldown;
                Launch();
            }
            currentShootCooldown -= Time.deltaTime;
        }
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
    void Launch()
    {
        GameObject projectileObject = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        SkellyArrowScript projectile = projectileObject.GetComponent<SkellyArrowScript>();
        if(transform.localScale.x < 0)
        {
            projectile.ShootLeft();
        }
        if(transform.localScale.x > 0)
        {
            projectile.ShootRight();
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
}
