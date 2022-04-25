using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JambiScript : MonoBehaviour
{
    public GameObject dialogbox;
    public GameObject dialogboxThanks;
    float timerDisplay;
    public float displayTime = 4.0f;
    void Start()
    {
        dialogboxThanks.SetActive(false);
        dialogbox.SetActive(false);
        timerDisplay = -1.0f;
        EnemyController controller = GetComponent<EnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timerDisplay >= 0)
        {
            timerDisplay -= Time.deltaTime;
            if (timerDisplay < 0)
            {
                dialogbox.SetActive(false);
                dialogboxThanks.SetActive(false);
            }
        }
    }
    public void DisplayDialog()
    {
        timerDisplay = displayTime;
        if (GameObject.Find("enemy").GetComponent<EnemyController>().broken)
        {
            dialogbox.SetActive(true);
        }
        else
        {
            dialogboxThanks.SetActive(true);
        }
    }
}

