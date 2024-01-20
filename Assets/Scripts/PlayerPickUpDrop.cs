using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerPickUpDrop : MonoBehaviour
{
    [SerializeField]
    private LayerMask pickUpLayerMask;
    [SerializeField]
    private Transform objectGrabPointTransform;
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
        if (grabbing)
        {
            if (objectGrabbable == null)
            {
                // not carrying, try to grab
                float pickUpDistance = 2f;
                if (Physics.Raycast(transform.position + new Vector3(0f, -0.5f, 0f), transform.forward, out RaycastHit raycastHit, pickUpDistance, pickUpLayerMask))
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
