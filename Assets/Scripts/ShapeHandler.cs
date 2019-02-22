using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeHandler : MonoBehaviour {

    GridHandler gridHandler;
    //Settings settings;

    public bool allowRotatation = true;
    public bool limitRotatation = false;

    public Rotate ReverseRotate => transform.rotation.eulerAngles.z > 0 ? Rotate.CounterClockWise : Rotate.ClockWise;

    // Use this for initialization
    void Start() {
        gridHandler = FindObjectOfType<GridHandler>();
        //settings = FindObjectOfType<Settings>();
    }

    // Update is called once per frame
    void Update() {

    }

    public void MoveShapeIfValid(Move move)
    {
        MoveShape(move, false);

        CheckToReverseChange(() => { MoveShape(move, true); });
    }

    public void RotateShapeIfValid(Rotate rotate)
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
