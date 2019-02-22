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

    void Start()
    {
        ActiveShape = SpawnRandomShape().GetComponent<ShapeHandler>();
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

    GameObject SpawnRandomShape()
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


}
