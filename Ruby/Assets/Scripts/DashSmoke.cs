using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashSmoke : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(removeSmoke());
    }
    IEnumerator removeSmoke()
    {
        yield return new WaitForSeconds(1);     
        Destroy(gameObject);
    }
}
