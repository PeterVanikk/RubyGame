using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformUpDown : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    private PlatformEffector2D effector;

    public bool movingUp = true;
    public float speedy;
    public float currentTimex;
    public bool oscillateY = true;
    public float maxHeight;
    public float minHeight;
    public float dropSpeed;
    public bool countDown;
    public float maxTimeUntilDrop;
    public float currentTimeUntilDrop;

    public GameObject player;
    void Start()
    {
        countDown = false;
        rigidbody2d = GetComponent<Rigidbody2D>();
        maxHeight = rigidbody2d.position.y + 0.22f;
        minHeight = rigidbody2d.position.y - 0.22f;
    }
    void Update()
    {
        /*if (movingRight)
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
        }*/
        if (oscillateY)
        {
            if (rigidbody2d.position.y > maxHeight)
            {
                movingUp = false;
            }
            if (rigidbody2d.position.y < minHeight)
            {
                movingUp = true;
            }
            if (movingUp)
            {
                transform.Translate(speedy * Vector2.up * Time.deltaTime);
            }
            else
            {
                transform.Translate(speedy * Vector2.down * Time.deltaTime);
            }
        }
        if (countDown)
        {
            currentTimeUntilDrop -= Time.deltaTime;
            if (currentTimeUntilDrop <= 0)
            {
                StartCoroutine(Fall());
            }
        }
        /*if (raiseNeeded)
        {
            if (rigidbody2d.position.x < maxHeight - 0.22f)
            {
                Debug.Log("dwa");
                transform.Translate(dropSpeed * 2 * Vector2.up * Time.fixedDeltaTime);
            }
        }
        if (rigidbody2d.position.x <= maxHeight - 0.22f)
        {
            raiseNeeded = false;
        }
        if (dropNeeded)
        {
            if (rigidbody2d.position.x >= -8f)
            {
                transform.Translate(dropSpeed * 2 * Vector2.down * Time.fixedDeltaTime);
            }
        }*/
        if (rigidbody2d.position.y <= -7.5)
        {
            countDown = false;
        }
    }
    /*
        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.Space))
        {
            StartCoroutine(Drop());
        }
    IEnumerator Drop()
    {
        effector.rotationalOffset = 180f;
        yield return new WaitForSeconds(0.4f);
        effector.rotationalOffset = 0f;
    }
    */
    public void OnCollisionEnter2D(Collision2D other)
    {
        MainCharController controller = other.gameObject.GetComponent<MainCharController>();
        if (controller == null)
        {
            return;
        }
        countDown = true;
        currentTimeUntilDrop = maxTimeUntilDrop;
    }
    public void OnCollisionStay2D(Collision2D other)
    {
        MainCharController controller = other.gameObject.GetComponent<MainCharController>();
        if (controller == null)
        {
            return;
        }
        player.transform.SetParent(this.transform);
        oscillateY = false;
        if (rigidbody2d.position.y > minHeight && other.gameObject.GetComponent<MainCharController>().IsGrounded())
        {
            transform.Translate(dropSpeed * Vector2.down * Time.deltaTime);
        }
    }
    public void OnCollisionExit2D(Collision2D other)
    {
        MainCharController controller = other.gameObject.GetComponent<MainCharController>();
        if (controller == null)
        {
            return;
        }
        player.transform.SetParent(null);
        if (rigidbody2d.position.y >= minHeight - 0.2f)
        {
            oscillateY = true;
            countDown = false;
        }
    }
    IEnumerator Fall()
    {
        if (rigidbody2d.position.y > minHeight - 7f)
        {
            transform.Translate(dropSpeed * 1.5f * Vector2.down * Time.fixedDeltaTime);
        }
        yield return new WaitForSeconds(3);
        if (rigidbody2d.position.y <= maxHeight - 0.22f)
        {
            transform.Translate(dropSpeed * 1.5f * Vector2.up * Time.fixedDeltaTime);
        }
        if (rigidbody2d.position.y >= minHeight - 0.22f)
        {
            oscillateY = true;
        }
    }
}
