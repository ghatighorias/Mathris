using System.Collections;
using System.Collections.Generic;
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
        gridBound = new Bounds(transform.position, (Vector2)gridSize);
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
