using UnityEditor.MemoryProfiler;
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
        if (context.performed)
        {
            tryGrab();
        }
    }

    public void OnLeftRotate(InputAction.CallbackContext context)
    {
        if (context.performed && objectGrabbable != null) {
            objectGrabbable.Rotate(-90f);
        }
    }
    
    public void OnRightRotate(InputAction.CallbackContext context)
    {
        if (context.performed && objectGrabbable != null) {
            objectGrabbable.Rotate(90f);
        }
    }
    

    private void tryGrab() {
        if (objectGrabbable == null)
        {
            // not carrying, try to grab
            float pickUpDistance = 2f;
            if (Physics.SphereCast(transform.position + new Vector3(-1f, pickUpVerticalOffset, 0f), 1f, transform.forward, out RaycastHit raycastHit, pickUpDistance, pickUpLayerMask))
            {
                Debug.Log(raycastHit.transform);
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
        }
        else
        {
            objectGrabbable.Drop();
            objectGrabbable = null;
        }
    }
}
