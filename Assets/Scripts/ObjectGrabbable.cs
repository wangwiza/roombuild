using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGrabbable : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed;
    private Rigidbody objectRigidbody;
    private Transform objectGrabPointTransform;

    private void Awake() {
        objectRigidbody = GetComponent<Rigidbody>();
    }
    public void Grab(Transform objectGrabPointTransform) {
        this.objectGrabPointTransform = objectGrabPointTransform;
        objectRigidbody.useGravity = false;
        objectRigidbody.drag = 10f;
        objectRigidbody.isKinematic = true;
    }

    public void Drop() {
        this.objectGrabPointTransform = null;
        objectRigidbody.useGravity = true;
        objectRigidbody.drag = 0;
        objectRigidbody.isKinematic = false;
    }

    public void Rotate(float angle) {
        // Calculate the target rotation (90 degrees clockwise)
        Quaternion targetRotation = transform.rotation * Quaternion.Euler(0f, 0f, angle);

        // Smoothly interpolate between the current rotation and the target rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    private void FixedUpdate() {
        if (objectGrabPointTransform != null) {
            Debug.Log("lerping");
            float lerpSpeed = 10f;
            Vector3 newPosition = Vector3.Lerp(transform.position, objectGrabPointTransform.position, Time.deltaTime * lerpSpeed);
            objectRigidbody.MovePosition(newPosition);
        }
    }

}
