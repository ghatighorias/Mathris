using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ShapeHandler;

public class Settings : MonoBehaviour {
    public float fallDelay = 1F;
    public bool allowAutomaticDrop = true;
    public Vector3 shapeSpawnPosition = Vector3.zero;
    public Vector3 shapeHoldPosition = Vector3.zero;
    bool skipFallForOneFrame;
    ShapeHandler ActiveShape;
    ShapeHandler NextShape;

    float fallTimer = 0F;

    [HideInInspector]
    public int comboLevel;
    [HideInInspector]
    public int currentLevel;
    [HideInInspector]
    public int currentScore;

    void Start()
    {
        comboLevel = 0;
        currentLevel = 0;
        currentScore = 0;

         
        SpawnRandomShape(true);
    }

    void Update()
    {
        skipFallForOneFrame = false;

        CheckUserInput();

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
        var gridHandler = FindObjectOfType<GridHandler>();

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
            var award = LineScoreHandler.GerScore(clearedLines, comboLevel, currentLevel);
            clearedLines += award.lineAward;
            currentScore += award.score;
            comboLevel++;
        }

    }

    void CheckUserInput()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ActiveShape.MoveShapeIfValid(Move.Down);
            skipFallForOneFrame = true;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (ActiveShape.allowRotatation)
            {
                if (ActiveShape.limitRotatation)
                {
                    ActiveShape.RotateShapeIfValid(ActiveShape.ReverseRotate);
                }
                else
                {
                    ActiveShape.RotateShapeIfValid(Rotate.ClockWise);
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ActiveShape.MoveShapeIfValid(Move.Left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ActiveShape.MoveShapeIfValid(Move.Right);
        }
    }
}
