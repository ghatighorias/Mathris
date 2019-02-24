using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GridHandler : MonoBehaviour {

    Bounds gridBound;
    public Vector2Int gridSize = new Vector2Int(10, 10);
    public Dictionary<int, List<GameObject>> GridRowDictionary;

    private void Start()
    {
        SetupBound();
    }

    void SetupBound()
    {
        gridBound = new Bounds(transform.position, new Vector3(gridSize.x, gridSize.y, 100));

        var gridBottom = transform.Find("GridBottom");
        if (gridBottom != null)
        {
            gridBottom.transform.position = new Vector3(0, -(float)(gridSize.y) / 2 - 1, 0);
            gridBottom.transform.localScale = new Vector3(gridSize.x, 1, 1);
        }

        var gridLeft = transform.Find("GridLeft");
        if (gridLeft != null)
        {
            gridLeft.transform.position = new Vector3(-(float)(gridSize.x) / 2 - 0.5F, 0, 0);
            gridLeft.transform.localScale = new Vector3(1, gridSize.y + 1, 1);
        }

        var gridRight = transform.Find("GridRight");
        if (gridRight != null)
        {
            gridRight.transform.position = new Vector3(+(float)(gridSize.x) / 2 + 0.5F, 0, 0);
            gridRight.transform.localScale = new Vector3(1, gridSize.y + 1, 1);
        }

        GridRowDictionary = new Dictionary<int, List<GameObject>>();
        for (int index = 1; index <= gridSize.y; index++)
        {
            GridRowDictionary.Add(index, new List<GameObject>(gridSize.x));
        }
    }

    public bool IsInBound(Vector3 position) => gridBound.Contains(position);

    bool IsRowCompleted(int row) => GridRowDictionary[row].Count == gridSize.x;

    public void ShiftRowDown(int row, int shitfCount)
    {
        if (row != 0 && GridRowDictionary[row].Count > 0)
        {
            var rowBlocks = GridRowDictionary[row];
            rowBlocks.ForEach((block) => { block.transform.position += shitfCount * Vector3.down; });
            GridRowDictionary[row - 1] = rowBlocks;
            GridRowDictionary[row].Clear();
        }
    }

    public void RemoveRow(int row)
    {
        // this should be tested to see if destroying will fuck up the index
        GridRowDictionary[row].ForEach((block) => { Destroy(block); });

        GridRowDictionary[row].Clear();
    }

    int GetBlockRowIndex(Transform block) => (int)block.position.y + (int)gridSize.y / 2 + 1;

    public List<int> AddToGrid(ShapeHandler Shape)
    {
        List<Transform> children = new List<Transform>();
        var gridObstacles = transform.Find("Obstacles");
        List<int> completedRows = new List<int>();

        foreach (Transform block in Shape.transform)
        {
            int row = GetBlockRowIndex(block);
            GridRowDictionary[row].Add(block.gameObject);
            block.gameObject.layer = LayerMask.NameToLayer("obstacle");
            children.Add(block);

            if (IsRowCompleted(row))
            {
                completedRows.Add(row);
            }
        }

        children.ForEach((child) => { child.parent = gridObstacles.transform; });

        return completedRows;
    }

    private void OnDrawGizmos()
    {
        SetupBound();
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(gridBound.center, gridBound.size);
    }
}
