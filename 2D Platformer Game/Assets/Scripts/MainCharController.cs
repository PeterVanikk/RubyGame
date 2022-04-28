using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharController : MonoBehaviour
{
    public float speed = 3.0f;

    bool isInvincible;
    float invincibleTimer;

    Rigidbody2D rigidbody2d;

    float horizontal;
    float vertical;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        if (horizontal > 0)
        {
            transform.localScale = new Vector2(1f, 1f);
        }
        if (horizontal < 0)
        {
            transform.localScale = new Vector2(-1f, 1f);
        }
        if (!Mathf.Approximately(move.x, 0.0f))
        {
            animator.SetBool("IsRunning", true);
        }
        if(Mathf.Approximately(move.x, 0.0f))
        {
            animator.SetBool("IsRunning", false);
        }
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }
}
