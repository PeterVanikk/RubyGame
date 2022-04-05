using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    public float speed = 3.0f;
    public int maxHealth = 5;
    public float timeInvincible = 2.0f;
    public GameObject projectilePrefab;

    public int health { get { return currentHealth; }}
    int currentHealth;

    bool isInvincible;
    float invincibleTimer;

    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;

    Animator animator;
    Vector2 lookDirection = new Vector2(1,0);
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        //create vector 2 in charge of moving
        Vector2 move = new Vector2(horizontal, vertical);
        //check to see if character is moving
        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
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
        if(Input.GetKeyDown(KeyCode.C))
        {
            Launch();
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
        Debug.Log("Ruby's Health is now" + currentHealth + "/" + maxHealth);
    }
    void Launch()
    {
        //create the Instantiate at the position of ruby's rigidbody, but a bit up so that it starts near her hands
        //hence rigidbody2d.posision + (Vector2.up * 0.5f) Quaternion meaning no rotation
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        //new projectile called projectile = (the humungous line above which is also assigned to the projectile we put in the ruby inspector)
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        //call the launch function and execute with the direction that you are facting with a force of 300N
        //300 could have been substituted for a public float variable and use that as force
        projectile.Launch(lookDirection, 300);
        //play "Launch" animation
        animator.SetTrigger("Launch");
    }
}

