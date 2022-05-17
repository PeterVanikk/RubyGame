using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAI : MonoBehaviour
{
    Rigidbody2D rigidbody2d;

    public float speed;
    public float distance;

    public bool noplayer;
    public Transform groundDetection;
    public Transform eyes;
    public Transform backeyes;

    Vector2 lookDirection = new Vector2(1, 0);
    void Start()
    {
        lookDirection.Set(1f, 0f);
        rigidbody2d = GetComponent<Rigidbody2D>();
        GameObject player = GameObject.Find("MainCharacter");
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
    }
    void OnTriggerStay2D(Collider2D other)
    {
        MainCharController controller = other.GetComponent<MainCharController>();
        if (controller != null)
        {
            lookDirection.Set(1f, 0f);
            controller.ChangeHealth(-1);
        }
    }
}
