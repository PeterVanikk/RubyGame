using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    float projectileTimer;
    //awake because unity does not run "start" if the gameobject was first created.
    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        projectileTimer -= Time.deltaTime;
        if(projectileTimer<0)
        {
            Destroy(gameObject);
        }
    }
    public void Launch(Vector2 direction, float force)
    {
        //add force with direction
        rigidbody2d.AddForce(direction * force);
        projectileTimer = 2.0f;
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        //check to see if it collided with enemy (has enemyController script) and call it "e"
        EnemyController e = other.collider.GetComponent<EnemyController>();
        //check if it has the script
        if (e != null)
        {
            e.Fix();
        }
        Destroy(gameObject);
        //add a debug so that we know which gameobject we collided with
        Debug.Log("Projectile Collision with " + other.gameObject);
        //destroy it (we know it is this gameobject because our function is
        //OnCollisionEnter
    }
}
