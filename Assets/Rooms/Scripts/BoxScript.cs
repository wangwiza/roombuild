using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : MonoBehaviour
{
    float boxLifeTime = 7.5f;

    private void Awake()
    {
        StartCoroutine(RemoveObject());
    }

    IEnumerator RemoveObject()
    {
        yield return new WaitForSeconds(boxLifeTime);
        Object.Destroy(this.gameObject);
    }
}
