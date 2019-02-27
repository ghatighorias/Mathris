using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GridHandler : MonoBehaviour {

    public Vector2Int gridSize = new Vector2Int(10, 10);
    public Vector2Int blockSize = new Vector2Int(1, 1);
    Dictionary<int, List<GameObject>> rowDictionary;

    Transform gridObstacles;


    private void Start()
    {
        gridObstacles = transform.Find("Obstacles");
        SetupBound();
    }

    void SetupBound()
    {
        var gridBottom = transform.Find("GridBottom");
        if (gridBottom != null)
        {
            gridBottom.transform.position = new Vector3(0, -(float)(gridSize.y) / 2 - (float)blockSize.y / 2, 0);
            gridBottom.transform.localScale = new Vector3(gridSize.x, 1, 1);
        }

        var gridLeft = transform.Find("GridLeft");
        if (gridLeft != null)
        {
            gridLeft.transform.position = new Vector3(-(float)(gridSize.x) / 2 - (float)blockSize.y / 2, 0, 0);
            gridLeft.transform.localScale = new Vector3(1, gridSize.y + 1, 1);
        }

        var gridRight = transform.Find("GridRight");
        if (gridRight != null)
        {
            gridRight.transform.position = new Vector3(+(float)(gridSize.x) / 2 + (float)blockSize.y / 2, 0, 0);
            gridRight.transform.localScale = new Vector3(1, gridSize.y + 1, 1);
        }

        // create the row keys
        rowDictionary = new Dictionary<int, List<GameObject>>();
        for (int index = 0; index < gridSize.y; index++)
        {
            rowDictionary.Add(index, new List<GameObject>());
        }
    }

    bool IsRowCompleted(int row) => rowDictionary[row].Count == gridSize.x;

    bool IsRowValid(int row) => rowDictionary.ContainsKey(row) ;

    int GetBlockRowIndex(Transform block) => (int)Mathf.Floor(block.position.y + (float)gridSize.y / 2);

    void ShiftRowDown(int targetRow, int destinationRow)
    {
        if (IsRowValid(targetRow) && IsRowValid(destinationRow))
        {
            // move the object in array
            rowDictionary[destinationRow] = rowDictionary[targetRow];
            rowDictionary[targetRow] = new List<GameObject>();
            // move the object in viewport
            rowDictionary[destinationRow].ForEach((block) => block.transform.position += Vector3.down * (targetRow - destinationRow));
        }
    }

    void DeleteRow(int row)
    {
        rowDictionary[row].ForEach((block) => { Destroy(block); });
        rowDictionary[row].Clear();
    }

    /// <summary>
    /// Deletes the rows. and shiftdown others affected by it
    /// </summary>
    /// <param name="rows">List of row indexes to be deleted</param>
    public void DeleteRows(List<int> rows)
    {
        int shiftDownCounter = 0;

        for (int rowIndex = 0; rowIndex < gridSize.y; rowIndex++)
        {
            if (rows.Contains(rowIndex))
            {
                DeleteRow(rowIndex);
                shiftDownCounter++;
            }
            else if (shiftDownCounter > 0)
            {
                ShiftRowDown(rowIndex, rowIndex - shiftDownCounter);
            }
        }
    }

    /// <summary>
    /// Adds blocks of the shape to the gridHandler gameobject and sets
    /// their layer to obstacle
    /// </summary>
    /// <param name="Shape">Shape to be deconstructed and added to grid</param>
    public void AddToGrid(ShapeHandler Shape)
    {
        List<Transform> children = new List<Transform>();

        foreach (Transform block in Shape.transform)
        {
            int row = GetBlockRowIndex(block);
            Debug.Log(row);
            rowDictionary[row].Add(block.gameObject);
            block.gameObject.layer = LayerMask.NameToLayer("obstacle");
            children.Add(block);
        }

        children.ForEach((child) => { child.parent = gridObstacles.transform; });
    }

    /// <summary>
    /// Get the list of row indexes that are completed
    /// </summary>
    /// <returns>The completed row indexes</returns>
    public List<int> GetCompletedRows()
    {
        List<int> completedRows = new List<int>();

        foreach (var rowPair in rowDictionary)
        {
            if (IsRowCompleted(rowPair.Key))
            {
                completedRows.Add(rowPair.Key);
            }
        }

        return completedRows;
    }

    private void OnDrawGizmos()
    {
        SetupBound();
        Gizmos.color = Color.yellow;

        // draw the grid
        for (int row = 0; row <= gridSize.y; row++)
        {
            Gizmos.DrawLine(new Vector3(-gridSize.x / 2,  row-gridSize.y / 2, 0),
                new Vector3(gridSize.x / 2,  row-gridSize.y / 2, 0));
        }

        for (int col = 0; col <= gridSize.x; col++)
        {
            Gizmos.DrawLine(new Vector3(col-gridSize.x / 2, -gridSize.y / 2, 0),
                            new Vector3(col-gridSize.x / 2, gridSize.y / 2, 0));
        }
    }

    void OnGUI()
    {
        var output = string.Empty;
        for (int i = rowDictionary.Count - 1; i >= 0; i--)
        {
            output += string.Format("\nrow-{0} : {1}", i, rowDictionary[i].Count);
        }
        GUI.Label(new Rect(0, 0, 100, 350), output);
    }

}
