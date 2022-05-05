using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    public float speed = 3.0f;
    public int maxHealth = 5;
    public float timeInvincible = 2.0f;
    public GameObject projectilePrefab;
    public GameObject projectilePrefab2;
    public GameObject smokePrefab;
    public float maxTimeProjectile;
    public float maxTimeProjectile2;
    public bool weaponOneTrue;
    public bool dash = false;

    AudioSource audioSource;
    public AudioClip throwCog;

    public int health { get { return currentHealth; } }
    int currentHealth;

    bool isInvincible;
    float invincibleTimer;
    float currentTimeProjectile;
    float currentTimeProjectile2;
    public float dashTime = 0.3f;
    public float currentDashTime;

    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;
    private float dashspeed = 7f;
    private Vector3 rubyPosition;
    public float dashCooldown = 3f;
    public float currentDashCooldown;
    public bool dashAllowed;
    public bool dashActive = false;

    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        transform.position = new Vector3(0, 0, 0);

        currentHealth = maxHealth;
        dashAllowed = true;

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        //create vector 2 in charge of moving
        Vector2 move = new Vector2(horizontal, vertical);
        //check to see if character is moving
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            //look in direction of move vector
            lookDirection.Set(move.x, move.y);
            //set length as 1
            lookDirection.Normalize();
        }
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);
        if (isInvincible)
        {
            //start counting down
            invincibleTimer -= Time.deltaTime;
            //check if the timer of 2.0f is done
            if (invincibleTimer < 0)
                //remove invincibility
                isInvincible = false;
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            weaponOneTrue = !weaponOneTrue;
        }

        if (dashActive == false)
        {
            if (Input.GetKey(KeyCode.I))
            {
                if (weaponOneTrue == true)
                {
                    //check if timer is less than 1
                    if (currentTimeProjectile < 1)
                    {
                        //set launch timer to 2 
                        currentTimeProjectile = maxTimeProjectile;
                        Launch();
                    }
                    currentTimeProjectile -= Time.deltaTime;
                }
            }
            if (Input.GetKey(KeyCode.I))
            {
                if (weaponOneTrue == false)
                {
                    if (currentTimeProjectile2 < 1.15)
                    {
                        currentTimeProjectile2 = maxTimeProjectile2;
                        Launch();
                    }
                    currentTimeProjectile2 -= Time.deltaTime;
                }
            }
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
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (dashAllowed)
                {
                    GameObject smokeObject = Instantiate(smokePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
                }
                if (dash == true)
                {
                    return;
                }
                dash = true;
                currentDashCooldown = dashCooldown;
                currentDashTime = dashTime;
                dashAllowed = false;
            }
            if (dash == true)
            {
                if (currentDashTime > 0)
                {
                    //dash
                    float horizontal = Input.GetAxis("Horizontal");
                    float vertical = Input.GetAxis("Vertical");
                    Vector2 position = transform.position;
                    //Vector3 dashtranslate = new Vector3(0.01f, 0f, 0f);
                    if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
                    {
                        position.x = position.x + dashspeed * horizontal * Time.deltaTime;
                        position.y = position.y + dashspeed * vertical * Time.deltaTime;
                    }
                    /*if(lookDirection.x>0)
                    dash in platformer
                    {
                    transform.Translate(dashtranslate);
                    }
                    if(lookDirection.x<0)
                    {
                        transform.Translate(-dashtranslate);
                    }
                    */
                    transform.position = position;
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
        }
        if (currentDashTime > 0)
        {
            dashActive = true;
        }
        else
        {
            dashActive = false;
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                JambiScript character = hit.collider.GetComponent<JambiScript>();
                if (character != null)
                {
                    character.DisplayDialog();
                }
            }
        }
    }
    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            animator.SetTrigger("Hit");
            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }
    void Launch()
    {
        if (!isInvincible)
        {
            if (!dashActive)
            {
                audioSource.PlayOneShot(throwCog);
                if (weaponOneTrue)
                {
                    GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
                    Projectile projectile = projectileObject.GetComponent<Projectile>();
                    projectile.Launch(lookDirection, 400);

                    animator.SetTrigger("Launch");
                }
                if (!weaponOneTrue)
                {
                    if (lookDirection.y == 0)
                    {
                        GameObject projectileObject2lower = Instantiate(projectilePrefab2, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
                        Projectile2 projectile2 = projectileObject2lower.GetComponent<Projectile2>();
                        projectile2.Launch(lookDirection + Vector2.down * 0.5f, 300);

                        GameObject projectileObject2mid = Instantiate(projectilePrefab2, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
                        Projectile2 projectile2mid = projectileObject2mid.GetComponent<Projectile2>();
                        projectile2mid.Launch(lookDirection, 300);

                        GameObject projectileObject2higher = Instantiate(projectilePrefab2, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
                        Projectile2 projectile2high = projectileObject2higher.GetComponent<Projectile2>();
                        projectile2high.Launch(lookDirection + Vector2.up * 0.5f, 300);

                        animator.SetTrigger("Launch");
                    }
                    if (lookDirection.x == 0)
                    {
                        GameObject projectileObject2left = Instantiate(projectilePrefab2, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
                        Projectile2 projectile2 = projectileObject2left.GetComponent<Projectile2>();
                        projectile2.Launch(lookDirection + Vector2.left * 0.5f, 300);

                        GameObject projectileObject2middle = Instantiate(projectilePrefab2, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
                        Projectile2 projectile2middle = projectileObject2middle.GetComponent<Projectile2>();
                        projectile2middle.Launch(lookDirection, 300);

                        GameObject projectileObject2rights = Instantiate(projectilePrefab2, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
                        Projectile2 projectile2right = projectileObject2rights.GetComponent<Projectile2>();
                        projectile2right.Launch(lookDirection + Vector2.right * 0.5f, 300);

                        animator.SetTrigger("Launch");
                    }
                }
            }
        }
    }
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

}