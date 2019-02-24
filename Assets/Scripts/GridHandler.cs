using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GridHandler : MonoBehaviour {

    Bounds gridBound;
    public Vector2Int gridSize = new Vector2Int(10, 10);

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
            gridBottom.transform.position = new Vector3(0, -(float)(gridSize.y) / 2, 0);
            gridBottom.transform.localScale = new Vector3(gridSize.x, 1, 1);
        }

        var gridLeft = transform.Find("GridLeft");
        if (gridLeft != null)
        {
            gridLeft.transform.position = new Vector3(-(float)(gridSize.x) / 2, 0, 0);
            gridLeft.transform.localScale = new Vector3(1, gridSize.y, 1);
        }

        var gridRight = transform.Find("GridRight");
        if (gridRight != null)
        {
            gridRight.transform.position = new Vector3(+(float)(gridSize.x) / 2, 0, 0);
            gridRight.transform.localScale = new Vector3(1, gridSize.y, 1);
        }
    }

    public bool IsInBound(Vector3 position)
    {

        return gridBound.Contains(position);
    }

    private void OnDrawGizmos()
    {
        SetupBound();
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(gridBound.center, gridBound.size);
    }
}
