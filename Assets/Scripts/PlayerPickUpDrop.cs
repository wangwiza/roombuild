using System;
using System.Collections.Generic;
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
    [SerializeField] private GameObject highlightPrefab;
    private List<GameObject> currentHighlights = new();
    [SerializeField] private Grid grid;
    private Vector3 lastHighlightPosition;

    private GridData floorData;
    private Renderer previewRenderer;
    private Vector3 furnitureSize;
    private Vector3 basePosition;

    private void HighlightGrid()
    {
        ClearHighlights();

        
        Vector3 objectSize = objectGrabbable.GetSize();
        furnitureSize = objectSize;
        Debug.Log($"{objectSize.x} {objectSize.y} {objectSize.z}");
        Vector3 gridPosition = grid.WorldToCell(transform.position + transform.forward);
        basePosition = gridPosition;

        //idk where to put this
        //ideally want to check the highlighted area
        /*bool placementValidity = CheckPlacementValidity(gridPosition);
        previewRenderer.material.color = placementValidity ? Color.white : Color.red;*/
        
        for (int x = 0; x < Mathf.CeilToInt(objectSize.x); x++)
        {
            for (int z = 0; z < Mathf.CeilToInt(objectSize.z); z++)
            {
                Vector3 highlightGridPosition = new Vector3(gridPosition.x + x, (float)0.3, gridPosition.y + z);
                GameObject highlight = Instantiate(highlightPrefab, highlightGridPosition, Quaternion.identity);
                currentHighlights.Add(highlight);
                lastHighlightPosition = highlightGridPosition;
            }
        }

        //Vector3Int gridPosition = grid.WorldToCell(transform.position);
        //Vector3Int newPos = new Vector3Int(gridPosition.x, gridPosition.z, gridPosition.y);
        //Vector3 frontPosition = grid.WorldToCell(transform.position + transform.forward);
        //Vector3 newDir = new Vector3(frontPosition.x, (float)0.3, frontPosition.y);
        //Debug.Log($"pos: {newPos}, dir: {newDir}");
        //GameObject highlight = Instantiate(highlightPrefab, newDir, Quaternion.identity);
        //currentHighlights.Add(highlight);

    }

    private void ClearHighlights()
    {
        foreach (GameObject highlight in currentHighlights)
        {
            Destroy(highlight);
        }

        currentHighlights.Clear();
    }

   
    // Start is called before the first frame update
    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<Spawner>();
        floorData = new(); //will data be affected with 2 players?
        previewRenderer = highlightPrefab.GetComponentInChildren<Renderer>();
    }

    private void Update()
    {
        if (objectGrabbable)
        {
            HighlightGrid();
        } 
        //else
        //{
        //    ClearHighlights();
        //}
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
            /*bool placementValidity = CheckPlacementValidity(lastHighlightPosition);
            if (!placementValidity)
                return;*/
            objectGrabbable.transform.position = lastHighlightPosition;
            objectGrabbable.Drop();
            objectGrabbable = null;
            ClearHighlights();
        }
    }

    /*private bool CheckPlacementValidity(Vector3 lastHighlightPosition)
    {
        return floorData.CanPlaceObjectAt(basePosition, furnitureSize);
    }*/
}
