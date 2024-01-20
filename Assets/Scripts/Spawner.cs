using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject box;
    public GameObject item;

    Queue<GameObject> boxList;
    bool isItemExist = false;

    float timePassed = 0f;
    int timeTrigger = 2;
    public int randomLowRange = 2;
    public int randomHighRange = 5;


    public void Start()
    {
        boxList = new Queue<GameObject>();
        SpawnBox();
    }

    public void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed > timeTrigger)
        {
            SpawnBox();
            timePassed = 0f;
            timeTrigger = Random.Range(randomLowRange, randomHighRange);
        }
    }

    public void SpawnBox()
    {
        if (boxList == null) return;
        if (boxList.Count > 5) return;

        boxList.Enqueue(Instantiate(box));
    }

    public void ConvertBoxToItem()
    {
        Debug.Log("CHECK");
        if (isItemExist || boxList.Count == 0) return;
        Debug.Log("CONVERT");
        GameObject temp = boxList.Dequeue();
        Object.Destroy(temp);
        Instantiate(item);
        isItemExist = true;
    }

    public void PickUpSpawnedItem()
    {
        isItemExist = false;
    }
}
