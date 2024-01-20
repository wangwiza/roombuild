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


    // Start is called before the first frame update
    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
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
                    if (raycastHit.transform.TryGetComponent(out objectGrabbable))
                    {
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
