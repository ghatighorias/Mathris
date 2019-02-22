using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeHandler : MonoBehaviour {

    float fallTimer = 0F;
    public float fallDelay = 1F;
    public bool skipFallForOneFrame = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        skipFallForOneFrame = false;

        CheckUserInput();

        fallTimer += Time.deltaTime;

        if(fallTimer >= fallDelay)
        {
            fallTimer = 0F;
            if (!skipFallForOneFrame)
            {
                MoveTile(PossibleSteps.Down);
            }
        }

    }

    void CheckUserInput()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveTile(PossibleSteps.Down);
            skipFallForOneFrame = true;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveTile(PossibleSteps.Rotate_Clockwise);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveTile(PossibleSteps.Left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveTile(PossibleSteps.Right);
        }
    }

    void MoveTile(PossibleSteps step)
    {
        switch (step)
        {
            case PossibleSteps.Down:
                transform.position += Vector3.down;
                break;
            case PossibleSteps.Left:
                transform.position += Vector3.left;
                break;
            case PossibleSteps.Right:
                transform.position += Vector3.right;
                break;
            case PossibleSteps.Rotate_Clockwise:
                transform.Rotate(Vector3.forward * 90);
                break;
        }
        //Input.GetKeyDown(KeyCode.DownArrow)
        
    }

    public enum PossibleSteps
    {
        Left,
        Right,
        Down,
        Rotate_Clockwise

    }
}
