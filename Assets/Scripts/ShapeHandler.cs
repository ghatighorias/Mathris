using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeHandler : MonoBehaviour {

    [HideInInspector]
    public Action ShapeLanded;

    public LayerMask obstacleLayer;
    public bool allowRotatation = true;
    public bool limitRotatation = false;
    public Rotate ReverseRotate => transform.rotation.eulerAngles.z > 0 ? Rotate.CounterClockWise : Rotate.ClockWise;

    public void MoveShapeIfValid(Move move)
    {
        var nextShapeLocation = GetNextShapeLocation(move);

        var traceResult = RayTraceLocation(nextShapeLocation, transform.rotation);

        if ( traceResult == RaytraceHitResultType.None)
        {
            transform.position = nextShapeLocation;
        }
        else if (move == Move.Down)
        {
            ShapeLanded?.Invoke();
        }
    }

    public void RotateShapeIfValid(Rotate rotate)
    {
        var nextShapeRotation = GetNextShapeRotation(rotate);
        if (RayTraceLocation(transform.position, nextShapeRotation) == RaytraceHitResultType.None)
        {
            transform.rotation = nextShapeRotation;
        }
    }

    Vector3 GetNextShapeLocation(Move move)
    {
        var positionOffset = Vector3.zero;

        switch (move)
        {
            case Move.Down:
                positionOffset += Vector3.down;
                break;
            case Move.Up:
                positionOffset += Vector3.up;
                break;
            case Move.Left:
                positionOffset += Vector3.left;
                break;
            case Move.Right:
                positionOffset += Vector3.right;
                break;
        }
        
        return transform.position + positionOffset;
    }

    Quaternion GetNextShapeRotation(Rotate rotate)
    {
        var zAxisRotationDegree = rotate == Rotate.ClockWise ? 90 : -90;

        return transform.rotation * Quaternion.Euler(0, 0, zAxisRotationDegree);
    }

    RaytraceHitResultType RayTraceLocation(Vector3 LocationToCheck, Quaternion nextShapeRotation)
    {
        RaytraceHitResultType hitResultType = RaytraceHitResultType.None;

        foreach (Transform blockTransform in transform)
        {
            var blockNextLocation = nextShapeRotation * blockTransform.localPosition + LocationToCheck;
            var hit = Physics2D.OverlapPoint(blockNextLocation, obstacleLayer);
            if (hit != null)
            {
                hitResultType = Settings.ConvertTag(hit.tag);
            }
        }

        return hitResultType;
    }
}
