using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonIdle : MonoBehaviour
{
    public float speed;
    public float distance;

    public bool movingRight = true;

    public Transform groundDetection;
    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distance);
        if(groundInfo.collider == false)
        {
            if(movingRight == true)
            {
                transform.localScale = new Vector2(-1f, 1f);
                transform.Translate(Vector2.left * speed * Time.deltaTime);
                movingRight = false;
            }
            if(movingRight == false)
            {
                transform.localScale = new Vector2(1f, 1f);
                transform.Translate(Vector2.right * speed * Time.deltaTime);
                movingRight = true;
            }
        }
    }
}
