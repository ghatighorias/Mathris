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
        var nextShapeLocation = MoveShape(move, false);
        if (RayTraceLocation(nextShapeLocation) == RaytraceHitResultType.None)
        {
            transform.position = nextShapeLocation;
        }
        //CheckToReverseChange(() => { MoveShape(move, true); });
    }

    public void RotateShapeIfValid(Rotate rotate)
    {
        var nextShapeLocation = RotateShape(rotate);
        if (RayTraceLocation(nextShapeLocation) == RaytraceHitResultType.None)
        {
            transform.position = nextShapeLocation;
        }
        //CheckToReverseChange(() => { RotateShape(ReverseRotate); });
    }

    void CheckToReverseChange(Action Reverser)
    {
        
        //foreach (Transform blockTransform in transform)
        //{
        //    if (!gridHandler.IsInBound(blockTransform.position))
        //    {
        //        Reverser();
        //        return;
        //    }
        //}
    }

    Vector3 MoveShape(Move move, bool reverse)
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

    Vector3 RotateShape(Rotate rotate)
    {
        var zAxisRotationDegree = rotate == Rotate.ClockWise ? 90 : -90;

        //transform.Rotate(0, 0, zAxisRotationDegree);
        return Quaternion.Euler(0, 0, zAxisRotationDegree) * transform.position;
    }

    RaytraceHitResultType RayTraceLocation(Vector3 LocationToCheck)
    {
        RaytraceHitResultType result = RaytraceHitResultType.None;

        foreach (Transform blockTransform in transform)
        {
            //var hit = Physics2D.OverlapPoint(new Vector2(5, 0), LayerMask.GetMask("obstacle"));
            var hit = Physics2D.OverlapPoint(blockTransform.localPosition + LocationToCheck, LayerMask.GetMask("obstacle"));
            if (hit != null)
            {
                result = Settings.ConvertTag(hit.tag);
                Debug.Log(hit.gameObject.tag);
            }
        }

        return result;
    }
}
