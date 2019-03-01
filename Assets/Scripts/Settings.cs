using System;
using System.Collections;
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

    public int shapeSortingLayer = 2;
    public int guideShapeSortingLayer = 1;

    ShapeHandler ActiveShape;
    ShapeHandler NextShape;

    public bool showLandingGuide = true;
    ShapeHandler landingGuideShape;

    GridHandler gridHandler;
    InputHandler inputHandler;

    float fallTimer;

    float softDropDelay;
    float softDropTimer;

    float hardDropDelay;
    float hardDropTimer;

    [HideInInspector]
    bool IsGameOver;

    public ScoreState scoreState;

    void Start()
    {
        gridHandler = FindObjectOfType<GridHandler>();
        inputHandler = GetComponent<InputHandler>();

        scoreState = new ScoreState(0, 0);

        fallTimer = 0F;

        softDropTimer = 0F;
        softDropDelay = 0.2F;

        hardDropTimer = 0F;
        hardDropDelay = 0.1F;

        SpawnRandomShape(true);
    }

    void Update()
    {
        if (!IsGameOver)
        {
            skipFallForOneFrame = false;

            MapInputToAction();

            UpdatelandingGuideShape();

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
    }

    void UpdatelandingGuideShape()
    {
        landingGuideShape.gameObject.SetActive(showLandingGuide);

        var landingOffset = ActiveShape.GetShapeLandingOffset(-gridHandler.gridSize.y / 2);
        landingGuideShape.transform.position = ActiveShape.transform.position + landingOffset;
        landingGuideShape.transform.rotation = ActiveShape.transform.rotation;
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
        ActiveShape = firstBlock ? ShapeHandler.InstantiateRandomShape(shapeSortingLayer) : NextShape;
        ActiveShape.gameObject.transform.position = shapeSpawnPosition;
        ActiveShape.ShapeLanded = OnActiveShapeLanded;

        NextShape = ShapeHandler.InstantiateRandomShape(shapeSortingLayer);
        NextShape.gameObject.transform.position = shapeHoldPosition;

        if (ActiveShape.OverLapsAnotherShape())
        {
            GameOver();
        }

        if(landingGuideShape)
        {
            Destroy(landingGuideShape.gameObject);
        }
        landingGuideShape = ActiveShape.Clone(Color.grey, guideShapeSortingLayer);
    }

    void OnActiveShapeLanded()
    {
        inputHandler.ResetHardDrop();

        if (gridHandler.AddToGrid(ActiveShape))
        {
            Destroy(ActiveShape.gameObject);

            var completedRows = gridHandler.GetCompletedRows();
            ProcessLineRemoval(completedRows.Count);
            gridHandler.DeleteRows(completedRows);

            SpawnRandomShape(false);
        }
        else
        {
            GameOver();
        }
    }

    void ProcessLineRemoval(int clearedLines)
    {
        if (clearedLines <= 0)
        {
            scoreState.ResetCombo();
        }
        else
        {
            scoreState += LineScoreHandler.GetScore(clearedLines, scoreState);

            scoreState.IncreaseCombo();
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

    void GameOver()
    {
        IsGameOver = true;
    }

    void OnGUI()
    {
        var output = string.Empty;
        
        GUI.Label(new Rect(100, 0, 100, 100), string.Format("level: {0}", scoreState.Level));
        GUI.Label(new Rect(100, 50, 100, 100), string.Format("score: {0}", scoreState.Score));
        GUI.Label(new Rect(100, 100, 100, 100), string.Format("combo: {0}", scoreState.ComboLevel));
    }
}
