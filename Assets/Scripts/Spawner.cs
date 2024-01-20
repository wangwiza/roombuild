using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject mySphere;
    float timePassed = 0f;
    int timeTrigger = 2;
    public int randomLowRange = 2;
    public int randomHighRange = 5;

    public void Start()
    {
        SpawnSphere();
    }

    public void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed > timeTrigger)
        {
            SpawnSphere();
            Debug.Log("Time Trigger: " + timeTrigger);
            timePassed = 0f;
            timeTrigger = Random.Range(randomLowRange, randomHighRange);
        }
    }

    public void SpawnSphere()
    {
        Instantiate(mySphere);
    }
}
