using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bouncingObjects : MonoBehaviour
{
    Rigidbody2D rb2d;
    public Vector3 startForce;
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.left * 1.7f * Time.fixedDeltaTime);
        if(rb2d.position.x <= 64.12)
        {
            Destroy(gameObject);
        }
    }
    void OnCollisionStay2D(Collision2D other)
    {
        MainCharController controller = other.gameObject.GetComponent<MainCharController>();
        if (controller != null)
        {
            if (controller.GetComponent<MainCharController>().IsGrounded())
            {
                controller.ChangeHealth(-1);
            }
        }
    }
}
