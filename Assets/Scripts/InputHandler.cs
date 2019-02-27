using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [HideInInspector]
    public InputActionMapper ActionMapper = new InputActionMapper();

    void Update()
    {
        GetUserAction();
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ActionMapper.hardDrop = true;
        }

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