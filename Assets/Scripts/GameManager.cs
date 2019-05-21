using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UiHandler)), RequireComponent(typeof(InputHandler)), RequireComponent(typeof(Game))]
public class GameManager : MonoBehaviour
{
    public delegate void GameStateChanged(GameState newState);
    public event GameStateChanged OnGameStateChanged;

    private GameState gameState = GameState.None;
    public GameState GameState
    {
        get { return gameState; }
        set
        {
            if (gameState != value)
            {
                gameState = value;
                OnGameStateChanged(gameState);
            }
        }
    }

    void Start()
    {
        GameState = GameState.NotStarted;
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    public void TogglePauseGame()
    {
        if (GameState == GameState.Paused)
        {
            GameState = GameState.Playing;
        }
        else if (GameState == GameState.Playing)
        {
            GameState = GameState.Paused;
        }
    }
}