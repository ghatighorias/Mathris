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

    public void ExecuteAction(ActionType actionType)
    {
        switch (actionType)
        {
            case ActionType.Rotate: 
                RotateShapeIfValid();
                break;
            case ActionType.MoveLeft:
                MoveShapeIfValid(Move.Left);
                break;
            case ActionType.MoveRight:
                MoveShapeIfValid(Move.Right);
                break;
            case ActionType.Down:
                MoveShapeIfValid(Move.Down);
                break;
            default:
                break;
        }
    }

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

    public void RotateShapeIfValid()
    {
        if (allowRotatation)
        {
            var rotationDirection = limitRotatation ? ReverseRotate : Rotate.ClockWise;

            var nextShapeRotation = GetNextShapeRotation(rotationDirection);
            if (RayTraceLocation(transform.position, nextShapeRotation) == RaytraceHitResultType.None)
            {
                transform.rotation = nextShapeRotation;
            }
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

    public RaytraceHitResultType RayTraceLocation(Vector3 LocationToCheck, Quaternion nextShapeRotation)
    {
        RaytraceHitResultType hitResultType = RaytraceHitResultType.None;

        foreach (Transform blockTransform in transform)
        {
            var blockNextLocation = nextShapeRotation * blockTransform.localPosition + LocationToCheck;
            var hit = Physics2D.OverlapPoint(blockNextLocation, obstacleLayer);
            if (hit != null)
            {
                hitResultType = TagToHitResultType(hit.tag);
            }
        }

        return hitResultType;
    }

    public static RaytraceHitResultType TagToHitResultType(string tag)
    {
        switch (tag)
        {
            case "GridWall":
                return RaytraceHitResultType.GridWall;
            case "GridBottom":
                return RaytraceHitResultType.GridBottom;
            case "Block":
                return RaytraceHitResultType.Block;
            default:
                return RaytraceHitResultType.None;
        }
    }

    public static ShapeHandler InstantiateRandomShape()
    {
        var randomShapeNumber = UnityEngine.Random.Range(1, 8);
        var shapePrefix = "Tile";
        var shapePostfix = string.Empty;

        switch (randomShapeNumber)
        {
            case 1:
                shapePostfix = "J";
                break;
            case 2:
                shapePostfix = "L";
                break;
            case 3:
                shapePostfix = "Long";
                break;
            case 4:
                shapePostfix = "S";
                break;
            case 5:
                shapePostfix = "Square";
                break;
            case 6:
                shapePostfix = "T";
                break;
            case 7:
                shapePostfix = "Z";
                break;
            default:
                Debug.Log("wrong number for instantiating shape");
                return null;
        }

        var shapeFullName = string.Format("Prefabs/{0}_{1}", shapePrefix, shapePostfix);

        return Instantiate(Resources.Load<GameObject>(shapeFullName), Vector3.zero, Quaternion.identity).GetComponent<ShapeHandler>();
    }

    float GetShapeHighestLandingPoint(float gridBottomY)
    {
        Transform lowestBlock = null;
        Transform highestHitLocation = null;
        bool firstCheck = true;

        foreach (Transform blockTransform in transform)
        {
            if (firstCheck || blockTransform.position.y <= lowestBlock.position.y)
            {
                firstCheck = false;

                lowestBlock = blockTransform;

                var endRayLocation = new Vector2(lowestBlock.position.x, gridBottomY);
                var hit = Physics2D.Linecast(lowestBlock.position, endRayLocation);

                if (hit.transform != null)
                {
                    highestHitLocation = hit.transform;
                }
            }
        }

        return (lowestBlock.position - highestHitLocation.position + Vector3.up).y;
    }

    public bool OverLapsAnotherShape()
    {
        foreach (Transform block in transform)
        {
            if (RaytraceHitResultType.Block ==
            RayTraceLocation(block.position, block.rotation))
            {
                return true;
            }
        }
        return false;
    }
}
