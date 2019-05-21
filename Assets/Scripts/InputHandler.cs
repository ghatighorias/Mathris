﻿using UnityEngine;

public class InputHandler : MonoBehaviour
{
    GameManager gameManager;

    [HideInInspector]
    public InputActionMapper ActionMapper = new InputActionMapper();

    void Awake()
    {
        if (gameManager == null)
        {
            gameManager = GetComponent<GameManager>();
        }
    }

    void Update()
    {
        if (gameManager?.GameState == GameState.Playing)
        {
            GetUserAction();
        }

        if (Input.GetButtonDown("Pause"))
        {
            gameManager.TogglePauseGame();
        }
    }

    public void GetUserAction()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ActionMapper.softDrop = true;
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            ActionMapper.softDrop = false;
        }

        ActionMapper.hardDrop |= Input.GetKeyDown(KeyCode.Space);

        ActionMapper.rotate = Input.GetKeyDown(KeyCode.UpArrow);
        ActionMapper.moveLeft = Input.GetKeyDown(KeyCode.LeftArrow);
        ActionMapper.moveRight = Input.GetKeyDown(KeyCode.RightArrow);
    }

    public void ResetHardDrop()
    {
        ActionMapper.hardDrop = false;
    }
}


public class InputActionMapper
{
    public bool softDrop;
    public bool hardDrop;
    public bool moveLeft;
    public bool moveRight;
    public bool rotate;
}