using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSideSide : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    private PlatformEffector2D effector;

    public bool movingRight = true;
    public float speedx;
    public float timeUntilFlipx;
    public float currentTimex;

    //for stick to platform


    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentTimex = timeUntilFlipx;
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
        /*if (oscillateY)
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
        }*/
    }
    public void OnCollisionStay2D(Collision2D other)
    {
        MainCharController controller = other.gameObject.GetComponent<MainCharController>();
        if (controller != null)
        {
            GameObject character = GameObject.FindWithTag("player");
            character.transform.SetParent(this.transform);
            /*oscillateY = false;
            if (rigidbody2d.position.y > minHeight && other.gameObject.GetComponent<MainCharController>().IsGrounded())
            {
                transform.Translate(dropSpeed * Vector2.down * Time.deltaTime);
            }*/
        }
    }
    public void OnCollisionExit2D(Collision2D other)
    {
        GameObject character = GameObject.FindWithTag("player");
        character.transform.parent = null;
        // oscillateY = true;
    }
}
