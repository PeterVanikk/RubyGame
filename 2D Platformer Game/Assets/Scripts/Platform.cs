using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    Rigidbody2D rigidbody2D;
    void onTriggerEnter2D(Collider2D other)
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        MainCharController controller = other.GetComponent<MainCharController>();
        if(controller != null)
        {
            if(controller.JumpStage()==true)
            {
                gameObject.GetComponent<BoxCollider>().enabled = false;
            }
                gameObject.GetComponent<BoxCollider>().enabled = true;
        }
    }
}
