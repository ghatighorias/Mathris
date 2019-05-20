using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public string menuSceneName;
    public string gameSceneName;

    void Awake()
    {
        Scenes.Instance.SetSceneNames(gameSceneName, menuSceneName);
    }

    public void LoadGame() => Scenes.Instance.LoadLevel(SceneOptions.Game);
    public void LoadMenu() => Scenes.Instance.LoadLevel(SceneOptions.Menu);
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
