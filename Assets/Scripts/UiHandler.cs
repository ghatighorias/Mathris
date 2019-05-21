using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiHandler : MonoBehaviour
{
    GameManager gameManager;

    [SerializeField]
    GameObject pauseMenu;
    [SerializeField]
    GameObject startMenu;

    void Awake()
    {
        if (gameManager == null)
        {
            gameManager = GetComponent<GameManager>();
        }

        if (pauseMenu == null)
        {
            pauseMenu = GameObject.Find("PauseMenu");
        }

        if (startMenu == null)
        {
            startMenu = GameObject.Find("StartMenu");
        }

        gameManager.OnGameStateChanged += GameManager_OnGameStateChanged;
    }

    void GameManager_OnGameStateChanged(GameState newState)
    {
        CloseAllMenus();
        
        switch (newState)
        {
            case GameState.NotStarted:
            case GameState.Over:
                startMenu?.SetActive(true);
                break;
            case GameState.Paused:
                pauseMenu?.SetActive(true);
                break;
        }
    }

    void CloseAllMenus()
    {
        pauseMenu?.SetActive(false);
        startMenu?.SetActive(false);
    }

    public void StartGame()
    {
        gameManager.GameState = GameState.Playing;
    }

    public void QuitGame()
    {
        gameManager.Quit();
    }
}
