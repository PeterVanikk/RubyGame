using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rb;
    Rigidbody2D rigidbody2d;

    public LayerMask groundLayers;
    float projectileTimer = 2.0f;
    float currentProjectileTime;
    // Start is called before the first frame update
    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        projectileTimer -= Time.deltaTime;
        if (projectileTimer < 0)
        {
            Destroy(gameObject);
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "enemy")
        {
            BoarBehaviour bcontroller = other.gameObject.GetComponent<BoarBehaviour>();
            if (bcontroller != null)
            {
                bcontroller.StartCoroutine(bcontroller.DieProcess());
            }
            SkeletonAI scontroller = other.gameObject.GetComponent<SkeletonAI>();
            if (scontroller != null)
            {
                scontroller.ChangeHealth(-1);
            }
            Destroy(gameObject);
        }
        if (other.collider.tag != "platform")
        {
            Destroy(gameObject);
        }
    }
}
