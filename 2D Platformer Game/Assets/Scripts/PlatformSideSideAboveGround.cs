using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSideSideAboveGround : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    private PlatformEffector2D effector;

    public bool movingRight = true;
    public float speedx;
    public float timeUntilFlipx;
    public float currentTimex;
    public float dropSpeed;
    public bool countDown;
    public float maxTimeUntilDrop;
    public float currentTimeUntilDrop;
    public float idleHeight;
    public bool oscillateX;

    public GameObject player;
    void Start()
    {
        oscillateX = true;
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentTimex = timeUntilFlipx;
        idleHeight = rigidbody2d.position.y;
    }

    void Update()
    {
        if (oscillateX)
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
        }
        if (countDown)
        {
            currentTimeUntilDrop -= Time.deltaTime;
            if (currentTimeUntilDrop <= 0)
            {
                StartCoroutine(Fall());
                oscillateX = false;
            }
        }
    IEnumerator Drop()
    {
        effector.rotationalOffset = 180f;
        yield return new WaitForSeconds(0.4f);
        effector.rotationalOffset = 0f;
        }
        if(rigidbody2d.position.y <= -7.5)
        {
            countDown = false;
        }
    }
    public void OnCollisionEnter2D(Collision2D other)
    {
        MainCharController controller = other.gameObject.GetComponent<MainCharController>();
        if (controller == null)
        {
            return;
        }
    }
    public void OnCollisionStay2D(Collision2D other)
    {
        MainCharController controller = other.gameObject.GetComponent<MainCharController>();
        if (controller == null)
        {
            return;
        }
        player.transform.SetParent(this.transform);
    }
    public void OnCollisionExit2D(Collision2D other)
    {
        MainCharController controller = other.gameObject.GetComponent<MainCharController>();
        if (controller == null)
        {
            return;
        }
        player.transform.SetParent(null);
        if (rigidbody2d.position.y >= idleHeight-0.5f)
        {
            oscillateX = true;
            countDown = false;
        }
    }
    IEnumerator Fall()
    {
        oscillateX = false;
        if (rigidbody2d.position.y > idleHeight - 7f)
        {
            transform.Translate(dropSpeed * 1.5f * Vector2.down * Time.fixedDeltaTime);
        }
        yield return new WaitForSeconds(3);
        if (rigidbody2d.position.y <= idleHeight)
        {
            transform.Translate(dropSpeed * 1.5f * Vector2.up * Time.fixedDeltaTime);
        }
        if (rigidbody2d.position.y >= idleHeight-0.5f)
        {
            oscillateX = true;
        }
    }
}
