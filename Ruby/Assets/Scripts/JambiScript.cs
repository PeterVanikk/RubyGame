using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JambiScript : MonoBehaviour
{
    public GameObject dialogbox;
    float timerDisplay;
    public float displayTime = 4.0f;
    void Start()
    {
        dialogbox.SetActive(false);
        timerDisplay = -1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(timerDisplay >=0)
        {
            timerDisplay -= Time.deltaTime;
            if(timerDisplay < 0)
            {
                dialogbox.SetActive(false);
            }
        }
    }
    public void DisplayDialog()
    {
        timerDisplay = displayTime;
        dialogbox.SetActive(true);
    }
}

