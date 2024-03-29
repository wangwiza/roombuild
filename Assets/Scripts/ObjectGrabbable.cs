using UnityEngine;

public class ObjectGrabbable : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed;
    private Rigidbody objectRigidbody;
    private Transform objectGrabPointTransform;
    private bool isRotating;
    private Quaternion targetRotation;

    [SerializeField] private Mesh[] allMeshes;
    [SerializeField] private MeshFilter currentMesh;
    [SerializeField] private AudioSource pickUpSource;
    [SerializeField] private AudioSource dropSource;
    [SerializeField] private AudioSource rotateSource;
    

    private void Start()
    {
        
        int chosenMesh = Random.Range(0, allMeshes.Length);
        Mesh tempMesh = allMeshes[chosenMesh];
        currentMesh.mesh = tempMesh;
        // this.gameObject.transform.localScale = new Vector3(1,1,1);
        this.gameObject.AddComponent<BoxCollider>();
    }

    private void Awake()
    {
        objectRigidbody = GetComponent<Rigidbody>();
    }
    public void Grab(Transform objectGrabPointTransform)
    {
        pickUpSource.Play();
        Physics.IgnoreCollision(objectGrabPointTransform.parent.GetComponent<Collider>(), GetComponent<Collider>());
        this.rotationSpeed = 5f;
        this.objectGrabPointTransform = objectGrabPointTransform;
        objectRigidbody.useGravity = false;
        objectRigidbody.drag = 10f;
        objectRigidbody.freezeRotation = true;
        // objectRigidbody.isKinematic = true;
    }

    public Vector3 GetSize()
    {
        return GetComponent<Collider>().bounds.size;

    }

    public int GetId()
    {
        return GetInstanceID();
    }

    public void Drop()
    {
        dropSource.Play();
        Physics.IgnoreCollision(objectGrabPointTransform.parent.GetComponent<Collider>(), GetComponent<Collider>(), false);
        this.rotationSpeed = 0f;
        this.objectGrabPointTransform = null;
        objectRigidbody.useGravity = true;
        objectRigidbody.drag = 0;
        objectRigidbody.freezeRotation = false;
    }

    public void Rotate(float angle)
    {
        if (!isRotating)
        {
            rotateSource.Play();
            // Calculate the target rotation (90 degrees clockwise)
            targetRotation = transform.rotation * Quaternion.Euler(0f, angle, 0f);

            // Set the flag to start rotating
            isRotating = true;
        }
    }

    private void FixedUpdate()
    {
        if (objectGrabPointTransform != null)
        {
            float t = Time.deltaTime;
            if (isRotating)
            {
                // Smoothly interpolate between the current rotation and the target rotation
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, t * rotationSpeed);

                // Check if the rotation is almost complete
                if (Quaternion.Angle(transform.rotation, targetRotation) < 0.01f)
                {
                    // Ensure the final rotation is exactly the target rotation
                    transform.rotation = targetRotation;

                    // Reset the flag
                    isRotating = false;
                }
            }
            float lerpSpeed = 10f;
            transform.position = Vector3.Lerp(transform.position, objectGrabPointTransform.position, t * lerpSpeed);
        }
        
    }

    private void Update()
    {

    }
}