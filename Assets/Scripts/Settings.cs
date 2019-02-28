﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ShapeHandler;

[RequireComponent(typeof(InputHandler))]
public class Settings : MonoBehaviour {
    public float fallDelay = 1F;
    public bool allowAutomaticDrop = true;
    public Vector3 shapeSpawnPosition = Vector3.zero;
    public Vector3 shapeHoldPosition = Vector3.zero;
    bool skipFallForOneFrame;

    ShapeHandler ActiveShape;
    ShapeHandler NextShape;

    bool showGuideShape = true;
    ShapeHandler GuideShape;

    GridHandler gridHandler;
    InputHandler inputHandler;

    float fallTimer;

    [HideInInspector]
    public int comboLevel;
    [HideInInspector]
    public int currentLevel;
    [HideInInspector]
    public int currentScore;

    float softDropDelay;
    float softDropTimer;

    float hardDropDelay;
    float hardDropTimer;

    void Start()
    {
        gridHandler = FindObjectOfType<GridHandler>();
        inputHandler = GetComponent<InputHandler>();

        fallTimer = 0F;

        softDropTimer = 0F;
        softDropDelay = 0.2F;

        hardDropTimer = 0F;
        hardDropDelay = 0.1F;

        comboLevel = 0;
        currentScore = 0;
        currentLevel = 1;

        SpawnRandomShape(true);
    }

    void Update()
    {
        skipFallForOneFrame = false;

        MapInputToAction();

        if (allowAutomaticDrop)
        {
            fallTimer += Time.deltaTime;

            if (fallTimer >= fallDelay)
            {
                fallTimer = 0F;
                if (!skipFallForOneFrame)
                {
                    ActiveShape.MoveShapeIfValid(Move.Down);
                }
            }
        }
    }

    void HandleSoftDrop()
    {
        softDropTimer += Time.deltaTime;

        if (softDropTimer >= softDropDelay)
        {
            softDropTimer = 0F;
            ActiveShape.MoveShapeIfValid(Move.Down);
        }
    }

    void HandleDropDrop()
    {
            hardDropTimer += Time.deltaTime;

            if (hardDropTimer >= hardDropDelay)
            {
                hardDropTimer = 0F;
                ActiveShape.MoveShapeIfValid(Move.Down);
            }
    }

    void SpawnRandomShape(bool firstBlock)
    {
        ActiveShape = firstBlock ? ShapeHandler.InstantiateRandomShape() : NextShape;
        ActiveShape.gameObject.transform.position = shapeSpawnPosition;
        ActiveShape.ShapeLanded = OnActiveShapeLanded;

        NextShape = ShapeHandler.InstantiateRandomShape();
        NextShape.gameObject.transform.position = shapeHoldPosition;
    }

    void OnActiveShapeLanded()
    {
        inputHandler.ResetHardDrop();

        gridHandler.AddToGrid(ActiveShape);
        Destroy(ActiveShape.gameObject);

        var completedRows = gridHandler.GetCompletedRows();
        ProcessLineRemoval(completedRows.Count);
        gridHandler.DeleteRows(completedRows);

        SpawnRandomShape(false);
    }

    void ProcessLineRemoval(int clearedLines)
    {
        if (clearedLines <= 0)
        {
            comboLevel = 0;
        }
        else
        {
            var award = LineScoreHandler.GetScore(clearedLines, comboLevel, currentLevel);
            clearedLines += award.lineAward;
            currentScore += award.score;
            comboLevel++;
        }

    }

    void MapInputToAction()
    {
        var userAction = inputHandler.ActionMapper;

        if (userAction.hardDrop)
        {
            HandleDropDrop();
        }
        else if (userAction.softDrop)
        {
            HandleSoftDrop();
        }

        if (userAction.moveLeft)
        {
            ActiveShape.MoveShapeIfValid(Move.Left);
        }

        if (userAction.moveRight)
        {
            ActiveShape.MoveShapeIfValid(Move.Right);
        }

        if (userAction.rotate)
        {
            ActiveShape.RotateShapeIfValid();
        }
    }

    void OnGUI()
    {
        var output = string.Empty;
        
        GUI.Label(new Rect(100, 0, 100, 100), string.Format("score: {0}", currentScore));
        GUI.Label(new Rect(100, 100, 100, 100), string.Format("combo: {0}", comboLevel));
    }
}
