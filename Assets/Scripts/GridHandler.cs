﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class GridHandler : MonoBehaviour
{
    public Vector2Int gridSize = new Vector2Int(10, 10);
    public Vector2Int blockSize = new Vector2Int(1, 1);
    public Color backgroundColor = Color.white; 

    Dictionary<int, List<GameObject>> rowDictionary;

    [SerializeField]
    Transform gridObstacles;

    [SerializeField]
    Color grayoutColor = new Color(0,0,0,128);

    public float Top => transform.position.y + gridSize.y / 2;
    public float Bottom => transform.position.y - gridSize.y / 2;
    public float Left => transform.position.x - gridSize.x / 2;
    public float Right => transform.position.x + gridSize.x / 2;

    public void GrayoutObstacles()
    {
        foreach (Transform obstacle in gridObstacles)
        {
            var spriteRenderer = obstacle.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.color = grayoutColor;
            }
        }
    }

    public void ClearGrid()
    {
        foreach (Transform obstacle in gridObstacles)
        {
            Destroy(obstacle.gameObject);
        }

        SetupRowDictionary();
    }

    public void SetObstacleActive(bool active)
    {
        gridObstacles.gameObject.SetActive(active);
    }

    void Start()
    {
        if (gridObstacles == null)
        {
            gridObstacles = transform.Find("Obstacles");
        }

        SetupGridBackground();

        SetupBound();

        SetupRowDictionary();
    }

    void SetupGridBackground()
    {
        var backgroundRenderer = GetComponent<SpriteRenderer>();
        backgroundRenderer.drawMode = SpriteDrawMode.Tiled;
        backgroundRenderer.size = gridSize;
        backgroundRenderer.color = backgroundColor;
    }

    void SetupBound()
    {
        var gridBottom = transform.Find("GridBottom");
        if (gridBottom != null)
        {
            gridBottom.transform.position = new Vector3(0, Bottom - (float)blockSize.y / 2, 0);
            gridBottom.transform.localScale = new Vector3(gridSize.x, 1, 1);
        }

        var gridLeft = transform.Find("GridLeft");
        if (gridLeft != null)
        {
            gridLeft.transform.position = new Vector3(Left - (float)blockSize.y / 2, (float)blockSize.y / 2, 0);
            gridLeft.transform.localScale = new Vector3(1, gridSize.y + 3, 1);
        }

        var gridRight = transform.Find("GridRight");
        if (gridRight != null)
        {
            gridRight.transform.position = new Vector3(Right + (float)blockSize.y / 2, (float)blockSize.y / 2, 0);
            gridRight.transform.localScale = new Vector3(1, gridSize.y + 3, 1);
        }
    }

    void SetupRowDictionary()
    {
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
            // change object position
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
    public bool AddToGrid(ShapeHandler Shape)
    {
        List<Transform> blocksToAdd = new List<Transform>();

        foreach (Transform block in Shape.transform)
        {
            int candidRow = GetBlockRowIndex(block);
            if (IsRowValid(candidRow))
            {
                rowDictionary[candidRow].Add(block.gameObject);
                block.gameObject.layer = LayerMask.NameToLayer("obstacle");
                blocksToAdd.Add(block);
            }
            else
                return false;
        }

        blocksToAdd.ForEach((block) => block.parent = gridObstacles.transform);

        return true;
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

    public List<GameObject> GetRowBlocks(int row)
    {
        if (IsRowValid(row))
        {
            return rowDictionary[row];
        }

        return null;
    }

    private void OnDrawGizmos()
    {
        SetupBound();
        Gizmos.color = Color.yellow;

        // draw the grid
        for (int row = 0; row <= gridSize.y; row++)
        {
            Gizmos.DrawLine(new Vector3(Left,  row + Bottom, 0),
                new Vector3(Right,  row + Bottom, 0));
        }

        for (int col = 0; col <= gridSize.x; col++)
        {
            Gizmos.DrawLine(new Vector3(col + Left, Bottom, 0),
                            new Vector3(col + Left, Top, 0));
        }
    }
}
