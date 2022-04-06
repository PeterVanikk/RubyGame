using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    public float timeUntilRespawn = 2.0f;
    float currentTimer;
    public GameObject CollectibleHealth;


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
                currentTimer = timeUntilRespawn;
            }
        }

        void Update() 
        {
                currentTimer -= Time.deltaTime;

            if(currentTimer < 0)
            {
                GameObject healthObject = Instantiate(CollectibleHealth, transform.position , transform.rotation);
                Debug.Log("Timer is done");
            }
        }
    }
}
