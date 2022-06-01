using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    private PlatformEffector2D effector;

    public bool movingRight = true;
    public float speedx;
    public float speedy;
    public float timeUntilFlipx;
    public float currentTimex;
    public bool oscillateY = true;
    public float maxHeight;
    public float minHeight;

    //for stick to platform
    public GameObject player;


    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentTimex = timeUntilFlipx;
        effector = GetComponent<PlatformEffector2D>();
    }

    void Update()
    {
        if (movingRight)
        {
            transform.Translate(speedx * Vector2.right * Time.deltaTime);
            currentTimex -= Time.deltaTime;
        }
        if (!movingRight)
        {
            transform.Translate(speedx * Vector2.left * Time.deltaTime);
            currentTimex -= Time.deltaTime;
        }
        if (currentTimex <= 0)
        {
            currentTimex = timeUntilFlipx;
            movingRight = !movingRight;
        }
        if (oscillateY)
        {
            //maxHeight = rigidbody2d.position.y + Vector2.up * 0.5f;
        }

        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.Space))
        {
            StartCoroutine(Drop());
        }
    }
    IEnumerator Drop()
    {
        effector.rotationalOffset = 180f;
        yield return new WaitForSeconds(0.4f);
        effector.rotationalOffset = 0f;
    }
    public void OnCollisionStay2D(Collision2D other)
    {
        MainCharController controller = other.gameObject.GetComponent<MainCharController>();
        if (controller != null)
        {
            player.transform.SetParent(this.transform);
            oscillateY = false;
        }
    }
    public void OnCollisionEnter2D(Collision2D other)
    {
        MainCharController controller = other.gameObject.GetComponent<MainCharController>();
        if (controller != null)
        {
            StartCoroutine(Fall());
        }
    }
    public void OnCollisionExit2D(Collision2D other)
    {
        player.transform.parent = null;
        oscillateY = true;
    }
    IEnumerator Fall()
    {
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        yield return new WaitForSeconds(0.1f);
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
    }
}
