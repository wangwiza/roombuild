using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteractable : MonoBehaviour
{
    private Rigidbody objectRigidbody;

    private void Awake()
    {
        objectRigidbody = GetComponent<Rigidbody>();
    }
}
