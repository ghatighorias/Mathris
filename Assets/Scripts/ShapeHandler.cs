using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeHandler : MonoBehaviour {

    GridHandler gridHandler;
    float fallTimer = 0F;
    public float fallDelay = 1F;
    public bool skipFallForOneFrame = true;
    public bool allowRotatation = true;
    public bool limitRotatation = false;

    Rotate ReverseRotate => transform.rotation.eulerAngles.z > 0 ? Rotate.CounterClockWise : Rotate.ClockWise;

    // Use this for initialization
    void Start() {
        gridHandler = FindObjectOfType<GridHandler>();
    }

    // Update is called once per frame
    void Update() {
        skipFallForOneFrame = false;

        CheckUserInput();

        fallTimer += Time.deltaTime;

        if (fallTimer >= fallDelay)
        {
            fallTimer = 0F;
            if (!skipFallForOneFrame)
            {
                // MoveTile(PossibleSteps.Down);
            }
        }

    }

    void CheckUserInput()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveShapeIfValid(Move.Down);
            skipFallForOneFrame = true;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (allowRotatation)
            {
                if (limitRotatation)
                {
                    RotateShapeIfValid(ReverseRotate);
                }
                else
                {
                    RotateShapeIfValid(Rotate.ClockWise);
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveShapeIfValid(Move.Left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveShapeIfValid(Move.Right);
        }
    }

    void MoveShapeIfValid(Move move)
    {
        MoveShape(move, false);

        CheckToReverseChange(() => { MoveShape(move, true); });
    }

    void RotateShapeIfValid(Rotate rotate)
    {
        RotateShape(rotate);
        CheckToReverseChange(() => { RotateShape(ReverseRotate); });
    }

    void CheckToReverseChange(Action Reverser)
    {
        foreach (Transform blockTransform in transform)
        {
            if (!gridHandler.IsInBound(blockTransform.position))
            {
                Reverser();
                return;
            }
        }
    }

    void MoveShape(Move move, bool reverse)
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

        transform.position += reverse ? -positionOffset : positionOffset;
    }

    void RotateShape(Rotate rotate)
    {
        var zAxisRotationDegree = rotate == Rotate.ClockWise ? 90 : -90;

        transform.Rotate(0, 0, zAxisRotationDegree);
    }

    public enum Move
    {
        Up,
        Down,
        Left,
        Right
    }

    public enum Rotate
    {
        ClockWise,
        CounterClockWise
    }
}
