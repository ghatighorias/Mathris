using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ShapeHandler;

public class Settings : MonoBehaviour {
    public float fallDelay = 1F;
    public bool allowAutomaticDrop = true;
    public Vector3 shapeSpawnPosition = Vector3.zero;
    bool skipFallForOneFrame = false;
    ShapeHandler ActiveShape = null;
    float fallTimer = 0F;

    int comboLevel;
    int currentLevel;
    int currentScore;

    void Start()
    {
        comboLevel = 0;
        currentLevel = 0;
        currentScore = 0;

        SpawnRandomShape();
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

    void SpawnRandomShape()
    {
        ActiveShape = InstantiateRandomShape().GetComponent<ShapeHandler>();
        ActiveShape.ShapeLanded = OnActiveShapeLanded;
    }

    void OnActiveShapeLanded()
    {
        var gridHandler = FindObjectOfType<GridHandler>();

        gridHandler.AddToGrid(ActiveShape);
        Destroy(ActiveShape.gameObject);

        var completedRows = gridHandler.GetCompletedRows();
        ProcessLineRemoval(completedRows.Count);
        gridHandler.DeleteRows(completedRows);

        SpawnRandomShape();
    }

    void ProcessLineRemoval(int clearedLines)
    {
        if (clearedLines == 0)
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

    GameObject InstantiateRandomShape()
    {
        var randomShapeNumber = Random.Range(1, 7);
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
        }

        var shapeFullName = string.Format("Prefabs/{0}_{1}", shapePrefix, shapePostfix);

        return (GameObject)Instantiate(Resources.Load<GameObject>(shapeFullName), shapeSpawnPosition, Quaternion.identity);
    }

    public static RaytraceHitResultType ConvertTag(string tag)
    {
        if (tag == "GridWall")
        {
            return RaytraceHitResultType.GridWall;
        }
        else if (tag == "GridBottom")
        {
            return RaytraceHitResultType.GridBottom;
        }
        else if (tag == "Block")
        {
            return RaytraceHitResultType.Block;
        }
        else
        {
            return RaytraceHitResultType.None;
        }
    }
}
