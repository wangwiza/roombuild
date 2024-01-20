using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerPickUpDrop : MonoBehaviour
{
    [SerializeField]
    private LayerMask pickUpLayerMask;
    private CharacterController controller;
    private bool grabbing;

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
        if (grabbing) {
            float pickUpDistance = 2f;
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit raycastHit, pickUpDistance, pickUpLayerMask)) {
                Debug.Log(raycastHit.transform);
                Debug.Log("hahaha");
                
            }
        }
    }
}
