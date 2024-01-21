using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public float timeEnd = 10f;
    float timePassed = 0f;
    public GameObject endScreen;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed > timeEnd)
        {
            endScreen.SetActive(true);
        }
    }
}
