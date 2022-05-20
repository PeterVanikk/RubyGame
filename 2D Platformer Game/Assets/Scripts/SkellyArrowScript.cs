using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkellyArrowScript : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void ShootLeft()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }
    public void ShootRight()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }
}
