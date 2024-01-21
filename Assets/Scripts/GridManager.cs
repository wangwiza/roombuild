using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridObject
{
    public int objectId;
    public List<Vector3Int> gridPositions;

    public GridObject(int id, List<Vector3Int> positions)
    {
        objectId = id;
        gridPositions = positions;
    }

    // You might also want a method to add or remove positions
    public void AddPosition(Vector3Int position)
    {
        if (!gridPositions.Contains(position))
        {
            gridPositions.Add(position);
        }
    }

    public void RemovePosition(Vector3Int position)
    {
        gridPositions.Remove(position);
    }
}


public class GridManager : MonoBehaviour
{
    public List<GridObject> gridObjects = new List<GridObject>();
    private GridManager database;


    private void Start()
    {
        database = new();
    }

    public void AddOrUpdateObject(int id, List<Vector3Int> positions)
    {
        GridObject existingObject = gridObjects.Find(obj => obj.objectId == id);
        if (existingObject != null)
        {
            existingObject.gridPositions = positions;
        }
        else
        {
            gridObjects.Add(new GridObject(id, positions));
        }
    }

    public void RemoveObject(int id)
    {
        gridObjects.RemoveAll(obj => obj.objectId == id);
    }

    public bool IsCellOccupied(Vector3Int gridPosition)
    {
        foreach (var obj in gridObjects)
        {
            if (obj.gridPositions.Contains(gridPosition))
            {
                return true;
            }
        }
        return false;
    }

    // Additional methods as needed
}


