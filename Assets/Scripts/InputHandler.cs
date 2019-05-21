using UnityEngine;

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
            UpdateUserAction();
        }

        if (Input.GetButtonDown("Pause"))
        {
            gameManager.TogglePauseGame();
        }
    }

    public void UpdateUserAction()
    {
        ActionMapper.rotate = Input.GetButtonDown("Rotate");
        ActionMapper.softDrop = Input.GetButton("SoftDrop");
        ActionMapper.hardDrop |= Input.GetButtonDown("HardDrop");
        ActionMapper.moveLeft = Input.GetButtonDown("MoveLeft");
        ActionMapper.moveRight = Input.GetButtonDown("MoveRight");
    }

    public void ResetHardDrop()
    {
        ActionMapper.hardDrop = false;
    }
}