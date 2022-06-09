using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarBehaviour : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    Animator animator;

    Vector2 lookDirection = new Vector2(1, 0);
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        lookDirection.Set(1f, 0f);
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator DieProcess()
    {
        animator.SetTrigger("Die");
        yield return new WaitForSeconds(5f);
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        enabled = false;
        yield return new WaitForSeconds(4.5f);
        Destroy(gameObject);
    }
    public void OnCollisionStay2D(Collider2D other)
    {
        MainCharController controller = other.gameObject.GetComponent<MainCharController>();
        if (controller == null)
        {
            controller.ChangeHealth(-1);
        }
    }
}
