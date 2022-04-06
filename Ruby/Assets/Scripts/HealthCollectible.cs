using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    public float timeUntilRespawn = 10.0f;
    float currentTimer;
    public GameObject CollectibleHealth;
    bool generate = false;
    Rigidbody2D rigidbody2D;


    void OnTriggerEnter2D(Collider2D other)
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {
            //check if health is not already max
            if (controller.health < controller.maxHealth)
            {
                //give 1 hp
                controller.ChangeHealth(1);
                
                
                StartCoroutine(regenerate());
            }
        }

    }
    void Update()
    {
        
    }
    IEnumerator regenerate()
    {
        gameObject.GetComponent<Renderer>().enabled=false;
        Projectile e = other.collider.GetComponent<Projectile>();
        yield return new WaitForSeconds(2);
        gameObject.GetComponent<Renderer>().enabled = true;
    }
}

