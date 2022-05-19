using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkellyArrowScript : MonoBehaviour
{
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void Shoot(Vector2 direction, float force)
    {
        Debug.Log("shootrunning");
        rb.AddForce(direction * force);
    }
}
