using System;
using System.Collections.Generic;
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

    [SerializeField]
    public Animator animator;

    private GridData floorData;
    private Renderer previewRenderer;

    [SerializeField] private GridManager database;
    private List<Vector3Int> positions = new();

    private void HighlightGrid()
    {
        ClearHighlights();
        
        Vector3 objectSize = objectGrabbable.GetSize();
        //furnitureSize = new Vector2Int(objectSize.x, y: (int)objectSize.z);
        Vector3 gridPosition = grid.WorldToCell(transform.position + transform.forward *2);
        //Debug.Log($"{gridPosition}");
        //basePosition = new Vector3Int((int)gridPosition.x, (int)gridPosition.z, (int)gridPosition.y);

        //idk where to put this
        //ideally want to check the highlighted area
        //bool placementValidity = CheckPlacementValidity(gridPosition);
        
        for (int x = 0; x < Mathf.CeilToInt(objectSize.x); x++)
        {
            for (int z = 0; z < Mathf.CeilToInt(objectSize.z); z++)
            {
                Vector3 highlightGridPosition = new Vector3(gridPosition.x + x, (float)0.3, gridPosition.y + z);
                positions.Add(new Vector3Int((int)highlightGridPosition.x, (int)highlightGridPosition.y, (int)highlightGridPosition.z));
                GameObject highlight = Instantiate(highlightPrefab, highlightGridPosition, Quaternion.identity);
                previewRenderer = highlight.GetComponentInChildren<Renderer>();
                previewRenderer.material.color = Color.white;
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
        positions.Clear();
    }

   
    // Start is called before the first frame update
    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<Spawner>();
        floorData = new(); //will data be affected with 2 players?
        previewRenderer = highlightPrefab.GetComponentInChildren<Renderer>();
        animator = gameObject.GetComponentInChildren<Animator>();
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
            float pickUpDistance = 1f;
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
                    animator.SetBool("isHolding", true);
                    objectGrabbable.Grab(objectGrabPointTransform);
                    database.RemoveObject(objectGrabbable.GetId());
                    foreach (GridObject gridObject in database.gridObjects)
                    {
                        string positions = "";
                        foreach (Vector3Int pos in gridObject.gridPositions)
                        {
                            positions += $"({pos.x}, {pos.y}, {pos.z}), ";
                        }

                        // Optionally trim the trailing comma and space
                        if (positions.Length > 0)
                        {
                            positions = positions.Substring(0, positions.Length - 2);
                        }
                        Debug.Log($"Object ID: {gridObject.objectId}, Positions: {positions}");
                    }
                }
            }
        }
        else
        {
            /*bool placementValidity = CheckPlacementValidity(lastHighlightPosition);
            if (!placementValidity)
                return;*/
            animator.SetBool("isHolding", false);

            objectGrabbable.transform.position = lastHighlightPosition;
            database.AddOrUpdateObject(objectGrabbable.GetId(), positions);
            foreach (GridObject gridObject in database.gridObjects)
            {
                string positions = "";
                foreach (Vector3Int pos in gridObject.gridPositions)
                {
                    positions += $"({pos.x}, {pos.y}, {pos.z}), ";
                }

                // Optionally trim the trailing comma and space
                if (positions.Length > 0)
                {
                    positions = positions.Substring(0, positions.Length - 2);
                }
                Debug.Log($"Object ID: {gridObject.objectId}, Positions: {positions}");
            }
            objectGrabbable.Drop();
            objectGrabbable = null;
            ClearHighlights();
        }
    }
}
