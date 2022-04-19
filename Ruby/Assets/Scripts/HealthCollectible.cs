using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
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
        gameObject.GetComponent<BoxCollider2D>().enabled=false;
        gameObject.GetComponent<Renderer>().enabled = false;
        yield return new WaitForSeconds(5);
        gameObject.GetComponent<Renderer>().enabled = true;
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }
}

