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
    public bool oscillateX;

    public GameObject player;
    void Start()
    {
        oscillateX = true;
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentTimex = timeUntilFlipx;
        effector = GetComponent<PlatformEffector2D>();
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
            if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.Space))
            {
                StartCoroutine(Drop());
            }
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
        }
    }
    public void OnCollisionExit2D(Collision2D other)
    {
        MainCharController controller = other.gameObject.GetComponent<MainCharController>();
        if (controller != null)
        {
            player.transform.SetParent(null);
        }
    }
}
