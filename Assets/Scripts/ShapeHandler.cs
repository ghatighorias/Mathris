using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeHandler : MonoBehaviour {

    GridHandler gridHandler;
    //Settings settings;

    public Action ShapeLanded;
    int obstacleLayer;
    public bool allowRotatation = true;
    public bool limitRotatation = false;
    public Rotate ReverseRotate => transform.rotation.eulerAngles.z > 0 ? Rotate.CounterClockWise : Rotate.ClockWise;

    void Awake()
    {
        obstacleLayer= LayerMask.GetMask("obstacle");
    }

    // Use this for initialization
    void Start() {
        gridHandler = FindObjectOfType<GridHandler>();
        //settings = FindObjectOfType<Settings>();
    }

    public void MoveShapeIfValid(Move move)
    {
        var nextShapeLocation = GetNextShapeLocation(move, false);
        if (RayTraceLocation(nextShapeLocation, transform.rotation) == RaytraceHitResultType.None)
        {
            transform.position = nextShapeLocation;
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

    Vector3 GetNextShapeLocation(Move move, bool reverse)
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
        //transform.position += reverse ? -positionOffset : positionOffset;
        
        return (transform.position) + (reverse ? -positionOffset : positionOffset);
    }

    Quaternion GetNextShapeRotation(Rotate rotate)
    {
        var zAxisRotationDegree = rotate == Rotate.ClockWise ? 90 : -90;

        return transform.rotation * Quaternion.Euler(0, 0, zAxisRotationDegree);
    }

    RaytraceHitResultType RayTraceLocation(Vector3 LocationToCheck, Quaternion nextShapeRotation)
    {
        RaytraceHitResultType result = RaytraceHitResultType.None;

        foreach (Transform blockTransform in transform)
        {
            var blockNextLocation = nextShapeRotation * blockTransform.localPosition + LocationToCheck;
            var hit = Physics2D.OverlapPoint(blockNextLocation, LayerMask.GetMask("obstacle"));
            if (hit != null)
            {
                result = Settings.ConvertTag(hit.tag);
                Debug.Log(hit.gameObject.tag);
            }
        }

        return result;
    }
}
