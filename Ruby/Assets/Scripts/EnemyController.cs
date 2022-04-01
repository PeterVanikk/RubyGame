using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public bool vertical;
    public float changeTime = 2.0f;

    Rigidbody2D rigidbody2D;
    float timer;
    int direction = 1;

    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        //set timer to 3.0f
        timer = changeTime;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //start counting down
        timer -= Time.deltaTime;
        //if timer finishes
        if (timer < 0)
        {
            //switch directions
            direction = -direction;
            //reset timer
            timer = changeTime;
        }
    }
    void FixedUpdate()
    {
        Vector2 position = rigidbody2D.position;

        if (vertical)
        {
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
            //set posotion x to (its origional position + ((t)(v))  )
            //multiply speed by direction because direction will be 1 or -1
            position.y = position.y + Time.deltaTime * speed * direction;
            
        }
        else
        {
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
            position.x = position.x + Time.deltaTime * speed * direction;
        }

        rigidbody2D.MovePosition(position);
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        //get component rubycontroller (player)
        RubyController player = other.gameObject.GetComponent<RubyController>();
        //check to see if it is the player
        if (player != null)
        {
            //remove 1 hp
            player.ChangeHealth(-1);
        }
    }
}

