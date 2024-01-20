using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerPickUpDrop : MonoBehaviour
{
    [SerializeField]
    private LayerMask pickUpLayerMask;
    [SerializeField]
    private Transform objectGrabPointTransform;
    [SerializeField]
    private float pickUpVerticalOffset;
    private CharacterController controller;
    private bool grabbing;

    private ObjectGrabbable objectGrabbable;
    private Spawner spawner;

    // Start is called before the first frame update
    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<Spawner>();
    }
    public void OnGrab(InputAction.CallbackContext context)
    {
        grabbing = context.action.triggered;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward) * 2;
        Debug.DrawRay(transform.position + new Vector3(0f, pickUpVerticalOffset, 0f), forward, Color.green);
        if (grabbing)
        {
            if (objectGrabbable == null)
            {
                // not carrying, try to grab
                float pickUpDistance = 2f;
                if (Physics.Raycast(transform.position + new Vector3(0f, pickUpVerticalOffset, 0f), transform.forward, out RaycastHit raycastHit, pickUpDistance, pickUpLayerMask))
                {
                    if (raycastHit.collider.gameObject.tag == "Wall")
                    {
                        // Debug.Log("WALL");
                        spawner.ConvertBoxToItem();
                    }
                    if (raycastHit.transform.TryGetComponent(out objectGrabbable))
                    {
                        if (raycastHit.collider.gameObject.tag == "Item")
                        { 
                            Debug.Log("NEW");
                            spawner.PickUpSpawnedItem();
                        }
                        Debug.Log("GRAB");
                        objectGrabbable.Grab(objectGrabPointTransform);
                    }

                }
            } else {
                objectGrabbable.Drop();
                objectGrabbable = null;
            }
        }
    }
}
