using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    public float timeUntilRespawn = 1.0f;
    public float currentTimer;
    public GameObject CollectibleHealth;

    void Start()
    {
        currentTimer = timeUntilRespawn;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {
            //check if health is not already max
            if (controller.health < controller.maxHealth)
            {
                //give 1 hp
                controller.ChangeHealth(1);
                Destroy(gameObject);
            currentTimer -= Time.deltaTime;
            }
        }
            if(currentTimer < 0)
            {
                GameObject healthObject = Instantiate(CollectibleHealth, transform.position , transform.rotation);
            }
    }
}
