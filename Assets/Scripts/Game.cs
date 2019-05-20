using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InputHandler))]
public class Game : MonoBehaviour {
    public float fallDelay = 1F;
    public bool allowAutomaticDrop = true;
    
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

    public float softDropDelay = 0.1F;
    float softDropTimer;

    public float hardDropDelay = 0.1F;
    float hardDropTimer;

    public Sprite[] Numbers;
    public Sprite[] Operators;

    [SerializeField]
    GameObject spawnLocationIndicator;

    [SerializeField]
    GameObject nextShapeLocationIndicator;

    [SerializeField]
    GameObject levelIndicatorText;
    [SerializeField]
    GameObject scoreIndicatorText;


    Text levelIndicatorTextComponent;
    Text scoreIndicatorTextComponent;

    [HideInInspector]
    bool IsGameOver;

    public ScoreState scoreState;

    void Awake()
    {
        levelIndicatorTextComponent = levelIndicatorText.GetComponent<Text>();
        scoreIndicatorTextComponent = scoreIndicatorText.GetComponent<Text>();
    }

    void Start()
    {
        fallTimer = 0F;
        softDropTimer = 0F;
        hardDropTimer = 0F;

        scoreState = new ScoreState(0, 0); 

        gridHandler = FindObjectOfType<GridHandler>();
        inputHandler = GetComponent<InputHandler>();

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

        var landingOffset = ActiveShape.GetShapeLandingOffset(gridHandler.Bottom);
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

    void HandleHardDrop()
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

        ActiveShape.gameObject.SetActive(false);

        ActiveShape.gameObject.transform.position = spawnLocationIndicator != null ? spawnLocationIndicator.transform.position : Vector3.zero;
        ActiveShape.ShapeLanded = OnActiveShapeLanded;

        NextShape = ShapeHandler.InstantiateRandomShape(shapeSortingLayer);
        NextShape.gameObject.transform.position = nextShapeLocationIndicator != null ? nextShapeLocationIndicator.transform.position : Vector3.zero; ;

        if (ActiveShape.OverlapsAnotherShape())
        {
            GameOver();
        }
        else
        {
            ActiveShape.gameObject.SetActive(true);

            if (landingGuideShape)
            {
                Destroy(landingGuideShape.gameObject);
            }

            landingGuideShape = ActiveShape.Clone(Color.grey, guideShapeSortingLayer);
        }
    }

    void OnActiveShapeLanded()
    {
        inputHandler.ResetHardDrop();

        if (gridHandler.AddToGrid(ActiveShape))
        {
            Destroy(ActiveShape.gameObject);

            var completedRows = gridHandler.GetCompletedRows();

            CheckMathEquation(completedRows);

            ProcessLineRemoval(completedRows.Count);

            gridHandler.DeleteRows(completedRows);

            SpawnRandomShape(false);
        }
        else
        {
            GameOver();
        }
    }

    void CheckMathEquation(List<int> clearedRowIndexes)
    {
        List<string> mathEquations = new List<string>();
        int equationCounter = 0;

        foreach (var row in clearedRowIndexes)
        {
            foreach (var block in gridHandler.GetRowBlocks(row))
            {
                var mathBlock = block.transform.GetComponentInChildren<MathBlock>();
                if (mathBlock)
                {
                    mathEquations[equationCounter] += mathBlock.GetStringValue();
                }
            }

            equationCounter++;
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

        levelIndicatorTextComponent.text = scoreState.Level.ToString();
        scoreIndicatorTextComponent.text = scoreState.Score.ToString();
    }

    void MapInputToAction()
    {
        var userAction = inputHandler.ActionMapper;

        if (userAction.hardDrop)
        {
            HandleHardDrop();
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

    Sprite GetMathItemSprite()
    {
        return Numbers[0];
    }

    void GameOver()
    {
        IsGameOver = true;
        Scenes.Instance.LoadLevel(SceneOptions.Menu);
    }
}
