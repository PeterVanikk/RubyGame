using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    private PlatformEffector2D effector;

    public bool movingRight = true;
    public float speed;
    public float timeUntilFlip;
    public float currentTime;

    //for stick to platform
    public GameObject player;


    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentTime = timeUntilFlip;
        effector = GetComponent<PlatformEffector2D>();
    }

    void Update()
    {
        if (movingRight)
        {
            transform.Translate(speed * Vector2.right * Time.deltaTime);
            currentTime -= Time.deltaTime;
        }
        if (!movingRight)
        {
            transform.Translate(speed * Vector2.left * Time.deltaTime);
            currentTime -= Time.deltaTime;
        }
        if (currentTime <= 0)
        {
            currentTime = timeUntilFlip;
            movingRight = !movingRight;
        }

        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.Space))
        {
            StartCoroutine(Drop());
        }
    }
    IEnumerator Drop()
    {
        effector.rotationalOffset = 180f;
        yield return new WaitForSeconds(0.2f);
        effector.rotationalOffset = 0f;
    }
    public void OnCollisionStay2D(Collision2D other)
    {
        MainCharController controller = other.gameObject.GetComponent<MainCharController>();
        if (controller != null)
        {
            player.transform.SetParent(this.transform);
        }
    }
    public void OnCollisionExit2D(Collision2D other)
    {
        player.transform.parent = null;
    }
}
