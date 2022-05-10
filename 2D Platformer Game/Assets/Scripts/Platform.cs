using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private PlatformEffector2D effector;
    void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.Space))
        {
            StartCoroutine(Drop());
        }
    }
    IEnumerator Drop()
    {
        effector.rotationalOffset = 180f;
        yield return new WaitForSeconds(0.2f);
        effector.rotationalOffset = 0f;
    }
}
