using UnityEngine;
using UnityEngine.SceneManagement;

class Scenes
{
    string menuSceneName;
    string gameSceneName;

    public bool IsGameOver
    {
        get;
        private set;
    }

    static Scenes instance;
    public static Scenes Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Scenes();
            }

            return instance;
        }
    }

    string ActiveSceneName => SceneManager.GetActiveScene().name;

    string SceneOptionToSceneName(SceneOptions scene) => scene == SceneOptions.Menu ? menuSceneName : gameSceneName;

    public void SetSceneNames(string gameSceneName, string menuSceneName)
    {
        this.gameSceneName = gameSceneName;
        this.menuSceneName = menuSceneName;
    }

    private Scenes() { }

    private void SetIsGameOver(SceneOptions desiredScene)
    {
        IsGameOver = ActiveSceneName == SceneOptionToSceneName(SceneOptions.Game) && desiredScene == SceneOptions.Menu;
    }

    public void LoadLevel(SceneOptions scene)
    {
        var desiredSceneName = SceneOptionToSceneName(scene);

        if (desiredSceneName != ActiveSceneName)
        {
            SetIsGameOver(scene);

            SceneManager.LoadScene(desiredSceneName);
        }
    }
}