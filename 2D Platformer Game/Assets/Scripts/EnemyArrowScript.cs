using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArrowScript : MonoBehaviour
{
    Rigidbody2D rb;
    public LayerMask groundLayers;

    Rigidbody2D rigidbody2d;
    float projectileTimer = 2.0f;
    float currentProjectileTime;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
        if (other.collider.tag == "player")
        {
            MainCharController player = other.collider.GetComponent<MainCharController>();
            player.ChangeHealth(-1);
        }
        if (other.collider.tag != "platform")
        {
            Destroy(gameObject);
        }
    }
}
